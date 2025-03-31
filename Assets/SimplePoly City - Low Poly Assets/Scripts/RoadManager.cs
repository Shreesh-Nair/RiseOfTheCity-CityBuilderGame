using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Road Prefabs")]
    public GameObject straightRoadPrefab;    // Normal lane (N key)
    public GameObject lTurnRoadPrefab;       // L turn/junction (L key)
    public GameObject tJunctionRoadPrefab;   // T junction (T key)
    public GameObject intersectionRoadPrefab; // 4-way intersection (X key)
    
    [Header("References")]
    public GridManager gridManager;
    public LayerMask gridLayer;
    
    [Header("Preview Materials")]
    public Material validPlacementMaterial;   // Green material
    public Material invalidPlacementMaterial; // Red material
    
    // Road tracking for traffic simulation
    private List<GameObject> placedRoads = new List<GameObject>();
    private Dictionary<Vector2Int, GameObject> roadGrid = new Dictionary<Vector2Int, GameObject>();
    private List<Vector3> roadNodes = new List<Vector3>(); // For traffic waypoints
    
    // Preview and placement variables
    private GameObject currentRoadPrefab;
    private GameObject previewObject;
    private Vector3 lastValidPosition;
    private bool canPlace = false;
    private float currentRotation = 0f;
    
    void Start()
    {
        // Initialize GridManager reference if not set in inspector
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
            
        // Initialize materials if not set
        if (validPlacementMaterial == null)
        {
            validPlacementMaterial = new Material(Shader.Find("Standard"));
            validPlacementMaterial.color = new Color(0, 1, 0, 0.5f); // Semi-transparent green
        }
        
        if (invalidPlacementMaterial == null)
        {
            invalidPlacementMaterial = new Material(Shader.Find("Standard"));
            invalidPlacementMaterial.color = new Color(1, 0, 0, 0.5f); // Semi-transparent red
        }
    }
    
    void Update()
    {
        HandleRoadSelection();
        HandleRotation();
        HandleRoadPlacement();
        HandleRoadRemoval();
    }
    
    void HandleRoadSelection()
    {
        // Select road type with keyboard keys
        if (Input.GetKeyDown(KeyCode.N)) SelectRoad(straightRoadPrefab);
        if (Input.GetKeyDown(KeyCode.L)) SelectRoad(lTurnRoadPrefab);
        if (Input.GetKeyDown(KeyCode.T)) SelectRoad(tJunctionRoadPrefab);
        if (Input.GetKeyDown(KeyCode.X)) SelectRoad(intersectionRoadPrefab);
        
        // Cancel road placement mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearPreview();
            currentRoadPrefab = null;
        }
    }
    
    void SelectRoad(GameObject roadPrefab)
    {
        // Update selection
        currentRoadPrefab = roadPrefab;
        
        // Reset rotation when selecting a new road
        // Fix for straight road initial orientation
        if (roadPrefab == straightRoadPrefab)
        {
            currentRotation = 90f; // Start with a 90-degree rotation for straight roads
        }
        else
        {
            currentRotation = 0f;
        }
        
        // Clear old preview
        ClearPreview();
        
        // Create new preview
        if (currentRoadPrefab != null)
        {
            previewObject = Instantiate(currentRoadPrefab);
            
            // Apply initial rotation
            previewObject.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
            
            // Disable any components that might interfere with preview
            Collider[] colliders = previewObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            
            // Make all child renderers semi-transparent
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = validPlacementMaterial;
            }
        }
    }
    
    void HandleRotation()
    {
        if (previewObject != null && Input.GetKeyDown(KeyCode.R))
        {
            // Add 90 degrees to current rotation
            currentRotation += 90f;
            
            // Reset to 0 if we've made a full 360
            if (currentRotation >= 360f)
            {
                currentRotation = 0f;
            }
            
            // Apply rotation to preview object
            previewObject.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
    }
    
    void ClearPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }
    
    void HandleRoadPlacement()
    {
        if (currentRoadPrefab == null || previewObject == null) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            // Fix for placement issue - directly use hit point for node lookup
            Vector3 hitPosition = hit.point;
            Node node = gridManager.GetNodeFromWorldPosition(hitPosition);
            
            if (node != null)
            {
                // Use the exact world position from the node to ensure proper alignment
                Vector3 snappedPosition = new Vector3(
                    node.worldPosition.x,
                    0.01f, // Slightly above ground to avoid z-fighting
                    node.worldPosition.z
                );
                
                // Check if the node is empty - fix for placement issue
                canPlace = node.isEmpty;
                
                // Update preview position
                previewObject.transform.position = snappedPosition;
                lastValidPosition = snappedPosition;
                
                // Update preview material based on placement validity
                Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = canPlace ? validPlacementMaterial : invalidPlacementMaterial;
                }
                
                // Place road on mouse click
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceRoad(lastValidPosition);
                }
            }
        }
        else
        {
            // If no raycast hit, use a ground plane approach similar to BuildingManager
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                
                // Snap to grid - Fix for placement issue
                int x = Mathf.FloorToInt(worldPosition.x / gridManager.cellSize);
                int z = Mathf.FloorToInt(worldPosition.z / gridManager.cellSize);
                Vector3 snappedPosition = new Vector3(
                    x * gridManager.cellSize,
                    0.01f,
                    z * gridManager.cellSize
                );
                
                // Get the node at this position
                Node node = gridManager.GetNodeFromWorldPosition(snappedPosition);
                if (node != null)
                {
                    // Use the exact world position from the node
                    snappedPosition = new Vector3(
                        node.worldPosition.x,
                        0.01f,
                        node.worldPosition.z
                    );
                    
                    canPlace = node.isEmpty;
                }
                else
                {
                    canPlace = false;
                }
                
                // Update preview
                previewObject.transform.position = snappedPosition;
                lastValidPosition = snappedPosition;
                
                // Update material
                Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = canPlace ? validPlacementMaterial : invalidPlacementMaterial;
                }
                
                // Place road on click
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceRoad(lastValidPosition);
                }
            }
        }
    }
    
    private void PlaceRoad(Vector3 position)
    {
        // Create the actual road
        GameObject placedRoad = Instantiate(currentRoadPrefab, position, Quaternion.Euler(0f, currentRotation, 0f));
        placedRoad.tag = "Road"; // Tag it for easy identification
        
        // Restore original materials for the placed road
        Renderer[] roadRenderers = placedRoad.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < roadRenderers.Length && i < currentRoadPrefab.GetComponentsInChildren<Renderer>().Length; i++)
        {
            // Try to get the original material from the prefab
            Renderer prefabRenderer = currentRoadPrefab.GetComponentsInChildren<Renderer>()[i];
            roadRenderers[i].material = prefabRenderer.sharedMaterial;
        }
        
        // Mark tile as occupied
        gridManager.SetNodeOccupied(position, true, 1);
        
        // Add to road tracking collections for traffic simulation
        placedRoads.Add(placedRoad);
        
        // Store in grid dictionary for easy lookup
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(position.x / gridManager.cellSize),
            Mathf.RoundToInt(position.z / gridManager.cellSize)
        );
        roadGrid[gridPos] = placedRoad;
        
        // Add to road nodes for traffic waypoints
        roadNodes.Add(position);
        
        // Debug grid occupancy
        gridManager.DebugPrintGridOccupancy();
    }
    
    // New method for handling road removal
    void HandleRoadRemoval()
    {
        if (currentRoadPrefab == null && Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Approximate coordinates to nearest grid cell
                Vector3 approximatedHitPoint = new Vector3(
                    Mathf.Round(hit.point.x / gridManager.cellSize) * gridManager.cellSize,
                    0.01f, // Keep at ground level
                    Mathf.Round(hit.point.z / gridManager.cellSize) * gridManager.cellSize
                );
                
                // Check if there's a road at this position
                Vector2Int gridPos = new Vector2Int(
                    Mathf.RoundToInt(approximatedHitPoint.x / gridManager.cellSize),
                    Mathf.RoundToInt(approximatedHitPoint.z / gridManager.cellSize)
                );
                
                if (roadGrid.ContainsKey(gridPos))
                {
                    GameObject roadToRemove = roadGrid[gridPos];
                    
                    // Remove from tracking collections
                    placedRoads.Remove(roadToRemove);
                    roadGrid.Remove(gridPos);
                    
                    // Remove from road nodes
                    roadNodes.RemoveAll(node => Vector3.Distance(node, approximatedHitPoint) < 0.1f);
                    
                    // Mark grid as unoccupied
                    gridManager.SetNodeOccupied(approximatedHitPoint, false, 1);
                    
                    // Destroy the road object
                    Destroy(roadToRemove);
                    
                    Debug.Log("Road removed at: " + approximatedHitPoint);
                    
                    // Debug grid occupancy
                    gridManager.DebugPrintGridOccupancy();
                }
                else
                {
                    // Alternative approach - check all road objects
                    foreach (GameObject road in placedRoads.ToArray())
                    {
                        if (road != null && Vector3.Distance(road.transform.position, approximatedHitPoint) < 0.1f)
                        {
                            // Remove from tracking collections
                            placedRoads.Remove(road);
                            
                            // Find and remove from grid dictionary
                            foreach (var kvp in new Dictionary<Vector2Int, GameObject>(roadGrid))
                            {
                                if (kvp.Value == road)
                                {
                                    roadGrid.Remove(kvp.Key);
                                    break;
                                }
                            }
                            
                            // Remove from road nodes
                            roadNodes.RemoveAll(node => Vector3.Distance(node, road.transform.position) < 0.1f);
                            
                            // Mark grid as unoccupied
                            gridManager.SetNodeOccupied(road.transform.position, false, 1);
                            
                            // Destroy the road object
                            Destroy(road);
                            
                            Debug.Log("Road removed at: " + road.transform.position);
                            
                            // Debug grid occupancy
                            gridManager.DebugPrintGridOccupancy();
                            break;
                        }
                    }
                }
            }
        }
    }
    
    // Methods for traffic simulation
    public List<Vector3> GetRoadNodes()
    {
        return roadNodes;
    }
    
    public bool IsRoadAt(Vector3 worldPosition)
    {
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / gridManager.cellSize),
            Mathf.RoundToInt(worldPosition.z / gridManager.cellSize)
        );
        
        return roadGrid.ContainsKey(gridPos);
    }
    
    public GameObject GetRoadAt(Vector3 worldPosition)
    {
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / gridManager.cellSize),
            Mathf.RoundToInt(worldPosition.z / gridManager.cellSize)
        );
        
        if (roadGrid.ContainsKey(gridPos))
            return roadGrid[gridPos];
            
        return null;
    }
    
    // Get all connected road positions from a starting point (for pathfinding)
    public List<Vector3> GetConnectedRoads(Vector3 startPosition)
    {
        List<Vector3> connectedRoads = new List<Vector3>();
        Vector2Int startGridPos = new Vector2Int(
            Mathf.RoundToInt(startPosition.x / gridManager.cellSize),
            Mathf.RoundToInt(startPosition.z / gridManager.cellSize)
        );
        
        // Check in four directions
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };
        
        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = startGridPos + dir;
            if (roadGrid.ContainsKey(checkPos))
            {
                connectedRoads.Add(new Vector3(
                    checkPos.x * gridManager.cellSize,
                    0.01f,
                    checkPos.y * gridManager.cellSize
                ));
            }
        }
        
        return connectedRoads;
    }
}

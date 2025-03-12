using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array of building prefabs (assign in Inspector)
    public GridManager gridManager; // Reference to your GridManager
    public LayerMask gridLayer; // Set this to "Grid" layer in the inspector
    public Material validPlacementMaterial; // Material when placement is valid (green)
    public Material invalidPlacementMaterial; // Material when placement is invalid (red)

    private GameObject currentBuildingPrefab; // Currently selected building
    private GameObject previewObject; // Preview version of the building before placement
    private Vector3 lastValidPosition; // Stores last valid placement position
    private bool canPlace = false; // Tracks whether placement is allowed
    private int selectedBuildingIndex = -1; // Currently selected building index

    void Start()
    {
        // Initialize GridManager reference if not set in inspector
        if (gridManager == null)
            gridManager = FindFirstObjectByType<GridManager>();
            
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
        HandleBuildingSelection();
        HandleBuildingPlacement();
    }

    void HandleBuildingSelection()
    {
        // Select a building with number keys (1, 2, 3...)
        if (Input.GetKeyDown(KeyCode.Alpha1) && buildingPrefabs.Length > 0) SelectBuilding(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && buildingPrefabs.Length > 1) SelectBuilding(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && buildingPrefabs.Length > 2) SelectBuilding(2);

        // Cancel building mode
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            ClearPreview();
            selectedBuildingIndex = -1;
            currentBuildingPrefab = null;
        }
    }

    void SelectBuilding(int index)
    {
        // Update selection
        selectedBuildingIndex = index;
        currentBuildingPrefab = buildingPrefabs[index];

        // Clear old preview
        ClearPreview();

        // Create new preview
        previewObject = Instantiate(currentBuildingPrefab);
        
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

    void ClearPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }

    void HandleBuildingPlacement()
    {
        if (currentBuildingPrefab == null || previewObject == null) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        // Debug ray
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            // Debug hit
            Debug.Log("Hit at position: " + hit.point);
            
            // Get the corresponding node from GridManager
            Node node = gridManager.GetNodeFromWorldPosition(hit.point);
            
            if (node != null)
            {
                // Debug node info
                Debug.Log("Found node at: " + node.worldPosition + ", Empty: " + node.isEmpty);
                
                // Snap to grid cell
                Vector3 snappedPosition = new Vector3(
                    node.worldPosition.x,
                    0, // Keep at ground level
                    node.worldPosition.z
                );
                
                // Check if the node is empty
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
                
                // Place building on mouse click
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    GameObject placedBuilding = Instantiate(currentBuildingPrefab, lastValidPosition, Quaternion.identity);
                    
                    // Restore original materials for the placed building
                    Renderer[] buildingRenderers = placedBuilding.GetComponentsInChildren<Renderer>();
                    for (int i = 0; i < buildingRenderers.Length && i < currentBuildingPrefab.GetComponentsInChildren<Renderer>().Length; i++)
                    {
                        // Try to get the original material from the prefab
                        Renderer prefabRenderer = currentBuildingPrefab.GetComponentsInChildren<Renderer>()[i];
                        buildingRenderers[i].material = prefabRenderer.sharedMaterial;
                    }
                    
                    // Mark the node as occupied
                    gridManager.SetNodeOccupied(lastValidPosition, true);
                    
                    // Debug grid occupancy
                    gridManager.DebugPrintGridOccupancy();
                }
            }
            else
            {
                // If hit but no valid node, show as invalid placement
                canPlace = false;
                
                // Update preview material to invalid
                Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = invalidPlacementMaterial;
                }
            }
        }
        else
        {
            // If no hit at all, hide or indicate invalid placement
            canPlace = false;
            Debug.Log("No hit detected");
            
            // Update preview material to invalid
            if (previewObject != null)
            {
                Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = invalidPlacementMaterial;
                }
            }
        }
    }
}
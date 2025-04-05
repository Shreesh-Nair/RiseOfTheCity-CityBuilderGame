using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Road Prefabs")]
    public GameObject straightRoadPrefab;
    public GameObject lTurnRoadPrefab;
    public GameObject tJunctionRoadPrefab;
    public GameObject intersectionRoadPrefab;

    [Header("References")]
    public GridManager gridManager;
    public LayerMask gridLayer;

    [Header("Preview Materials")]
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    private List<GameObject> placedRoads = new List<GameObject>();
    private Dictionary<Vector2Int, GameObject> roadGrid = new Dictionary<Vector2Int, GameObject>();
    private List<Vector3> roadNodes = new List<Vector3>();

    private GameObject currentRoadPrefab;
    private GameObject previewObject;
    private Vector3 lastValidPosition;
    private bool canPlace = false;
    private float currentRotation = 0f;

    void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();

        if (validPlacementMaterial == null)
        {
            validPlacementMaterial = new Material(Shader.Find("Standard"));
            validPlacementMaterial.color = new Color(0, 1, 0, 0.5f);
        }

        if (invalidPlacementMaterial == null)
        {
            invalidPlacementMaterial = new Material(Shader.Find("Standard"));
            invalidPlacementMaterial.color = new Color(1, 0, 0, 0.5f);
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
        if (Input.GetKeyDown(KeyCode.N)) SelectRoad(straightRoadPrefab);
        if (Input.GetKeyDown(KeyCode.L)) SelectRoad(lTurnRoadPrefab);
        if (Input.GetKeyDown(KeyCode.T)) SelectRoad(tJunctionRoadPrefab);
        if (Input.GetKeyDown(KeyCode.X)) SelectRoad(intersectionRoadPrefab);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearPreview();
            currentRoadPrefab = null;
        }
    }

    void SelectRoad(GameObject roadPrefab)
    {
        currentRoadPrefab = roadPrefab;
        currentRotation = (roadPrefab == straightRoadPrefab) ? 90f : 0f; // Set initial rotation for straight road
        ClearPreview();
        if (currentRoadPrefab != null)
        {
            previewObject = Instantiate(currentRoadPrefab);
            previewObject.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
            Collider[] colliders = previewObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
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
            currentRotation += 90f;
            if (currentRotation >= 360f)
            {
                currentRotation = 0f;
            }
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
            Vector3 hitPosition = hit.point;
            Vector3 snappedPosition = SnapToGrid(hitPosition);
            UpdatePreview(snappedPosition);

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceRoad(lastValidPosition);
            }
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                Vector3 snappedPosition = SnapToGrid(worldPosition);
                UpdatePreview(snappedPosition);

                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceRoad(lastValidPosition);
                }
            }
        }
    }

    Vector3 SnapToGrid(Vector3 worldPosition)
{
    int x = Mathf.RoundToInt(worldPosition.x / gridManager.cellSize);
    int z = Mathf.RoundToInt(worldPosition.z / gridManager.cellSize);
    return new Vector3(
        x * gridManager.cellSize,
        0.01f,
        z * gridManager.cellSize
    );
}

    void UpdatePreview(Vector3 position)
    {
        Node node = gridManager.GetNodeFromWorldPosition(position);
        if (node != null)
        {
            canPlace = node.isEmpty;
            previewObject.transform.position = position;
            lastValidPosition = position;

            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = canPlace ? validPlacementMaterial : invalidPlacementMaterial;
            }
        }
        else
        {
            canPlace = false;
        }
    }

    private void PlaceRoad(Vector3 position)
    {
        GameObject placedRoad = Instantiate(currentRoadPrefab, position, Quaternion.Euler(0f, currentRotation, 0f));
        placedRoad.tag = "Road";

        Renderer[] roadRenderers = placedRoad.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < roadRenderers.Length && i < currentRoadPrefab.GetComponentsInChildren<Renderer>().Length; i++)
        {
            Renderer prefabRenderer = currentRoadPrefab.GetComponentsInChildren<Renderer>()[i];
            roadRenderers[i].material = prefabRenderer.sharedMaterial;
        }

        gridManager.SetNodeOccupied(position, true, 1);
        placedRoads.Add(placedRoad);

        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(position.x / gridManager.cellSize),
            Mathf.RoundToInt(position.z / gridManager.cellSize)
        );
        roadGrid[gridPos] = placedRoad;
        roadNodes.Add(position);

        gridManager.DebugPrintGridOccupancy();
    }

    void HandleRoadRemoval()
    {
        if (currentRoadPrefab == null && Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 hitPoint = SnapToGrid(hit.point);
                Vector2Int gridPos = new Vector2Int(
                    Mathf.RoundToInt(hitPoint.x / gridManager.cellSize),
                    Mathf.RoundToInt(hitPoint.z / gridManager.cellSize)
                );

                if (roadGrid.ContainsKey(gridPos))
                {
                    GameObject roadToRemove = roadGrid[gridPos];
                    placedRoads.Remove(roadToRemove);
                    roadGrid.Remove(gridPos);
                    roadNodes.Remove(hitPoint);
                    gridManager.SetNodeOccupied(hitPoint, false, 1);
                    Destroy(roadToRemove);
                    gridManager.DebugPrintGridOccupancy();
                }
            }
        }
    }

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
        return roadGrid.ContainsKey(gridPos) ? roadGrid[gridPos] : null;
    }

    public List<Vector3> GetConnectedRoads(Vector3 startPosition)
    {
        List<Vector3> connectedRoads = new List<Vector3>();
        Vector2Int startGridPos = new Vector2Int(
            Mathf.RoundToInt(startPosition.x / gridManager.cellSize),
            Mathf.RoundToInt(startPosition.z / gridManager.cellSize)
        );

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

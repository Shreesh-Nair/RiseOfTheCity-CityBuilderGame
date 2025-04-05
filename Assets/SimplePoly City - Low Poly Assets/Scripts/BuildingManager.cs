using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array of building prefabs (assign in Inspector)
    public GridManager gridManager; // Reference to your GridManager
    public LayerMask gridLayer; // Set this to "Grid" layer in the inspector
    public Material validPlacementMaterial; // Material when placement is valid (green)
    public Material invalidPlacementMaterial; // Material when placement is invalid (red)
    public int populationLimit = 0;
    public Text populationValue;
    private GameObject currentBuildingPrefab;
    private BuildingDatabase.BuildingData currentAsset; // Currently selected building
    private GameObject previewObject; // Preview version of the building before placement
    private Vector3 lastValidPosition; // Stores last valid placement position
    private bool canPlace = false; // Tracks whether placement is allowed
    private int selectedBuildingIndex = -1; // Currently selected building index
    private float currentRotation = 0f; // Current rotation of the building (in y-axis)
    public int totalCommercialProduction = 0;

    public BuildingDatabase.BuildingData[] buildingData;
    public int maintainanceFactor = 0;
    public int pollutionFactor = 0;
    public HandleBuildingSelection buildingSelectionHandler;
    public int totalBuildings = 0;
    void Start()
    {
        if (buildingSelectionHandler == null)
        {
            buildingSelectionHandler = FindFirstObjectByType<HandleBuildingSelection>();
        }
        if (buildingSelectionHandler != null)
        {
            buildingData = buildingSelectionHandler.buildingData;
        }
        buildingPrefabs[0] = buildingData[0].prefab;
        buildingPrefabs[1] = buildingData[1].prefab;
        buildingPrefabs[2] = buildingData[2].prefab;
        buildingPrefabs[3] = buildingData[3].prefab;
        buildingPrefabs[4] = buildingData[4].prefab;
        buildingPrefabs[5] = buildingData[5].prefab;
        buildingPrefabs[6] = buildingData[6].prefab;
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
        HandleRotation(); // Add rotation handling
        HandleBuildingPlacement();
        HandleBuildingRemoval();
    }

    void HandleBuildingSelection()
    {
        // Select a building with number keys (1, 2, 3...)
        if (Input.GetKeyDown(KeyCode.Alpha1) && buildingPrefabs.Length > 0) SelectBuilding(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && buildingPrefabs.Length > 1) SelectBuilding(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && buildingPrefabs.Length > 2) SelectBuilding(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && buildingPrefabs.Length > 3) SelectBuilding(3);
        if (Input.GetKeyDown(KeyCode.Alpha5) && buildingPrefabs.Length > 4) SelectBuilding(4);
        if (Input.GetKeyDown(KeyCode.Alpha6) && buildingPrefabs.Length > 5) SelectBuilding(5);
        if (Input.GetKeyDown(KeyCode.Alpha7) && buildingPrefabs.Length > 6) SelectBuilding(6);
        if (Input.GetKeyDown(KeyCode.Alpha8) && buildingPrefabs.Length > 7) SelectBuilding(7);
        // Cancel building mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearPreview();
            selectedBuildingIndex = -1;
            currentBuildingPrefab = null;
        }
    }

    // Add new method to handle rotation
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

            //Debug.Log("Rotated building to: " + currentRotation + " degrees");
        }
    }

    void HandleBuildingRemoval()
    {
        if (currentBuildingPrefab == null && Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Approximate coordinates to nearest grid cell
                Vector3 approximatedHitPoint = new Vector3(
                    Mathf.Round(hit.point.x / gridManager.cellSize) * gridManager.cellSize,
                    0, // Keep at ground level
                    Mathf.Round(hit.point.z / gridManager.cellSize) * gridManager.cellSize
                );

                // Iterate through all game objects in the scene
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject obj in allObjects)
                {
                    // Check if the object's position matches the approximated hit point
                    if (Vector3.Distance(obj.transform.position, approximatedHitPoint) < 0.1f)
                    {
                        // Check if the object is tagged as "Building"
                        if (obj.CompareTag("Building"))
                        {
                            // Get the node position
                            Node node = gridManager.GetNodeFromWorldPosition(obj.transform.position);
                            if (node != null)
                            {
                                string destroyerPrefab = obj.name;
                                int tileSize = 0;
                                destroyerPrefab = destroyerPrefab.Replace("(Clone)", "");

                                for (int i = 0; i < buildingData.Length; i++)
                                {
                                    string prefabName = buildingData[i].prefab.name;
                                    if (destroyerPrefab == prefabName)
                                    {
                                        Debug.Log(prefabName + " --- " + destroyerPrefab);
                                        tileSize = buildingData[i].tileSize;
                                        Debug.Log("Found tile size: " + tileSize);
                                        break;
                                    }
                                }

                                for (int i = 0; i < buildingPrefabs.Length; i++)
                                {
                                    if (buildingPrefabs[i].name == destroyerPrefab)
                                    {
                                        populationLimit -= buildingData[i].populationCapacity;
                                        pollutionFactor -= buildingData[i].pollutionFactor;
                                        maintainanceFactor -= buildingData[i].maintenanceCost;
                                        totalCommercialProduction -= buildingData[i].commercialProduction;
                                        populationValue.text = populationLimit.ToString();
                                        totalBuildings--;
                                        break;
                                    }
                                }

                                // Mark all tiles as unoccupied based on the building's size
                                Vector3 buildingPosition = obj.transform.position;
                                gridManager.SetNodeOccupied(buildingPosition, false, tileSize);

                                // Remove the building
                                Destroy(obj);
                                Debug.Log("Building removed at approximated coordinates: " + approximatedHitPoint);

                                // Debug grid occupancy
                                gridManager.DebugPrintGridOccupancy();
                            }
                            break;
                        }
                    }
                }
            }
        }
    }




    void SelectBuilding(int index)
    {
        // Update selection
        selectedBuildingIndex = index;
        currentBuildingPrefab = buildingPrefabs[index];
        currentAsset = buildingData[index];

        // Reset rotation when selecting a new building
        currentRotation = 0f;

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
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null) return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            Node node = gridManager.GetNodeFromWorldPosition(hit.point);
            if (node != null)
            {
                Vector3 snappedPosition = new Vector3(
                    node.worldPosition.x,
                    0.01f,
                    node.worldPosition.z
                );

                // Get the tile size from the current asset
                int tileSize = currentAsset.tileSize;

                // Check if all required tiles are empty
                canPlace = true;

                // Convert to grid coordinates
                int centerX = Mathf.FloorToInt(snappedPosition.x / gridManager.cellSize);
                int centerY = Mathf.FloorToInt(snappedPosition.z / gridManager.cellSize);

                // Check all grid cells that would be occupied by this building
                for (int x = centerX - (tileSize - 1); x <= centerX + (tileSize - 1); x++)
                {
                    for (int y = centerY - (tileSize - 1); y <= centerY + (tileSize - 1); y++)
                    {
                        // Check if coordinates are within grid bounds
                        if (x < 0 || x >= gridManager.width || y < 0 || y >= gridManager.height)
                        {
                            canPlace = false;
                            break;
                        }

                        // Check if the node is empty
                        if (!gridManager.grid[x, y].isEmpty)
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    if (!canPlace) break;
                }

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
                    PlaceBuilding(lastValidPosition);
                }
            }
            else
            {
                // If hit but no valid node, show as invalid placement
                canPlace = false;
                Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = invalidPlacementMaterial;
                }
            }
        }
        else
        {
            // If no raycast hit, use a ground plane approach
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (groundPlane.Raycast(ray, out distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                // Snap to grid
                float x = Mathf.Floor(worldPosition.x / gridManager.cellSize) * gridManager.cellSize;
                float z = Mathf.Floor(worldPosition.z / gridManager.cellSize) * gridManager.cellSize;
                Vector3 snappedPosition = new Vector3(x, 0, z);

                // Get the tile size from the current asset
                int tileSize = currentAsset.tileSize;

                // Check if all required tiles are empty
                canPlace = true;
                for (int i = -tileSize + 1; i < tileSize; i++)
                {
                    for (int j = -tileSize + 1; j < tileSize; j++)
                    {
                        Vector3 tilePosition = new Vector3(
                            snappedPosition.x + (i * gridManager.cellSize),
                            0.01f,
                            snappedPosition.z + (j * gridManager.cellSize)
                        );

                        Node tileNode = gridManager.GetNodeFromWorldPosition(tilePosition);

                        if (tileNode == null || !tileNode.isEmpty)
                        {
                            canPlace = false;
                            break;
                        }
                    }
                    if (!canPlace) break;
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

                // Place building on click
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceBuilding(lastValidPosition);
                }
            }
        }
    }


    // Extracted building placement logic to avoid duplicated code
    private void PlaceBuilding(Vector3 position)
    {
        int tileSize = currentAsset.tileSize;

        // Create the actual building
        GameObject placedBuilding = Instantiate(currentBuildingPrefab, position, Quaternion.Euler(0f, currentRotation, 0f));

        string prefabName = currentBuildingPrefab.name;
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            if (buildingPrefabs[i].name == prefabName)
            {
                populationLimit += buildingData[i].populationCapacity;
                pollutionFactor += buildingData[i].pollutionFactor;
                maintainanceFactor += buildingData[i].maintenanceCost;
                totalCommercialProduction += buildingData[i].commercialProduction;
                populationValue.text = populationLimit.ToString();
            }
        }

        // Restore original materials for the placed building
        Renderer[] buildingRenderers = placedBuilding.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < buildingRenderers.Length && i < currentBuildingPrefab.GetComponentsInChildren<Renderer>().Length; i++)
        {
            // Try to get the original material from the prefab
            Renderer prefabRenderer = currentBuildingPrefab.GetComponentsInChildren<Renderer>()[i];
            buildingRenderers[i].material = prefabRenderer.sharedMaterial;
        }

        // Mark all tiles as occupied based on the building's size
        gridManager.SetNodeOccupied(position, true, tileSize);

        // Debug grid occupancy
        gridManager.DebugPrintGridOccupancy();
        totalBuildings++;
    }


}
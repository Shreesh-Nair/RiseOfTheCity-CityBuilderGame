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
    public int totalCommercialProduction=0;
    
    public BuildingDatabase.BuildingData[] buildingData;
    public int taxFactor=0;
    public int maintainanceFactor=0;
    public int rawMaterialFactor=0;
    public int foodMaterailFactor=0;
    public int pollutionFactor=0;
    public HandleBuildingSelection buildingSelectionHandler;
    public int totalBuildings=0;
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
        buildingPrefabs[0]=buildingData[0].prefab;
        buildingPrefabs[1]=buildingData[1].prefab;
        buildingPrefabs[2]=buildingData[2].prefab;
        buildingPrefabs[3]=buildingData[3].prefab;
        buildingPrefabs[4]=buildingData[4].prefab;
        buildingPrefabs[5]=buildingData[5].prefab;
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
                                string destroyerPrefab=obj.name;
                                destroyerPrefab=destroyerPrefab.Replace("(Clone)","");
                                for (int i=0;i<buildingPrefabs.Length;i++){
                                    Debug.Log(buildingPrefabs[i].name+"  ---   "+destroyerPrefab);
                                    if (buildingPrefabs[i].name==destroyerPrefab){
                                        Debug.Log("Found");
                                        populationLimit-=buildingData[i].populationCapacity;
                                        populationValue.text=populationLimit.ToString();
                                        break;
                                    }
                                }
                                Debug.Log(destroyerPrefab);
                                // Mark the node as unoccupied
                                gridManager.SetNodeOccupied(node.worldPosition, false);

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
            //Debug.LogError("GridManager reference is null! Trying to find it...");
            gridManager = FindFirstObjectByType<GridManager>();

            // If still null, exit early to prevent further errors
            if (gridManager == null)
            {
                //Debug.LogError("Could not find GridManager in the scene!");
                return;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Debug ray
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        //Debug.Log("Grid Layer: " + LayerMask.LayerToName(gridLayer));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            // Debug hit
            //Debug.Log("Hit at position: " + hit.point);

            // Get the corresponding node from GridManager
            Node node = gridManager.GetNodeFromWorldPosition(hit.point);

            if (node != null)
            {
                // Debug node info
                //Debug.Log("Found node at: " + node.worldPosition + ", Empty: " + node.isEmpty);

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
                    
                    PlaceBuilding(lastValidPosition);
                    Debug.Log("Found node at: " + node.worldPosition + ", Empty: " + node.isEmpty);
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

                // Check if position is valid in grid bounds
                Node node = gridManager.GetNodeFromWorldPosition(snappedPosition);
                canPlace = (node != null && node.isEmpty);

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
        // Create the actual building
        GameObject placedBuilding = Instantiate(currentBuildingPrefab, position, Quaternion.Euler(0f, currentRotation, 0f));

        string prefabName = currentBuildingPrefab.name;
        for (int i=0;i<buildingPrefabs.Length;i++){
            if (buildingPrefabs[i].name==prefabName){
              populationLimit+=buildingData[i].populationCapacity; 
              populationValue.text=populationLimit.ToString();     
                
            }
        }
        // Update UI text if it exists
        

        // Restore original materials for the placed building
        Renderer[] buildingRenderers = placedBuilding.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < buildingRenderers.Length && i < currentBuildingPrefab.GetComponentsInChildren<Renderer>().Length; i++)
        {
            // Try to get the original material from the prefab
            Renderer prefabRenderer = currentBuildingPrefab.GetComponentsInChildren<Renderer>()[i];
            buildingRenderers[i].material = prefabRenderer.sharedMaterial;
        }

        // Mark the node as occupied
        gridManager.SetNodeOccupied(position, true);

        // Debug grid occupancy
        gridManager.DebugPrintGridOccupancy();
    }
}
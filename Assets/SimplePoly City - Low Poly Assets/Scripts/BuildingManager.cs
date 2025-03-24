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
    private GameObject currentBuildingPrefab; // Currently selected building
    private GameObject previewObject; // Preview version of the building before placement
    private Vector3 lastValidPosition; // Stores last valid placement position
    private bool canPlace = false; // Tracks whether placement is allowed
    private int selectedBuildingIndex = -1; // Currently selected building index
    private float currentRotation = 0f; // Current rotation of the building (in y-axis)
    public int taxableBuilding = 0;
    public int maintainanceRequired = 0;
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
        HandleRotation(); // Add rotation handling
        HandleBuildingPlacement();
        //HandleBuildingRemoval(); // Add building removal handling
    }

    void HandleBuildingSelection()
    {
        // Select a building with number keys (1, 2, 3...)
        if (Input.GetKeyDown(KeyCode.Alpha1) && buildingPrefabs.Length > 0) SelectBuilding(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && buildingPrefabs.Length > 1) SelectBuilding(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && buildingPrefabs.Length > 2) SelectBuilding(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && buildingPrefabs.Length > 2) SelectBuilding(3);
        if (Input.GetKeyDown(KeyCode.Alpha5) && buildingPrefabs.Length > 2) SelectBuilding(4);
        if (Input.GetKeyDown(KeyCode.Alpha6) && buildingPrefabs.Length > 2) SelectBuilding(5);
        if (Input.GetKeyDown(KeyCode.Alpha7) && buildingPrefabs.Length > 2) SelectBuilding(6);
        if (Input.GetKeyDown(KeyCode.Alpha8) && buildingPrefabs.Length > 2) SelectBuilding(7);
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

            Debug.Log("Rotated building to: " + currentRotation + " degrees");
        }
    }


    void SelectBuilding(int index)
    {
        // Update selection
        selectedBuildingIndex = index;
        currentBuildingPrefab = buildingPrefabs[index];

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

        // Check if gridManager is null before trying to use it
        if (gridManager == null)
        {
            Debug.LogError("GridManager reference is null! Trying to find it...");
            gridManager = FindFirstObjectByType<GridManager>();

            // If still null, exit early to prevent further errors
            if (gridManager == null)
            {
                Debug.LogError("Could not find GridManager in the scene!");
                return;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Debug ray
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        Debug.Log("Grid Layer: " + LayerMask.LayerToName(gridLayer));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            // Debug hit
            Debug.Log("Hit at position: " + hit.point);

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

        // Update stats based on building type
        if (selectedBuildingIndex == 1)
        {
            populationLimit += 5;
        }
        else if (selectedBuildingIndex == 2)
        {
            populationLimit += 7;
        }

        if (selectedBuildingIndex != 5 && selectedBuildingIndex != 4 && selectedBuildingIndex != 6)
        {
            taxableBuilding += 1;
        }
        else
        {
            Debug.Log("Maintenance Required");
            maintainanceRequired += 1;
        }

        // Update UI text if it exists
        if (populationValue != null)
        {
            populationValue.text = populationLimit.ToString();
        }
        else
        {
            Debug.LogWarning("populationValue Text component is null!");
        }

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
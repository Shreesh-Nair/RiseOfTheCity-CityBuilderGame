using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Material lineMaterial; // Assign this in the inspector

    public Node[,] grid;
    private GameObject gridVisualization;
    private GameObject gridCollider;

    private void Start()
    {
        StartCoroutine(InitializeGrid());
    }

    private IEnumerator InitializeGrid()
    {
        // Wait for BuildingDatabase to be ready
        while (BuildingDatabase.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
        
        // Wait for SaveLoadManager to be ready
        if (saveLoadManager == null)
        {
            Debug.LogWarning("SaveLoadManager not found, creating new grid");
            GenerateGrid();
        }
        else
        {
            // Only generate a new grid if we don't have saved data
            if (!LoadGridState())
            {
                GenerateGrid();
            }
        }
        
        CreateGridVisualization();
        CreateGridCollider();
    }

    bool LoadGridState()
    {
        try
        {
            // Wait for SaveLoadManager
            while (saveLoadManager == null)
            {
                saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
                if (saveLoadManager == null)
                {
                    Debug.LogWarning("SaveLoadManager not available yet");
                    return false;
                }
            }

            // Wait for BuildingDatabase
            while (BuildingDatabase.Instance == null)
            {
                Debug.LogWarning("BuildingDatabase not available yet");
                return false;
            }

            var saveData = saveLoadManager.LoadGridDimensions();
            if (!saveData.HasValue)
            {
                Debug.LogWarning("No save data found during LoadGridState");
                return false;
            }

            width = saveData.Value.width;
            height = saveData.Value.height;
            grid = new Node[width, height];
                    
            // Initialize grid with saved occupancy data
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 worldPos = new Vector3(x * cellSize, 0, y * cellSize);
                    bool isEmpty = saveData.Value.occupancy[x + y * width];
                    grid[x, y] = new Node(worldPos, isEmpty);
                }
            }
            
            if (saveData.Value.buildings != null && saveData.Value.buildings.Count > 0)
            {
                try
                {
                    // Load and place buildings
                    saveLoadManager.LoadAndPlaceBuildings(saveData.Value.buildings);
                    Debug.Log($"Successfully loaded {saveData.Value.buildings.Count} buildings from save file");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error loading buildings: {e.Message}");
                    // Continue even if building loading fails
                }
            }
            
            Debug.Log($"Grid restored from save file: {width}x{height}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading grid state: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    void GenerateGrid()
    {
        grid = new Node[width, height];

        // Create all nodes with proper world positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = new Vector3(x * cellSize, 0, y * cellSize);
                grid[x, y] = new Node(worldPos, true); // Initialize as empty
            }
        }
    }

    void CreateGridVisualization()
    {
        // Clean up any existing visualization
        if (gridVisualization != null)
            Destroy(gridVisualization);

        gridVisualization = new GameObject("Grid Visualization"); // visualize the grid
        gridVisualization.transform.parent = this.transform;

        // Create line renderer to show the grid
        for (int x = 0; x <= width; x++)
        {
            GameObject line = new GameObject($"Vertical Line {x}");
            line.transform.parent = gridVisualization.transform;

            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(x * cellSize, 0, 0));
            lr.SetPosition(1, new Vector3(x * cellSize, 0, height * cellSize));
        }

        for (int y = 0; y <= height; y++)
        {
            GameObject line = new GameObject($"Horizontal Line {y}");
            line.transform.parent = gridVisualization.transform;

            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(0, 0, y * cellSize));
            lr.SetPosition(1, new Vector3(width * cellSize, 0, y * cellSize));
        }
    }

    void CreateGridCollider()
    {
        // Clean up any existing collider
        if (gridCollider != null)
            Destroy(gridCollider);

        gridCollider = new GameObject("Grid Collider");
        gridCollider.transform.parent = this.transform;

        // Position at the center of the grid
        gridCollider.transform.position = new Vector3(width * cellSize / 2, 0, height * cellSize / 2);

        // Add a box collider sized to cover the entire grid
        BoxCollider collider = gridCollider.AddComponent<BoxCollider>();
        collider.size = new Vector3(width * cellSize, 0.1f, height * cellSize); // set size to cover the grid

        // Set layer to Grid (create this layer in Unity)
        gridCollider.layer = LayerMask.NameToLayer("Grid");
    }

    // Update GetNodeFromWorldPosition to use RoundToInt for more consistent results
    public Node GetNodeFromWorldPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize); // rounding the position to snap to grid
        int y = Mathf.RoundToInt(position.z / cellSize);

        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }


    public bool IsPositionValid(Vector3 worldPosition)
    {
        Node node = GetNodeFromWorldPosition(worldPosition);
        return node != null && node.isEmpty;
    }

    public void SetNodeOccupied(Vector3 worldPosition, bool occupied, int tileSize = 1)
    {
        // Convert world position to grid coordinates
        int centerX = Mathf.FloorToInt(worldPosition.x / cellSize);
        int centerY = Mathf.FloorToInt(worldPosition.z / cellSize);

        // Mark all nodes within the building's footprint as occupied/unoccupied
        for (int x = centerX - (tileSize - 1); x <= centerX + (tileSize - 1); x++)
        {
            for (int y = centerY - (tileSize - 1); y <= centerY + (tileSize - 1); y++)
            {
                // Check if coordinates are within grid bounds
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    grid[x, y].isEmpty = !occupied;
                }
            }
        }
        
    }


    // Optional: Method to clear all occupied nodes (reset grid)
    public void ClearGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].isEmpty = true;
            }
        }
    }

    // Optional: Debug method to visualize which cells are occupied
    public void DebugPrintGridOccupancy()
    {
        string gridDebug = "Grid Occupancy:\n";
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                gridDebug += grid[x, y].isEmpty ? "O " : "X ";
            }
            gridDebug += "\n";
        }
        //Debug.Log(gridDebug);
    }
    private SaveLoadManager saveLoadManager;

    private void Update()
    {
        // Check if the 'G' key is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGridVisualization();
        }

        // Save when K is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (saveLoadManager == null)
            {
                saveLoadManager = FindFirstObjectByType<SaveLoadManager>();
            }
            if (saveLoadManager != null)
            {
                saveLoadManager.SaveGridDimensions();
            }
        }
    }

    private void ToggleGridVisualization()
    {
        // If grid visualization exists, toggle its active state
        if (gridVisualization != null)
        {
            gridVisualization.SetActive(!gridVisualization.activeSelf);
        }
        // If it doesn't exist and we're trying to show it, create it first
        else
        {
            CreateGridVisualization();
        }
    }

}

public class Node
{
    public Vector3 worldPosition;
    public bool isEmpty;

    public Node(Vector3 position, bool empty)
    {
        worldPosition = position;
        isEmpty = empty;
    }
}
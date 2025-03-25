using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Material lineMaterial; // Assign this in the inspector
    
    private Node[,] grid;
    private GameObject gridVisualization;
    private GameObject gridCollider;

    private void Start()
    {
        GenerateGrid();
        CreateGridVisualization();
        CreateGridCollider();
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
            
        gridVisualization = new GameObject("Grid Visualization");
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
        collider.size = new Vector3(width * cellSize, 0.1f, height * cellSize);
        
        // Set layer to Grid (create this layer in Unity)
        gridCollider.layer = LayerMask.NameToLayer("Grid");
    }

    public Node GetNodeFromWorldPosition(Vector3 position)
    {
        // Convert world position to grid coordinates
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.z / cellSize);

        // Check if coordinates are within grid bounds
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        
        return null;
    }
    
    public bool IsPositionValid(Vector3 worldPosition)
    {
        Node node = GetNodeFromWorldPosition(worldPosition);
        return node != null && node.isEmpty;
    }
    
    public void SetNodeOccupied(Vector3 worldPosition, bool occupied)
    {
        Node node = GetNodeFromWorldPosition(worldPosition);
        if (node != null)
        {
            node.isEmpty = !occupied;
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
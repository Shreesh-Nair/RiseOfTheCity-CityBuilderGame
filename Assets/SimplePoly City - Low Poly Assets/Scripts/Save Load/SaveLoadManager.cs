using UnityEngine;
using System.IO;
using System;

[Serializable]
public class SaveData
{
    public int gridWidth;
    public int gridHeight;
    public bool[] gridOccupancy; // Flattened 2D array of grid occupancy
}

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    private GridManager gridManager;
    
    void Awake()
    {
        savePath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, "save_data.json"));
    }
    
    void Start()
    {
        try
        {
            gridManager = FindFirstObjectByType<GridManager>();
            
            string saveDirectory = Path.GetDirectoryName(savePath);
            Debug.Log($"Save directory: {saveDirectory}");
            
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
                Debug.Log($"Created save directory at: {saveDirectory}");
            }

            Debug.Log($"Full save file path: {savePath}");
            if (File.Exists(savePath))
            {
                Debug.Log("Save file already exists!");
            }

            // Try to load existing save file
            var loadedData = LoadGridDimensions();
            if (loadedData.HasValue)
            {
                Debug.Log("Successfully found existing save file!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in Start: {e.Message}\nStack trace: {e.StackTrace}");
        }
    }

    public void SaveGridDimensions()
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found!");
            return;
        }

        try
        {
            // Create flattened array of grid occupancy
            bool[] occupancyData = new bool[gridManager.width * gridManager.height];
            for (int x = 0; x < gridManager.width; x++)
            {
                for (int y = 0; y < gridManager.height; y++)
                {
                    occupancyData[x + y * gridManager.width] = gridManager.grid[x, y].isEmpty;
                }
            }

            SaveData data = new SaveData
            {
                gridWidth = gridManager.width,
                gridHeight = gridManager.height,
                gridOccupancy = occupancyData
            };

            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, jsonData);
            Debug.Log($"Successfully saved game to: {savePath}");
            Debug.Log($"Saved data: {jsonData}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}\nStack trace: {e.StackTrace}");
        }
    }

    public (int width, int height, bool[] occupancy)? LoadGridDimensions()
    {
        try
        {
            // Ensure path is properly formatted
            string fullPath = Path.GetFullPath(savePath);
            Debug.Log($"Attempting to load save file from: {fullPath}");
            
            // List all files in directory to help debug
            string directory = Path.GetDirectoryName(fullPath);
            if (Directory.Exists(directory))
            {
                Debug.Log("Files in save directory:");
                foreach (string file in Directory.GetFiles(directory))
                {
                    Debug.Log($"Found file: {file}");
                }
            }

            if (!File.Exists(fullPath))
            {
                Debug.Log($"No save file found at: {fullPath}");
                return null;
            }

            string jsonData = File.ReadAllText(fullPath);
            Debug.Log($"Read save file content: {jsonData}");
            
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            
            if (data == null)
            {
                Debug.LogError("Failed to parse save data from JSON");
                return null;
            }
            
            if (data.gridOccupancy == null || data.gridOccupancy.Length == 0)
            {
                Debug.LogError("Save file contains no occupancy data");
                return null;
            }
            
            Debug.Log($"Successfully loaded grid dimensions: {data.gridWidth}x{data.gridHeight} with {data.gridOccupancy.Length} occupancy values");
            return (data.gridWidth, data.gridHeight, data.gridOccupancy);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading save file: {e.Message}\nStack trace: {e.StackTrace}");
            return null;
        }
    }
}

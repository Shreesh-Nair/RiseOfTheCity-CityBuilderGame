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
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            if (File.Exists(savePath))
            {
                Debug.Log("Save file found, will load grid state from save.");
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
            Debug.Log("Grid state and occupancy saved successfully");
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
            if (!File.Exists(savePath))
            {
                return null;
            }

            string jsonData = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            
            if (data == null || data.gridOccupancy == null || data.gridOccupancy.Length == 0)
            {
                return null;
            }
            
            Debug.Log("Grid occupancy loaded from save file");
            return (data.gridWidth, data.gridHeight, data.gridOccupancy);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading save file: {e.Message}\nStack trace: {e.StackTrace}");
            return null;
        }
    }
}

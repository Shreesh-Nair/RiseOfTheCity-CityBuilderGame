using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

[Serializable]
public class BuildingInfo
{
    public string assetName;
    public float xPos;
    public float yPos;
    public float zPos;
    public float rotation;
}

[Serializable]
public class SaveData
{
    public int gridWidth;
    public int gridHeight;
    public bool[] gridOccupancy; // Flattened 2D array of grid occupancy
    public List<BuildingInfo> placedBuildings = new List<BuildingInfo>();
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

    private BuildingManager buildingManager;

    public void SaveGridDimensions()
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found!");
            return;
        }

        if (buildingManager == null)
        {
            buildingManager = FindFirstObjectByType<BuildingManager>();
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

            // Find all buildings in the scene
            GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
            List<BuildingInfo> buildingInfos = new List<BuildingInfo>();

            foreach (GameObject building in buildings)
            {
                // Get the name of the prefab this building was instantiated from
                string prefabName = "";
                for (int i = 0; i < buildingManager.buildingData.Length; i++)
                {
                    if (buildingManager.buildingData[i].prefab.name == building.name.Replace("(Clone)", "").Trim())
                    {
                        prefabName = buildingManager.buildingData[i].assetName;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(prefabName))
                {
                    BuildingInfo info = new BuildingInfo
                    {
                        assetName = prefabName,
                        xPos = building.transform.position.x,
                        yPos = building.transform.position.y,
                        zPos = building.transform.position.z,
                        rotation = building.transform.eulerAngles.y
                    };
                    buildingInfos.Add(info);
                }
            }

            SaveData data = new SaveData
            {
                gridWidth = gridManager.width,
                gridHeight = gridManager.height,
                gridOccupancy = occupancyData,
                placedBuildings = buildingInfos
            };

            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, jsonData);
            Debug.Log($"Grid state and {buildingInfos.Count} buildings saved successfully");
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

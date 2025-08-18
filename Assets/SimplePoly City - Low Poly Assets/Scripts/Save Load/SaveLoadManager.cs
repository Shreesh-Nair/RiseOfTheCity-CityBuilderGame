using UnityEngine;
using System.IO;
using System;
using System.Collections;
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
    public bool[] gridOccupancy;
    public List<BuildingInfo> placedBuildings = new List<BuildingInfo>();
}

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    private GridManager gridManager;
    private BuildingManager buildingManager;
    private bool isInitialized = false;
    
    void Awake()
    {
        savePath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, "save_data.json"));
    }
    
    void Start()
    {
        StartCoroutine(InitializeAndLoad());
    }

    private IEnumerator InitializeAndLoad()
    {
        // Create save directory if it doesn't exist
        try
        {
            string saveDirectory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error creating save directory: {e.Message}\nStack trace: {e.StackTrace}");
            yield break;
        }

        // Wait for required components
        while (!isInitialized)
        {
            if (gridManager == null)
                gridManager = FindFirstObjectByType<GridManager>();
            
            if (buildingManager == null)
                buildingManager = FindFirstObjectByType<BuildingManager>();

            if (BuildingDatabase.Instance != null && gridManager != null && buildingManager != null)
            {
                isInitialized = true;
                Debug.Log("SaveLoadManager components initialized successfully");

                try
                {
                    // Load saved data if it exists
                    if (File.Exists(savePath))
                    {
                        var saveData = LoadGridDimensions();
                        if (saveData.HasValue)
                        {
                            LoadAndPlaceBuildings(saveData.Value.buildings);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading save data: {e.Message}\nStack trace: {e.StackTrace}");
                }
                yield break;
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }

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
                    // Get snapped coordinates like in BuildingManager
                    Vector3 snappedPos = new Vector3(
                        Mathf.Round(building.transform.position.x / gridManager.cellSize) * gridManager.cellSize,
                        0f,
                        Mathf.Round(building.transform.position.z / gridManager.cellSize) * gridManager.cellSize
                    );

                    BuildingInfo info = new BuildingInfo
                    {
                        assetName = prefabName,
                        xPos = snappedPos.x,
                        yPos = snappedPos.y,
                        zPos = snappedPos.z,
                        rotation = building.transform.eulerAngles.y
                    };
                    buildingInfos.Add(info);
                    
                    Debug.Log($"Saved building {prefabName} at position {snappedPos}");
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

    public (int width, int height, bool[] occupancy, List<BuildingInfo> buildings)? LoadGridDimensions()
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
            
            Debug.Log($"Grid occupancy and {data.placedBuildings?.Count ?? 0} buildings loaded from save file");
            return (data.gridWidth, data.gridHeight, data.gridOccupancy, data.placedBuildings);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading save file: {e.Message}\nStack trace: {e.StackTrace}");
            return null;
        }
    }

    // Helper method to load and place buildings
    public void LoadAndPlaceBuildings(List<BuildingInfo> buildings)
    {
        if (buildings == null || buildings.Count == 0)
        {
            Debug.Log("No buildings to load");
            return;
        }

        // Make sure required components are available
        while (buildingManager == null)
        {
            buildingManager = FindFirstObjectByType<BuildingManager>();
            if (buildingManager == null)
            {
                Debug.LogWarning("Waiting for BuildingManager to be available...");
                return;
            }
        }

        while (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null)
            {
                Debug.LogWarning("Waiting for GridManager to be available...");
                return;
            }
        }

        while (BuildingDatabase.Instance == null)
        {
            Debug.LogWarning("Waiting for BuildingDatabase to be available...");
            return;
        }

        foreach (var buildingInfo in buildings)
        {
            if (buildingInfo == null)
            {
                Debug.LogWarning("Null building info encountered, skipping...");
                continue;
            }

            // First try to get the building data from BuildingDatabase
            BuildingDatabase.BuildingData buildingData = null;
            
            try
            {
                // Try residential/commercial buildings first
                buildingData = BuildingDatabase.Instance.GetResidentialCommercialBuilding(buildingInfo.assetName);
                
                // If not found, try industrial buildings
                if (buildingData == null)
                    buildingData = BuildingDatabase.Instance.GetIndustrialBuilding(buildingInfo.assetName);
                
                // If still not found, try agricultural buildings
                if (buildingData == null)
                    buildingData = BuildingDatabase.Instance.GetAgriculturalBuilding(buildingInfo.assetName);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error getting building data: {e.Message}");
                continue;
            }

            if (buildingData != null)
            {
                // Calculate the snapped position using the same logic as BuildingManager
                float snappedX = Mathf.Round(buildingInfo.xPos / gridManager.cellSize) * gridManager.cellSize;
                float snappedZ = Mathf.Round(buildingInfo.zPos / gridManager.cellSize) * gridManager.cellSize;
                Vector3 position = new Vector3(snappedX, 0f, snappedZ);

                // Verify grid position is valid
                Node node = gridManager.GetNodeFromWorldPosition(position);
                if (node != null)
                {
                    // Create the building
                    GameObject building = Instantiate(buildingData.prefab, position, Quaternion.Euler(0f, buildingInfo.rotation, 0f));
                    if (building != null)
                    {
                        building.tag = "Building";

                        // Mark grid cells as occupied
                        gridManager.SetNodeOccupied(position, true, buildingData.tileSize);

                        // Update the game stats safely
                        if (buildingManager != null)
                        {
                            buildingManager.populationLimit += buildingData.populationCapacity;
                            buildingManager.pollutionFactor += buildingData.pollutionFactor;
                            buildingManager.maintainanceFactor += buildingData.maintenanceCost;
                            buildingManager.totalCommercialProduction += buildingData.commercialProduction;
                            if (buildingManager.populationValue != null)
                                buildingManager.populationValue.text = buildingManager.populationLimit.ToString();
                            buildingManager.totalMorale += buildingData.moraleFactor;
                            buildingManager.totalSafety += buildingData.safety;
                            buildingManager.totalBuildings++;

                            Debug.Log($"Successfully placed building {buildingInfo.assetName} at position {position}");
                        }
                        else
                        {
                            Debug.LogError("BuildingManager is null when trying to update stats");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to instantiate building {buildingInfo.assetName}");
                    }
                }
                else
                {
                    Debug.LogError($"Invalid grid position for building {buildingInfo.assetName} at {position}");
                }

                Debug.Log($"Loaded and placed building {buildingInfo.assetName} at position {position}");
            }
            else
            {
                Debug.LogWarning($"Could not find building data for asset: {buildingInfo.assetName}");
            }
        }
    }
}

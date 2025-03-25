using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour
{
    // Singleton pattern for easy access
    public static BuildingDatabase Instance { get; private set; }

    // Enum for building types
    public enum BuildingType
    {
        Housing,
        Commercial,
        Factory,
        Agricultural,
        Recreation,
        Decoration,
        Government,
        Road
    }

    // Enum for region types
    public enum RegionType
    {
        ResidentialCommercial,
        Industrial,
        Agricultural,
        Road
    }

    // Building data structure
    [System.Serializable]
    public class BuildingData
    {
        public string assetName;
        public int tileSize;
        public BuildingType buildingType;
        public RegionType regionType;
        public GameObject prefab;
        public int constructionCost;
        public int maintenanceCost;
        public int populationCapacity;
        public int moraleFactor;
        public int rawMatrialProduction; 
        public int rawMaterialConsumption;
        public int foodMatrialProduction; 
        public int foodMaterialConsumption;
        public int pollutionFactor;
        public int commercialProduction;      
    }

    // Dictionaries for each region
    private Dictionary<string, BuildingData> residentialCommercialBuildings = new Dictionary<string, BuildingData>();
    private Dictionary<string, BuildingData> industrialBuildings = new Dictionary<string, BuildingData>();
    private Dictionary<string, BuildingData> agriculturalBuildings = new Dictionary<string, BuildingData>();
    private Dictionary<string, BuildingData> roadElements = new Dictionary<string, BuildingData>();

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeBuildingData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeBuildingData()
    {
        // Here you'll manually add all your building data
        PopulateResidentialCommercialBuildings();
        PopulateIndustrialBuildings();
        PopulateAgriculturalBuildings();
        PopulateRoadElements();
    }

    private void PopulateResidentialCommercialBuildings()
    {
        // Example of manually adding residential/commercial buildings
        
        // Example 1: Small House
        BuildingData smallHouse = new BuildingData
        {
            assetName = "Small_House_1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_big_color01_final"),
            constructionCost = 100,
            maintenanceCost = 5,
            populationCapacity = 5,
            moraleFactor = 2,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 1,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 2,
            pollutionFactor = 1,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(smallHouse.assetName, smallHouse);
        
        // Example 2: Bakery
        BuildingData bakery = new BuildingData
        {
            assetName = "Bakery",
            tileSize = 2,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Bakery"),
            constructionCost = 250,
            maintenanceCost = 15,
            populationCapacity = 0,
            moraleFactor = 3,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 2,
            foodMatrialProduction = 5,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(bakery.assetName, bakery);
        
        // Add more residential/commercial buildings here...
    }

    private void PopulateIndustrialBuildings()
    {
        
        BuildingData factory = new BuildingData
        {
            assetName = "Factory_1",
            tileSize = 3,
            buildingType = BuildingType.Factory,
            regionType = RegionType.Industrial,
            prefab = Resources.Load<GameObject>("Buildings/Factory_1"),
            constructionCost = 500,
            maintenanceCost = 30,
            populationCapacity = 0,
            moraleFactor = -5,
            rawMatrialProduction = 15,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 10,
            commercialProduction = 0
        };
        industrialBuildings.Add(factory.assetName, factory);
        
        // Add more industrial buildings here...
    }

    private void PopulateAgriculturalBuildings()
    {
        // Example of manually adding agricultural buildings
        
        // Example: Farm
        BuildingData farm = new BuildingData
        {
            assetName = "Farm_1",
            tileSize = 4,
            buildingType = BuildingType.Agricultural,
            regionType = RegionType.Agricultural,
            prefab = Resources.Load<GameObject>("Buildings/Farm_1"),
            constructionCost = 300,
            maintenanceCost = 20,
            populationCapacity = 0,
            moraleFactor = 1,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 1,
            foodMatrialProduction = 10,
            foodMaterialConsumption = 0,
            pollutionFactor = 1,
            commercialProduction = 0
        };
        agriculturalBuildings.Add(farm.assetName, farm);
        
        // Add more agricultural buildings here...
    }

    private void PopulateRoadElements()
    {
        // Example of manually adding road elements
        
        // Example: Straight Road
        BuildingData straightRoad = new BuildingData
        {
            assetName = "Road_Straight",
            tileSize = 1,
            buildingType = BuildingType.Road,
            regionType = RegionType.Road,
            prefab = Resources.Load<GameObject>("Roads/Road_Straight"),
            constructionCost = 20,
            maintenanceCost = 1,
            populationCapacity = 0,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 1,
            commercialProduction = 0
        };
        roadElements.Add(straightRoad.assetName, straightRoad);
        
        // Example: Intersection
        BuildingData intersection = new BuildingData
        {
            assetName = "Road_Intersection",
            tileSize = 1,
            buildingType = BuildingType.Road,
            regionType = RegionType.Road,
            prefab = Resources.Load<GameObject>("Roads/Road_Intersection"),
            constructionCost = 30,
            maintenanceCost = 2,
            populationCapacity = 0,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0
        };
        roadElements.Add(intersection.assetName, intersection);
        
        // Add more road elements here...
    }

    // Methods to access building data
    public BuildingData GetResidentialCommercialBuilding(string assetName)
    {
        if (residentialCommercialBuildings.TryGetValue(assetName, out BuildingData data))
            return data;
        return null;
    }

    public BuildingData GetIndustrialBuilding(string assetName)
    {
        if (industrialBuildings.TryGetValue(assetName, out BuildingData data))
            return data;
        return null;
    }

    public BuildingData GetAgriculturalBuilding(string assetName)
    {
        if (agriculturalBuildings.TryGetValue(assetName, out BuildingData data))
            return data;
        return null;
    }

    public BuildingData GetRoadElement(string assetName)
    {
        if (roadElements.TryGetValue(assetName, out BuildingData data))
            return data;
        return null;
    }

    // Get all buildings of a specific type across regions
    public List<BuildingData> GetBuildingsByType(BuildingType type)
    {
        List<BuildingData> result = new List<BuildingData>();
        
        foreach (var building in residentialCommercialBuildings.Values)
        {
            if (building.buildingType == type)
                result.Add(building);
        }
        
        foreach (var building in industrialBuildings.Values)
        {
            if (building.buildingType == type)
                result.Add(building);
        }
        
        foreach (var building in agriculturalBuildings.Values)
        {
            if (building.buildingType == type)
                result.Add(building);
        }
        
        return result;
    }

    // Get all buildings in a specific region
    public Dictionary<string, BuildingData> GetBuildingsByRegion(RegionType region)
    {
        switch (region)
        {
            case RegionType.ResidentialCommercial:
                return residentialCommercialBuildings;
            case RegionType.Industrial:
                return industrialBuildings;
            case RegionType.Agricultural:
                return agriculturalBuildings;
            case RegionType.Road:
                return roadElements;
            default:
                return null;
        }
    }
}

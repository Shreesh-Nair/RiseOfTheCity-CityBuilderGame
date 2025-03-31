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
        public int safety=0;      
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
        
        // Example 1: skyscraper
        BuildingData skyscraper1 = new BuildingData
        {
            assetName = "Skyscraper1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_big_color01_final"),
            constructionCost = 250,
            maintenanceCost = 2,
            populationCapacity = 25,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 50,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 3,
            commercialProduction = 0,
            safety = 0
        };
        residentialCommercialBuildings.Add(skyscraper1.assetName, skyscraper1);

        //skyscraper 2
        BuildingData skyscraper2 = new BuildingData
        {
            assetName = "Skyscraper2",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_big_color02_final"),
            constructionCost = 250,
            maintenanceCost = 2,
            populationCapacity = 25,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 50,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 3,
            commercialProduction = 0,
            safety = 0
        };
        residentialCommercialBuildings.Add(skyscraper2.assetName, skyscraper2);

        //skyscraper 3
        BuildingData skyscraper3 = new BuildingData
        {
            assetName = "Skyscraper3",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_big_color03_final"),
            constructionCost = 250,
            maintenanceCost = 2,
            populationCapacity = 25,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 50,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 3,
            commercialProduction = 0,
            safety = 0
        };
        residentialCommercialBuildings.Add(skyscraper3.assetName, skyscraper3);

        //skyscraper small 1
        BuildingData skyscraperSmall1 = new BuildingData
        {
            assetName = "SkyscraperSmall1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_small_color01_final"),
            constructionCost = 150,
            maintenanceCost = 1,
            populationCapacity = 15,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 27,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0,
            safety = 0
        };
        residentialCommercialBuildings.Add(skyscraperSmall1.assetName, skyscraperSmall1);

        //skyscraper small 2
        BuildingData skyscraperSmall2 = new BuildingData
        {
            assetName = "SkyscraperSmall2",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_small_color02_final"),
            constructionCost = 150,
            maintenanceCost = 1,
            populationCapacity = 15,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 27,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0,
            safety = 0
        };
        residentialCommercialBuildings.Add(skyscraperSmall2.assetName, skyscraperSmall2);

        //skyscraper small 3
        BuildingData skyscraperSmall3 = new BuildingData
        {
            assetName = "SkyscraperSmall3",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building Sky_small_color03_final"),
            constructionCost = 150,
            maintenanceCost = 1,
            populationCapacity = 15,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 27,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0,
            safety = 0
        };
        residentialCommercialBuildings.Add(skyscraperSmall3.assetName, skyscraperSmall3);


        //bakery
        BuildingData bakery = new BuildingData
        {
            assetName = "bakery",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Bakery_final"),
            constructionCost = 70,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 10,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 40,
            pollutionFactor = 2,
            commercialProduction = 20,
            safety = 0
        };
        residentialCommercialBuildings.Add(bakery.assetName, bakery);

        //bar
        BuildingData bar = new BuildingData
        {
            assetName = "bar",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Bar_final"),
            constructionCost = 100,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 30,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 30,
            pollutionFactor = 2,
            commercialProduction = 50,
            safety = 0
        };
        residentialCommercialBuildings.Add(bar.assetName, bar);

        //bookshop
        BuildingData bookshop = new BuildingData
        {
            assetName = "bookshop",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Books Shop_final"),
            constructionCost = 60,
            maintenanceCost = 5,
            populationCapacity = 0,
            moraleFactor = 20,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 10
        };
        residentialCommercialBuildings.Add(bookshop.assetName, bookshop);

        //chickenshop
        BuildingData chickenshop = new BuildingData
        {
            assetName = "chickenshop",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Chicken Shop_final"),
            constructionCost = 70,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 25,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 30,
            pollutionFactor = 0,
            commercialProduction = 25
        };
        residentialCommercialBuildings.Add(chickenshop.assetName, chickenshop);

        //clothshop
        BuildingData clothshop = new BuildingData
        {
            assetName = "clothshop",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Books Shop_final"),
            constructionCost = 65,
            maintenanceCost = 5,
            populationCapacity = 0,
            moraleFactor = 23,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(clothshop.assetName, clothshop);

        //coffeeshop
        BuildingData coffeeshop = new BuildingData
        {
            assetName = "coffeeshop",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Coffee Shop_with_chairs_final"),
            constructionCost = 65,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(coffeeshop.assetName, coffeeshop);
    
        //drugstore
        BuildingData drugstore = new BuildingData
        {
            assetName = "drugstore",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Drug Store_final"),
            constructionCost = 55,
            maintenanceCost = 10,
            populationCapacity = 0,
            moraleFactor = 20,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(drugstore.assetName, drugstore);

        //fastfood
        BuildingData fastfood = new BuildingData
        {
            assetName = "fastfood",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Fast_Food_final"),
            constructionCost = 70,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 25,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 30,
            pollutionFactor = 0,
            commercialProduction = 25
        };
        residentialCommercialBuildings.Add(fastfood.assetName, fastfood);

        //fruitshop
        BuildingData fruitshop = new BuildingData
        {
            assetName = "fruitshop",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Fruits _Shop_final"),
            constructionCost = 75,
            maintenanceCost = 7,
            populationCapacity = 0,
            moraleFactor = 23,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 25
        };
        residentialCommercialBuildings.Add(fruitshop.assetName, fruitshop);

        //gasstation
        BuildingData gasstation = new BuildingData
        {
            assetName = "gasstation",
            tileSize = 2,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Gas_Station_final"),
            constructionCost = 100,
            maintenanceCost = 15,
            populationCapacity = 0,
            moraleFactor = 25,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 40
        };
        residentialCommercialBuildings.Add(gasstation.assetName, gasstation);

        //smallhouse1
        BuildingData smallhouse1 = new BuildingData
        {
            assetName = "smallhouse1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_01_color01_final"),
            constructionCost = 50,
            maintenanceCost = 0,
            populationCapacity = 4,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(smallhouse1.assetName, smallhouse1);

        //smallhouse2
        BuildingData smallhouse2 = new BuildingData
        {
            assetName = "smallhouse2",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_01_color02_final"),
            constructionCost = 50,
            maintenanceCost = 0,
            populationCapacity = 4,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(smallhouse2.assetName, smallhouse2);

        //smallhouse3
        BuildingData smallhouse3 = new BuildingData
        {
            assetName = "smallhouse3",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_01_color03_final"),
            constructionCost = 50,
            maintenanceCost = 0,
            populationCapacity = 4,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(smallhouse3.assetName, smallhouse3);

        //midhouse1
        BuildingData midhouse1 = new BuildingData
        {
            assetName = "midhouse1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_02_color01_final"),
            constructionCost = 60,
            maintenanceCost = 0,
            populationCapacity = 6,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(midhouse1.assetName, midhouse1);

        //midhouse2
        BuildingData midhouse2 = new BuildingData
        {
            assetName = "midhouse2",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_02_color02_final"),
            constructionCost = 60,
            maintenanceCost = 0,
            populationCapacity = 6,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(midhouse2.assetName, midhouse2);

        //midhouse3
        BuildingData midhouse3 = new BuildingData
        {
            assetName = "midhouse3",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_02_color03_final"),
            constructionCost = 60,
            maintenanceCost = 0,
            populationCapacity = 6,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(midhouse3.assetName, midhouse3);

        //premhouse1
        BuildingData premhouse1 = new BuildingData
        {
            assetName = "premhouse1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_03_color01_final"),
            constructionCost = 80,
            maintenanceCost = 0,
            populationCapacity = 10,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(premhouse1.assetName, premhouse1);

        //premhouse2
        BuildingData premhouse2 = new BuildingData
        {
            assetName = "premhouse2",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_03_color02_final"),
            constructionCost = 80,
            maintenanceCost = 0,
            populationCapacity = 10,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(premhouse2.assetName, premhouse2);

        //premhouse3
        BuildingData premhouse3 = new BuildingData
        {
            assetName = "premhouse3",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_03_color03_final"),
            constructionCost = 80,
            maintenanceCost = 0,
            populationCapacity = 10,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(premhouse3.assetName, premhouse3);

        //bungalow1
        BuildingData bungalow1 = new BuildingData
        {
            assetName = "bungalow1",
            tileSize = 2,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_04_color01_final"),
            constructionCost = 100,
            maintenanceCost = 0,
            populationCapacity = 12,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(bungalow1.assetName, bungalow1);

        //bungalow2
        BuildingData bungalow2 = new BuildingData
        {
            assetName = "bungalow2",
            tileSize = 2,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_04_color02_final"),
            constructionCost = 100,
            maintenanceCost = 0,
            populationCapacity = 12,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(bungalow2.assetName, bungalow2);

        //bungalow3
        BuildingData bungalow3 = new BuildingData
        {
            assetName = "bungalow3",
            tileSize = 2,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_House_04_color03_final"),
            constructionCost = 100,
            maintenanceCost = 0,
            populationCapacity = 12,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(bungalow3.assetName, bungalow3);

        //musicshop
        BuildingData musicshop = new BuildingData
        {
            assetName = "musicshop",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Music_Store_final"),
            constructionCost = 60,
            maintenanceCost = 5,
            populationCapacity = 0,
            moraleFactor = 20,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 10
        };
        residentialCommercialBuildings.Add(musicshop.assetName, musicshop);

        //pizza
        BuildingData pizza = new BuildingData
        {
            assetName = "pizza",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Pizza_final"),
            constructionCost = 80,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 25,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 30,
            pollutionFactor = 0,
            commercialProduction = 30
        };
        residentialCommercialBuildings.Add(pizza.assetName, pizza);

        //apartment1
        BuildingData apartment1 = new BuildingData
        {
            assetName = "apartment1",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Residential_color01_final"),
            constructionCost = 200,
            maintenanceCost = 1,
            populationCapacity = 20,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 35,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(apartment1.assetName, apartment1);

        //apartment2
        BuildingData apartment2 = new BuildingData
        {
            assetName = "apartment2",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Residential_color02_final"),
            constructionCost = 200,
            maintenanceCost = 1,
            populationCapacity = 20,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 35,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(apartment2.assetName, apartment2);

        //apartment3
        BuildingData apartment3 = new BuildingData
        {
            assetName = "apartment3",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Residential_color03_final"),
            constructionCost = 200,
            maintenanceCost = 1,
            populationCapacity = 20,
            moraleFactor = 0,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 35,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 2,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(apartment3.assetName, apartment3);

        //restraunt
        BuildingData restraunt = new BuildingData
        {
            assetName = "restraunt",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Restaurant_final"),
            constructionCost = 70,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 25,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 30,
            pollutionFactor = 0,
            commercialProduction = 25
        };
        residentialCommercialBuildings.Add(restraunt.assetName, restraunt);

        //shoeshop
        BuildingData shoeshop = new BuildingData
        {
            assetName = "shoeshop",
            tileSize = 1,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Shoes_Shop_final"),
            constructionCost = 65,
            maintenanceCost = 5,
            populationCapacity = 0,
            moraleFactor = 23,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(shoeshop.assetName, shoeshop);

        //stadium
        BuildingData stadium = new BuildingData
        {
            assetName = "stadium",
            tileSize = 5,
            buildingType = BuildingType.Recreation,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Stadium_final"),
            constructionCost = 200,
            maintenanceCost = 15,
            populationCapacity = 0,
            moraleFactor = 50,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 20,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 30,
            pollutionFactor = 0,
            commercialProduction = 100
        };
        residentialCommercialBuildings.Add(stadium.assetName, stadium);

        //cinema
        BuildingData cinema = new BuildingData
        {
            assetName = "cinema",
            tileSize = 2,
            buildingType = BuildingType.Recreation,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/commercial_building_cinema_final"),
            constructionCost = 130,
            maintenanceCost = 10,
            populationCapacity = 0,
            moraleFactor = 40,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 15,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 20,
            pollutionFactor = 0,
            commercialProduction = 70
        };
        residentialCommercialBuildings.Add(cinema.assetName, cinema);

        //firestation
        BuildingData firestation = new BuildingData
        {
            assetName = "firestation",
            tileSize = 2,
            buildingType = BuildingType.Government,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Fire_Station_final"),
            constructionCost = 100,
            maintenanceCost = 12,
            populationCapacity = 0,
            moraleFactor = 35,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 10,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0,
            safety = 10
        };
        residentialCommercialBuildings.Add(firestation.assetName, firestation);

        //bank
        BuildingData bank = new BuildingData
        {
            assetName = "bank",
            tileSize = 2,
            buildingType = BuildingType.Government,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/government_building_bank_final"),
            constructionCost = 100,
            maintenanceCost = 12,
            populationCapacity = 0,
            moraleFactor = 35,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 10,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0,
            safety = 10
        };
        residentialCommercialBuildings.Add(bank.assetName, bank);

        //cityhall
        BuildingData cityhall = new BuildingData
        {
            assetName = "cityhall",
            tileSize = 2,
            buildingType = BuildingType.Government,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/government-building-city-hall_final"),
            constructionCost = 100,
            maintenanceCost = 12,
            populationCapacity = 0,
            moraleFactor = 35,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 10,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0,
            safety = 10
        };
        residentialCommercialBuildings.Add(cityhall.assetName, cityhall);

        //hospital
        BuildingData hospital = new BuildingData
        {
            assetName = "hospital",
            tileSize = 2,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/hospitalFinal"),
            constructionCost = 100,
            maintenanceCost = 12,
            populationCapacity = 0,
            moraleFactor = 35,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 10,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0,
            safety = 10
        };
        residentialCommercialBuildings.Add(hospital.assetName, hospital);

        //School
        BuildingData school = new BuildingData
        {
            assetName = "School",
            tileSize = 4,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/SchoolCenter_final"),
            constructionCost = 100,
            maintenanceCost = 5,
            populationCapacity = 20,
            moraleFactor = 2,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 1,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 2,
            pollutionFactor = 1,
            commercialProduction = 0
        };
        residentialCommercialBuildings.Add(school.assetName, school);
        
        //giftshop
        BuildingData giftshop = new BuildingData
        {
            assetName = "giftshop",
            tileSize = 1,
            buildingType = BuildingType.Housing,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Building_Gift_Shop_final"),
            constructionCost = 65,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(giftshop.assetName, giftshop);

        //basketball
        BuildingData basketball = new BuildingData
        {
            assetName = "basketball",
            tileSize = 2,
            buildingType = BuildingType.Recreation,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/basketballCourt_final"),
            constructionCost = 65,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(basketball.assetName, basketball);

        //park
        BuildingData park = new BuildingData
        {
            assetName = "park",
            tileSize = 2,
            buildingType = BuildingType.Decoration,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/infrastructure_building_park_final"),
            constructionCost = 65,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(park.assetName, park);

        //plaza
        BuildingData plaza = new BuildingData
        {
            assetName = "plaza",
            tileSize = 2,
            buildingType = BuildingType.Decoration,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/infrastructure_building_plaza_final"),
            constructionCost = 65,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(plaza.assetName, plaza);

        //market
        BuildingData market = new BuildingData
        {
            assetName = "market",
            tileSize = 2,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Market_center_final"),
            constructionCost = 65,
            maintenanceCost = 0,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(market.assetName, market);

        //policestation
        BuildingData policestation = new BuildingData
        {
            assetName = "policestation",
            tileSize = 2,
            buildingType = BuildingType.Government,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/Police_Station_final"),
            constructionCost = 100,
            maintenanceCost = 12,
            populationCapacity = 0,
            moraleFactor = 35,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 10,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 0,
            pollutionFactor = 0,
            commercialProduction = 0,
            safety = 10
        };
        residentialCommercialBuildings.Add(policestation.assetName, policestation);

        //supermarket
        BuildingData supermarket = new BuildingData
        {
            assetName = "supermarket",
            tileSize = 2,
            buildingType = BuildingType.Commercial,
            regionType = RegionType.ResidentialCommercial,
            prefab = Resources.Load<GameObject>("Buildings/SuperMarket_final"),
            constructionCost = 65,
            maintenanceCost = 1,
            populationCapacity = 0,
            moraleFactor = 27,
            rawMatrialProduction = 0,
            rawMaterialConsumption = 0,
            foodMatrialProduction = 0,
            foodMaterialConsumption = 35,
            pollutionFactor = 0,
            commercialProduction = 20
        };
        residentialCommercialBuildings.Add(supermarket.assetName, supermarket);

        
        
        
        // Add more residential/commercial buildings here...
    }

    private void PopulateIndustrialBuildings()
    {
        
        BuildingData factory = new BuildingData
        {
            assetName = "Factory1",
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
        BuildingData farm1 = new BuildingData
        {
            assetName = "Farm_1",
            tileSize = 4,
            buildingType = BuildingType.Agricultural,
            regionType = RegionType.Agricultural,
            prefab = Resources.Load<GameObject>("Farm/Natures_Grass Tile Small 1"),
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
        agriculturalBuildings.Add(farm1.assetName, farm1);
        BuildingData farm2 = new BuildingData
        {
            assetName = "Farm_2",
            tileSize = 4,
            buildingType = BuildingType.Agricultural,
            regionType = RegionType.Agricultural,
            prefab = Resources.Load<GameObject>("Farm/Farm_Crop_Sunflower_Step_03 1"),
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
        agriculturalBuildings.Add(farm2.assetName, farm2);
        BuildingData farm3 = new BuildingData
        {
            assetName = "Farm_3",
            tileSize = 4,
            buildingType = BuildingType.Agricultural,
            regionType = RegionType.Agricultural,
            prefab = Resources.Load<GameObject>("Farm/Farm_Crop_Corn_Step_01 1"),
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
        agriculturalBuildings.Add(farm3.assetName, farm3);
        BuildingData farmHouse = new BuildingData
        {
            assetName = "Farm_House",
            tileSize = 4,
            buildingType = BuildingType.Agricultural,
            regionType = RegionType.Agricultural,
            prefab = Resources.Load<GameObject>("Farm/Farm_House_Barn_01"),
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
        agriculturalBuildings.Add(farmHouse.assetName, farmHouse);
        
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

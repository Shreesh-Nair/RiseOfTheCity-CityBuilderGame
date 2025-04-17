using UnityEngine;
using UnityEngine.UI;

public class HandleBuildingSelection : MonoBehaviour
{
    // Building data array to store all available buildings
    public BuildingDatabase.BuildingData[] buildingData;

    // Reference to the BuildingGridContent where cells will be created

    public int currentKeyboardKey = -1;
    public Button[] buildingButtons;
    public Text[] currentNames;
    public string[] assetNames;
    public Button[] allAssets;
    // Current keyboard key selected
    void Start()
    {
        // Initialize building data array with buildings from database
        buildingData = new BuildingDatabase.BuildingData[]{
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("Skyscraper1"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("firestation"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("supermarket"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bank"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("cityhall"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("supermarket"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("market"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("market"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("market"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("market")
        };
        currentNames[0].text = buildingData[0].buildingName;
        currentNames[1].text = buildingData[1].buildingName;
        currentNames[2].text = buildingData[2].buildingName;
        currentNames[3].text = buildingData[3].buildingName;
        currentNames[4].text = buildingData[4].buildingName;
        currentNames[5].text = buildingData[5].buildingName;
        currentNames[6].text = buildingData[6].buildingName;
        currentNames[7].text = buildingData[7].buildingName;
        currentNames[8].text = buildingData[8].buildingName;
        currentNames[9].text = buildingData[9].buildingName;
        assetNames = new string[]{"Skyscraper1","Skyscraper2","Skyscraper3","SkyscraperSmall1","SkyscraperSmall2",
            "SkyscraperSmall3","bakery","bar","bookshop","chickenshop","clothshop","coffeeshop","drugstore","fastfood","fruitshop","gasstation","smallhouse1","smallhouse2","smallhouse3",
            "midhouse1","midhouse2","midhouse3","premhouse1","premhouse2","premhouse3","bungalow1","bungalow2","bungalow3","musicshop","pizza","apartment1","apartment2","apartment3",
            "restraunt","shoeshop","stadium","cinema","firestation","bank","cityhall","hospital","School","giftshop","basketball","park","plaza","market","policestation","supermarket"};
        if (buildingButtons != null)
        {
            for (int i = 0; i < buildingButtons.Length && i < buildingData.Length; i++)
            {
                int index = i; // Capture index for the closure
                buildingButtons[i].onClick.AddListener(() => OnBuildingButtonClicked(index));
            }
        }

        if (allAssets != null)
        {
            for (int i = 0; i < allAssets.Length && i < 50; i++)
            {
                int index = i; // Capture index for the closure
                allAssets[i].onClick.AddListener(() => setBuilding(index));
            }
        }

    }

    void OnBuildingButtonClicked(int index)
    {
        currentKeyboardKey = (index + 1) % 10;
    }

    void setBuilding(int index)
    {
        if (currentKeyboardKey != -1 && currentKeyboardKey != 0)
        {
            Debug.Log("Setting building for key: " + currentKeyboardKey + " to " + assetNames[index]);
            buildingData[currentKeyboardKey-1] = BuildingDatabase.Instance.GetResidentialCommercialBuilding(assetNames[index]);
            currentNames[currentKeyboardKey-1].text = assetNames[index];
        }
        else if (currentKeyboardKey != -1 && currentKeyboardKey == 0){
            Debug.Log("Setting building for key: " + currentKeyboardKey + " to " + assetNames[index]);
            buildingData[9] = BuildingDatabase.Instance.GetResidentialCommercialBuilding(assetNames[index]);
            currentNames[9].text = assetNames[index];
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Add keyboard support for selecting keys 1-0

    }
}

using UnityEngine;
using UnityEngine.UI;

public class HandleBuildingSelection : MonoBehaviour
{
    // Building data array to store all available buildings
    public BuildingDatabase.BuildingData[] buildingData;

    // Reference to the BuildingGridContent where cells will be created
    

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
        };

        
    }

    // Create grid cells for each building
    

    // Update is called once per frame
    void Update()
    {
        // Add keyboard support for selecting keys 1-0
        
    }
}

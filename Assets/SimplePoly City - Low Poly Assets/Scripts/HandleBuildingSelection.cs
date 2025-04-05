using UnityEngine;

public class HandleBuildingSelection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BuildingDatabase.BuildingData[] buildingData;
    void Start()
    {
        buildingData=new BuildingDatabase.BuildingData[]{
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("Skyscraper1"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("supermarket"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("SkyscraperSmall3"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("park"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bakery"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("fruitshop"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("premhouse3"),
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

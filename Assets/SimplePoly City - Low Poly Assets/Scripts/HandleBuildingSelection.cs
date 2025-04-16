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
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("gasstation"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bungalow3"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bungalow2"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bungalow1"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("cinema"),
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

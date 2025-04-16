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
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("firestation"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bank"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("cityhall"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("supermarket"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("market"),
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class HandleBuildingSelection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BuildingDatabase.BuildingData[] buildingData;
    void Start()
    {
        buildingData=new BuildingDatabase.BuildingData[]{
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("Skyscraper1"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("firestation"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bank"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("apartment2"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("cinema"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("stadium"),
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

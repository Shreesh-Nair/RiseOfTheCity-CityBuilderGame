using UnityEngine;

public class HandleBuildingSelection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BuildingDatabase.BuildingData[] buildingData;
    void Start()
    {
        buildingData=new BuildingDatabase.BuildingData[]{
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("Small_House_1"),
            BuildingDatabase.Instance.GetAgriculturalBuilding("Farm_1"),
            BuildingDatabase.Instance.GetAgriculturalBuilding("Farm_2"),
            BuildingDatabase.Instance.GetAgriculturalBuilding("Farm_3"),
            BuildingDatabase.Instance.GetAgriculturalBuilding("Farm_House"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("School"),
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

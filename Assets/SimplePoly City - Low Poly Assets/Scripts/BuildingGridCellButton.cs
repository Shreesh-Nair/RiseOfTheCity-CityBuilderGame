using UnityEngine;
using UnityEngine.UI;

public class BuildingGridCellButton : MonoBehaviour
{
    public BuildingDatabase.BuildingData buildingData; // Info about this building
    public Image buildingImage; // Reference to the Image component that will display the building icon
    
    public void SetBuildingData(BuildingDatabase.BuildingData data)
    {
        buildingData = data;
        
        // Update the image if available
        if (buildingImage != null && buildingData != null && buildingData.icon != null)
        {
            buildingImage.sprite = buildingData.icon;
            buildingImage.enabled = true;
        }
    }
    
    public void OnClickCell()
    {
        // Tell the manager that THIS building was clicked
        BuildingSelectionUI.Instance.OnBuildingCellClicked(this);
    }
}

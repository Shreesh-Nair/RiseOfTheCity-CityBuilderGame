using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    public int keyIndex; // 0 for key "1", 1 for key "2", etc.
    public Text buildingNameText; // Reference to the text showing building name
    public Image buildingIcon; // Reference to the image showing building icon

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnKeyClicked);
    }

    public void OnKeyClicked()
    {
        // Tell the manager this key was selected
        BuildingSelectionUI.Instance.OnKeyButtonClicked(keyIndex);
    }

    // Method to update the key's display when a building is assigned
    public void UpdateKeyDisplay(BuildingDatabase.BuildingData buildingData)
{
    if (buildingData != null)
    {
        buildingNameText.text = buildingData.assetName;
        // Remove or comment out icon-related code
        // buildingIcon.sprite = buildingData.icon;
        // buildingIcon.enabled = true;
    }
    else
    {
        buildingNameText.text = "Empty";
        // Remove or comment out icon-related code
        // buildingIcon.enabled = false;
    }
}


}

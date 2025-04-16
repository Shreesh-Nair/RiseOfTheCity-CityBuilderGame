using UnityEngine;

public class BuildingSelectionUI : MonoBehaviour
{
    public static BuildingSelectionUI Instance;
    public KeyButton[] keyButtons; // Add this array to store references to all key buttons

    private int selectedKeyIndex = -1; // 0 for '1', 1 for '2', ..., 9 for '0'
    private BuildingDatabase.BuildingData[] assignedBuildings = new BuildingDatabase.BuildingData[10];

    void Awake() { Instance = this; }

    public void OnKeyButtonClicked(int keyIndex)
    {
        selectedKeyIndex = keyIndex;
        // Highlight the selected key
        HighlightSelectedKey(keyIndex);
    }

    private void HighlightSelectedKey(int keyIndex)
    {
        // Reset all keys
        for (int i = 0; i < keyButtons.Length; i++)
        {
            if (keyButtons[i] != null)
            {
                keyButtons[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
        }
        
        // Highlight the selected key
        if (keyIndex >= 0 && keyIndex < keyButtons.Length && keyButtons[keyIndex] != null)
        {
            keyButtons[keyIndex].GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    public void OnBuildingCellClicked(BuildingGridCellButton cell)
    {
        if (selectedKeyIndex >= 0)
        {
            assignedBuildings[selectedKeyIndex] = cell.buildingData;
            // Update the key's display
            keyButtons[selectedKeyIndex].UpdateKeyDisplay(cell.buildingData);
            // Reset selection
            selectedKeyIndex = -1;
            HighlightSelectedKey(-1);
        }
    }
}

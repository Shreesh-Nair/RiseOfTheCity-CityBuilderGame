using UnityEngine;
using UnityEngine.UI;

public class UIToggleManager : MonoBehaviour
{
    // References to UI elements
    public GameObject buildingSelectionUI; // Your existing building selection UI (ManagePanel)
    public GameObject textboxCanvas;       // Canvas containing your 4 textboxes
    public Button showBuildingUIButton;    // The "+" button
    public Button doneButton;         
    
    void Start()
    {
        // Set up button click events
        showBuildingUIButton.onClick.AddListener(ShowBuildingUI);
        doneButton.onClick.AddListener(HideBuildingUI);
        
        
        // Initially hide the building selection UI
        buildingSelectionUI.SetActive(false);
        textboxCanvas.SetActive(true);
    }
    
    public void ShowBuildingUI()
    {
        // Show building selection UI and hide textboxes
        buildingSelectionUI.SetActive(true);
        textboxCanvas.SetActive(false);
        
        // Optionally hide the "+" button while building UI is visible
        showBuildingUIButton.gameObject.SetActive(false);
    }
    
    public void HideBuildingUI()
    {
        // Hide building selection UI and show textboxes
        buildingSelectionUI.SetActive(false);
        textboxCanvas.SetActive(true);
        
        // Show the "+" button again
        showBuildingUIButton.gameObject.SetActive(true);
    }
    void Update()
{
    // Toggle UI with Tab key or another key of your choice
    if (Input.GetKeyDown(KeyCode.Tab))
    {
        if (buildingSelectionUI.activeSelf)
        {
            HideBuildingUI();
        }
        else
        {
            ShowBuildingUI();
        }
    }
}
}

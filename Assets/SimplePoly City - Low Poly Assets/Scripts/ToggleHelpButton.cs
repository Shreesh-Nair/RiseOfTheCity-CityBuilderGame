using UnityEngine;
using UnityEngine.UI;

public class ToggleHelpButton : MonoBehaviour
{
    public GameObject helpPanel;
    public Text buttonText;
    
    private bool isHelpVisible = false;
    
    void Start()
    {
        // Set initial button text
        buttonText.text = "Help";
        
        // Initially hide the help panel
        helpPanel.SetActive(false);
        
        // Add click listener
        GetComponent<Button>().onClick.AddListener(ToggleHelp);
    }
    
    public void ToggleHelp()
    {
        isHelpVisible = !isHelpVisible;
        
        // Update button text
        buttonText.text = isHelpVisible ? "OK" : "Help";
        
        // Show/hide help panel
        helpPanel.SetActive(isHelpVisible);
    }
    
    // Optional: Add keyboard shortcut for help
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.H))
        {
            ToggleHelp();
        }
    }
}

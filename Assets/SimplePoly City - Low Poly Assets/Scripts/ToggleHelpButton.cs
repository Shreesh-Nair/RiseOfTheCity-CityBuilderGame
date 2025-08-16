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
        GetComponent<Button>().onClick.AddListener(ToggleHelp); // add a button listener
        
    }
    
    public void ToggleHelp()
    {
        isHelpVisible = !isHelpVisible;
        
        // Update button text
        buttonText.text = isHelpVisible ? "OK" : "Help"; // toggle between "Help" and "OK" in the button text
        // Optionally, you can also change the button color or style here
        
        // Show/hide help panel
        helpPanel.SetActive(isHelpVisible); // visible if true, hidden if false
    }
    
    // Optional: Add keyboard shortcut for help
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.H))
        {
            ToggleHelp(); // gets input from the keyboard and calls the ToggleHelp method
        }
    }
}

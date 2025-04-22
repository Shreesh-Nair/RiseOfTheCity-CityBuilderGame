using UnityEngine;
using UnityEngine.UI;

public class ToggleDetails : MonoBehaviour
{
    public GameObject detailsPanel;
    public Text buttonText;
    
    private bool isDetailsVisible = false;
    
    void Start()
    {
        // Set initial button text
        buttonText.text = "Details";
        
        // Initially hide the help panel
        detailsPanel.SetActive(false);
        
        // Add click listener
        GetComponent<Button>().onClick.AddListener(ToggleHelp);
    }
    
    public void ToggleHelp()
    {
        isDetailsVisible = !isDetailsVisible;
        
        // Update button text
        buttonText.text = isDetailsVisible ? "OK" : "Details";
        
        // Show/hide help panel
        detailsPanel.SetActive(isDetailsVisible);
    }
    
    // Optional: Add keyboard shortcut for help
    void Update()
    {
        
    }
}

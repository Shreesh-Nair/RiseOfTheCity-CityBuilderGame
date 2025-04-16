using UnityEngine;
using UnityEngine.UI;

public class HandleBuildingSelection : MonoBehaviour
{
    // Building data array to store all available buildings
    public BuildingDatabase.BuildingData[] buildingData;

    // Reference to the BuildingGridContent where cells will be created
    public Transform buildingGridContent;

    // Prefab for the building grid cell
    public GameObject buildingCellPrefab;

    // Reference to the BuildingSelectionUI
    private BuildingSelectionUI selectionUI;

    void Start()
    {
        // Initialize building data array with buildings from database
        buildingData = new BuildingDatabase.BuildingData[]{
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("Skyscraper1"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("firestation"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("supermarket"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("bank"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("cityhall"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("supermarket"),
            BuildingDatabase.Instance.GetResidentialCommercialBuilding("market"),
        };

        // Get reference to BuildingSelectionUI
        selectionUI = FindFirstObjectByType<BuildingSelectionUI>();
        if (selectionUI == null)
        {
            Debug.LogError("BuildingSelectionUI not found in scene!");
        }

        // Find BuildingGridContent if not assigned
        if (buildingGridContent == null)
        {
            buildingGridContent = GameObject.Find("BuildingGridContent").transform;
            if (buildingGridContent == null)
            {
                Debug.LogError("BuildingGridContent not found in scene!");
                return;
            }
        }

        // Create the building grid cells
        CreateBuildingGridCells();
    }

    // Create grid cells for each building
    void CreateBuildingGridCells()
    {
        // Clear existing cells if any
        foreach (Transform child in buildingGridContent)
        {
            Destroy(child.gameObject);
        }

        // Create a cell for each building in the data array
        for (int i = 0; i < buildingData.Length; i++)
        {
            // Skip if building data is null
            if (buildingData[i] == null) continue;

            // Instantiate the cell prefab
            GameObject cellGameObject = Instantiate(buildingCellPrefab, buildingGridContent);

            // Set the building name text
            Transform nameTextTransform = cellGameObject.transform.Find("BuildingName");
            if (nameTextTransform != null && nameTextTransform.GetComponent<Text>() != null)
            {
                nameTextTransform.GetComponent<Text>().text = buildingData[i].assetName;
            }

            // Add BuildingGridCellButton component if it doesn't exist
            BuildingGridCellButton cellButton = cellGameObject.GetComponent<BuildingGridCellButton>();
            if (cellButton == null)
            {
                cellButton = cellGameObject.AddComponent<BuildingGridCellButton>();
            }

            // Find or add an Image component for the building icon
            Image buildingImage = cellGameObject.GetComponent<Image>();
            if (buildingImage == null)
            {
                // If the root doesn't have an Image, look for a child Image
                buildingImage = cellGameObject.transform.Find("BuildingImage")?.GetComponent<Image>();

                // If still not found, create a new Image GameObject
                if (buildingImage == null)
                {
                    GameObject imageObj = new GameObject("BuildingImage");
                    imageObj.transform.SetParent(cellGameObject.transform, false);
                    buildingImage = imageObj.AddComponent<Image>();

                    // Set the image to fill the cell
                    RectTransform rectTransform = imageObj.GetComponent<RectTransform>();
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.offsetMin = new Vector2(5, 5); // Small padding
                    rectTransform.offsetMax = new Vector2(-5, -25); // Space for text at bottom
                }
            }

            // Assign the image to the cell button
            cellButton.buildingImage = buildingImage;

            // Assign the building data and update the image
            cellButton.SetBuildingData(buildingData[i]);

            // Add Button component if it doesn't exist
            Button button = cellGameObject.GetComponent<Button>();
            if (button == null)
            {
                button = cellGameObject.AddComponent<Button>();
            }

            // Set up the button click event
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(cellButton.OnClickCell);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Add keyboard support for selecting keys 1-0
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) ||
                (i == 9 && Input.GetKeyDown(KeyCode.Alpha0)))
            {
                int keyIndex = (i == 9) ? 9 : i;
                if (selectionUI != null)
                {
                    selectionUI.OnKeyButtonClicked(keyIndex);
                }
            }
        }
    }
}

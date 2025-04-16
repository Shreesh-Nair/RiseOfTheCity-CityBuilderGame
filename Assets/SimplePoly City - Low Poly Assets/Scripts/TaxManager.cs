using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TaxManager : MonoBehaviour
{
    // Reference to the BuildingManager to get the taxable building count
    public BuildingManager buildingManager;
    
    // UI elements
    public Text moneyText;
    public Text incomeText;
    
    // Tax variables
    public int  budget = 1000;
    public float taxCollectionInterval = 30f;
    public int gdp;
    public int population;
    public int totalMaintainanceCost = 0;
    
    private float timer = 0f;

    void Start()
    {
        // Find the building manager if not assigned
        if (buildingManager == null)
        {
            buildingManager = FindFirstObjectByType<BuildingManager>();
            if (buildingManager == null)
            {
                Debug.LogError("BuildingManager not found! TaxManager requires a BuildingManager in the scene.");
            }
        }

        
        // Initialize UI
        UpdateMoneyDisplay();
    }
    
    void Update()
    {
        // Count down to next tax collection
        timer += Time.deltaTime;
        budget = buildingManager.totalMoney;
        moneyText.text = $"{budget}";
        gdp=buildingManager.totalCommercialProduction;
        // When time is up, collect taxes
        if (timer >= taxCollectionInterval)
        {
            CollectTaxes();
            UpdateMaintainanceCost();
            timer = 0f;
        }
        population=buildingManager.populationLimit;
        
    
    }

    void UpdateMaintainanceCost()
    {
        totalMaintainanceCost=buildingManager.maintainanceFactor;
    }
    
    void CollectTaxes()
    {
        
        
        UpdateMoneyDisplay();
        
    }
    
    void UpdateMoneyDisplay()
    {   
        if (incomeText != null)
        {
            incomeText.text = $"{gdp}";
        }
        
    }
    
    
}
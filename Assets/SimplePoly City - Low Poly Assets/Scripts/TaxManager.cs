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
    public int money = 1000;
    public int taxPerBuilding = 100;
    public float taxCollectionInterval = 30f;
    public int maintainanceCost=5;
    // Optional visual feedback
    
    
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
        
        // When time is up, collect taxes
        if (timer >= taxCollectionInterval)
        {
            CollectTaxes();
            timer = 0f;
        }
        
        // Optionally update a UI countdown timer
        if (incomeText != null)
        {
            int timeLeft = Mathf.CeilToInt(taxCollectionInterval - timer);
            int income = buildingManager.taxableBuilding * taxPerBuilding;
            int spending=buildingManager.maintainanceRequired*maintainanceCost;
            incomeText.text = $"{income-spending} in {timeLeft}s";

        }
    
    }
    
    void CollectTaxes()
    {
        // Calculate tax based on number of taxable buildings
        int taxAmount = buildingManager.taxableBuilding * taxPerBuilding;
        
        // Add to money
        money += taxAmount;
        
        // Update UI
        UpdateMoneyDisplay();
        
        // Show visual feedback
        if (taxAmount > 0)
        {
            //Debug.Log($"Collected {taxAmount} in taxes from {buildingManager.taxableBuilding} buildings.");
            //PlayTaxCollectionEffect();
        }
    }
    
    void UpdateMoneyDisplay()
    {
        if (moneyText != null)
        {
            moneyText.text = $"{money}";
        }
    }
    
    // Function to check if player can afford something
    public bool CanAfford(int cost)
    {
        return money >= cost;
    }
    
    // Function for spending money
    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyDisplay();
            return true;
        }
        
        return false;
    }
    
    // Optional visual feedback when taxes are collected
    // void PlayTaxCollectionEffect()
    // {
    //     if (coinEffectPrefab != null && uiCanvas != null)
    //     {
    //         GameObject effect = Instantiate(coinEffectPrefab, uiCanvas);
    //         Destroy(effect, 2f); // Destroy after 2 seconds
    //     }
        
    //     // You could also play a sound effect here
    // }
    
    // Public method to get current income rate (for UI or other systems)
    public int GetIncomeRate()
    {
        return buildingManager.taxableBuilding * taxPerBuilding;
    }
    
    // Optional: Manual tax collection for testing or player-triggered collection
    public void ManualTaxCollection()
    {
        CollectTaxes();
        timer = 0f;
    }
}
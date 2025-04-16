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
    public Text populationText;
    public Text Morale;
    public Text Safety;
    public Text ProsperityText;
    public Text ImmigrationRate;
    public Text CrimeRate;


    public int gst = 10;
    public int incomeTax = 10;
    // Tax variables
    public float budget = 10000f;
    public float taxCollectionInterval = 30f;
    public int gdp;
    public int income;
    public int populationCap;
    public float totalMaintainanceCost = 0f;
    public float morale;
    public int population = 0;
    private float timer = 0f;
    public float Prosperity = 0f;
    public float moraleThreshold = 30f;
    public float immigrationRate = 0f;
    public float crimeRate = 0f;
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
        // Replace line 47-50 in TaxManager.cs with:
        

        


        // Initialize UI
        UpdateMoneyDisplay();
    }

    void Update()
    {
        // Count down to next tax collection
        timer += Time.deltaTime;
        budget = buildingManager.totalMoney;
        moneyText.text = $"{budget}";
        populationCap = buildingManager.populationLimit;
        gdp = buildingManager.totalCommercialProduction;
        if (population == 0 && buildingManager.totalBuildings > 0 && populationCap > 1)
        {
            population = 1;
            // Seed initial population when first building placed
        }
        // When time is up, collect taxes
        if (buildingManager.totalBuildings > 0 && population > 0)
        {
            morale = (buildingManager.totalMorale / buildingManager.totalBuildings) * (buildingManager.totalSafety / population) * (1 - buildingManager.pollutionFactor/(buildingManager.totalBuildings * 100f));
            Prosperity = gdp * (buildingManager.totalSafety / population) * (buildingManager.totalMorale / buildingManager.totalBuildings) * (1 - buildingManager.pollutionFactor/(buildingManager.totalBuildings * 100f));
            immigrationRate = (gdp / (population + 1)) * (1 - buildingManager.pollutionFactor / (buildingManager.totalBuildings*100f)) * (1 - population / populationCap) * ((morale) - 30) / 20;
            if (buildingManager.totalSafety > 0) crimeRate = (population * population / buildingManager.totalSafety) * (1 - gdp / (population * 1000));
        }
        if (timer >= taxCollectionInterval)
        {
            CollectTaxes();
            UpdateMaintainanceCost();
            timer = 0f;
        }



    }

    void UpdateMaintainanceCost()
    {
        totalMaintainanceCost = buildingManager.maintainanceFactor/7;
        buildingManager.totalMoney -= totalMaintainanceCost;
        Debug.Log("Updated budget: "+budget);
        
        if (budget < 0)
        {
            budget = 0;
        }
    }

    void CollectTaxes()
    {
        population += (int)immigrationRate;
        Debug.Log("Immigrated: "+population);
        if (population > populationCap)
        {
            population = populationCap;
        }
        if (population < 0)
        {
            population = 0;
        }
        income = (gdp * gst / 100) + (population * incomeTax / 100);
        buildingManager.totalMoney += income;
        UpdateMoneyDisplay();

    }

    void UpdateMoneyDisplay()
    {
        if (incomeText != null)
        {
            incomeText.text = $"{income}";
        }

    }


}
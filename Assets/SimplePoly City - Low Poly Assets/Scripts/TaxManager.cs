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
    public int budget = 1000;
    public float taxCollectionInterval = 30f;
    public int gdp;
    public int income;
    public int populationCap;
    public int totalMaintainanceCost = 0;
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
        float safePop = Mathf.Max(population, 1);
        float normMorale = buildingManager.totalMorale / (buildingManager.totalBuildings * 50f);
        float normSafety = buildingManager.totalSafety / 10f;
        float normPollution = buildingManager.pollutionFactor / 10f;

        morale = normMorale * normSafety * (1 - normPollution);


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
            morale = (buildingManager.totalMorale / buildingManager.totalBuildings) * (buildingManager.totalSafety / population) * (1 - buildingManager.pollutionFactor);
            Prosperity = gdp * (buildingManager.totalSafety / population) * (buildingManager.totalMorale / buildingManager.totalBuildings) * (1 - buildingManager.pollutionFactor);
            immigrationRate = (gdp / (population + 1)) * (1 - buildingManager.pollutionFactor / buildingManager.totalBuildings) * (1 - population / populationCap) * ((morale * 50) - 30) / 20;
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
        totalMaintainanceCost = buildingManager.maintainanceFactor;
    }

    void CollectTaxes()
    {
        population += (int)immigrationRate;
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
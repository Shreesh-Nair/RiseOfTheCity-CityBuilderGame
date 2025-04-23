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
    public Text EconStatus;
    public Text final;
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
    public int currentMaxPop = 0;
    public Text cityStatus;
    public int[] achievements = new int[10]; // Array to track achievements

    void Start()
    {
        Debug.Log("" + achievements.Length);
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
            morale = (buildingManager.totalMorale / buildingManager.totalBuildings) * (buildingManager.totalSafety * 10 / population) * (1 - buildingManager.pollutionFactor / (buildingManager.totalBuildings * 100f));
            Prosperity = gdp * (buildingManager.totalSafety / population) * (buildingManager.totalMorale / buildingManager.totalBuildings) * (1 - buildingManager.pollutionFactor / (buildingManager.totalBuildings * 100f));
            immigrationRate = (gdp / (population + 1)) * (1 - buildingManager.pollutionFactor / (buildingManager.totalBuildings * 100f)) * (1 - population / populationCap) * ((morale) - 30) / 20;
            if (buildingManager.totalSafety > 0) crimeRate = (population / buildingManager.totalSafety) * (1 - gdp / (population * 1000));
        }
        populationText.text = $"{population} / {populationCap}";
        if (population > currentMaxPop)
        {
            currentMaxPop = population;
        }
        if (currentMaxPop > 5000 && achievements[3] != 1)
        {
            cityStatus.text = "Growing City";
            achievements[3] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 100000f;
        }
        else if (currentMaxPop > 1000 && achievements[2] != 1)
        {
            cityStatus.text = "Small Town";
            achievements[2] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 10000f;
        }
        else if (currentMaxPop > 100 && achievements[1] != 1)
        {
            cityStatus.text = "Village";
            achievements[1] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 1000f;
        }
        else if (currentMaxPop >= 1 && achievements[0] != 1)
        {
            cityStatus.text = "Starting Out";
            achievements[0] = 1;
        }
        else if (currentMaxPop >= 10000 && achievements[9] != 1)
        {
            cityStatus.text = "Mega City";
            StartCoroutine(ShowFinalTextForDuration("Congratulations! You have built a thriving city!", 10f));
            achievements[9] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 100000f;
        }
        if (gdp > 1000 && achievements.Length > 4 && achievements[4] != 1)
        {
            achievements[4] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 200f;
            EconStatus.text = "Emerging Settlement";
        }
        else if (gdp > 2500 && achievements.Length > 5 && achievements[5] != 1)
        {
            achievements[5] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 500f;
            EconStatus.text = "Growing Township";
        }
        else if (gdp > 5000 && achievements.Length > 6 && achievements[6] != 1)
        {
            achievements[6] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 1000f;
            EconStatus.text = "Prosperous Municipality";
        }
        else if (gdp > 8000 && achievements.Length > 7 && achievements[7] != 1)
        {
            achievements[7] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 1500f;
            EconStatus.text = "Thriving Metropolis";
        }
        else if (gdp > 10000 && achievements.Length > 8 && achievements[8] != 1)
        {
            achievements[8] = 1;
            buildingManager.totalMoney = buildingManager.totalMoney + 2500f;
            EconStatus.text = "Thriving Metropolis";
        }
        if (timer >= taxCollectionInterval)
        {
            CollectTaxes();
            UpdateMaintainanceCost();
            timer = 0f;
        }



    }
    IEnumerator ShowFinalTextForDuration(string message, float duration)
    {
        final.text = message;
        yield return new WaitForSeconds(duration);
        final.text = string.Empty;
    }

    void UpdateMaintainanceCost()
    {
        totalMaintainanceCost = buildingManager.maintainanceFactor / 7;
        buildingManager.totalMoney -= totalMaintainanceCost;
        Debug.Log("Updated budget: " + budget);

        if (budget < 0)
        {
            budget = 0;
        }
    }

    void CollectTaxes()
    {
        Morale.text = morale.ToString();
        Safety.text = buildingManager.totalSafety.ToString();
        ProsperityText.text = Prosperity.ToString();
        ImmigrationRate.text = immigrationRate.ToString();
        CrimeRate.text = crimeRate.ToString();
        population += (int)immigrationRate;
        Debug.Log("Immigrated: " + population);
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
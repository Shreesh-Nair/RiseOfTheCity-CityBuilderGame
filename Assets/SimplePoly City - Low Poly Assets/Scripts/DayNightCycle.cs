using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]
    [SerializeField] private float _targetDayLength = 0.5f; // length of day in minutes
    public float targetDayLength { get { return _targetDayLength; } }

    [SerializeField] private float elapsedTime;
    [SerializeField] private bool use24Clock = true;
    [SerializeField] private Text clockText;
    [SerializeField][Range(0f, 1f)] private float _timeOfDay = 0.3f; // Default to morning
    public float timeOfDay { get { return _timeOfDay; } }
    [SerializeField] private int _dayNumber = 0; // tracks the days passed
    public int dayNumber { get { return _dayNumber; } }
    [SerializeField] private int _yearNumber = 0;
    public int yearNumber { get { return _yearNumber; } }
    private float _timeScale = 100f;
    [SerializeField] private int _yearLength = 100;
    public float yearLength { get { return _yearLength; } }
    public bool pause = false;
    [SerializeField] private AnimationCurve timeCurve;
    private float timeCurveNormalization = 1f;

    [Header("Sun Light")]
    [SerializeField] private Transform dailyRotation;
    [SerializeField] public Light sun;
    private float intensity;
    [SerializeField] private float sunBaseIntensity = 0.3f;
    [SerializeField] private float sunVariation = 1.5f;
    [SerializeField] private Gradient sunColor;

    [Header("Seasonal Variables")]
    [SerializeField] private Transform sunSeasonalRotation;
    // Removed maxSeasonalTilt, replaced with per-season tilts

    // --- SEASON SYSTEM START ---
    [Header("Seasons")]
    [SerializeField] private string[] seasons = { "Summer", "Monsoon", "Autumn", "Winter" };
    [SerializeField] private float[] seasonTilts = { 30f, 15f, -15f, -30f };
    [SerializeField] public string currentSeason = "Summer";
    // --- SEASON SYSTEM END ---

    [Header("Moon Light")]
    [SerializeField] private Light moon;
    [SerializeField] private Transform moonDailyRotation;
    [SerializeField] private Transform moonSeasonalRotation;
    [SerializeField] private float moonBaseIntensity = 0.2f;
    [SerializeField] private float moonVariation = 0.3f;
    [SerializeField] private Gradient moonColor;

    [Header("Modules")]
    private List<DNModuleBase> moduleList = new List<DNModuleBase>();

    private void Start()
    {
        ValidateComponents();
        NormalTimeCurve();
        AdjustSunRotation();
        AdjustMoonRotation();
        SunIntensity();
        MoonIntensity();
        AdjustSunColor();
        AdjustMoonColor();
    }

    private void ValidateComponents()
    {
        if (dailyRotation == null)
            Debug.LogError("Daily Rotation transform is not assigned!");
        if (sunSeasonalRotation == null)
            Debug.LogError("Sun Seasonal Rotation transform is not assigned!");
        if (sun == null)
            Debug.LogError("Sun light is not assigned!");
        if (moon == null)
            Debug.LogWarning("Moon light is not assigned! Moon functionality will be disabled.");
        if (moonDailyRotation == null)
            Debug.LogWarning("Moon Daily Rotation transform is not assigned! Moon rotation will not work properly.");
        if (moonSeasonalRotation == null)
            Debug.LogWarning("Moon Seasonal Rotation transform is not assigned! Moon seasonal tilt will not work properly.");
        if (timeCurve == null || timeCurve.keys.Length == 0)
        {
            timeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
            Debug.LogWarning("Time Curve was not set. Using default flat curve.");
        }
        if (moonColor == null || moonColor.colorKeys.Length == 0)
        {
            // Create a default blue-ish gradient for the moon
            moonColor = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(new Color(0.5f, 0.5f, 1f, 1f), 0f);
            colorKeys[1] = new GradientColorKey(new Color(0.8f, 0.8f, 1f, 1f), 1f);
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);
            alphaKeys[1] = new GradientAlphaKey(1f, 1f);
            moonColor.SetKeys(colorKeys, alphaKeys);
            Debug.LogWarning("Moon Color gradient was not set. Using default blue gradient.");
        }
    }

    private void Update()
    {
        if (!pause)
        {
            UpdateTimeScale();
            UpdateTime();
            UpdateClock();
        }
        AdjustSunRotation();
        AdjustMoonRotation();
        SunIntensity();
        MoonIntensity();
        AdjustSunColor();
        AdjustMoonColor();
        UpdateModules();
    }

    private void UpdateTimeScale()
    {
        // Prevent division by zero
        if (_targetDayLength <= 0)
        {
            _targetDayLength = 0.5f;
            Debug.LogWarning("Target day length was <= 0, reset to 0.5");
        }
        // Calculate time scale based on target day length
        _timeScale = 24f / (_targetDayLength / 60f);
        // Apply time curve if we have a valid elapsed time to evaluate
        if (targetDayLength > 0)
        {
            float normalizedTime = Mathf.Clamp01(elapsedTime / (targetDayLength * 60f));
            _timeScale *= timeCurve.Evaluate(normalizedTime);
        }
        // Apply normalization
        if (timeCurveNormalization > 0)
        {
            _timeScale /= timeCurveNormalization;
        }
    }

    private void NormalTimeCurve()
    {
        float stepSize = 0.01f;
        int numberSteps = Mathf.FloorToInt(1f / stepSize);
        float curveTotal = 0;
        for (int i = 0; i < numberSteps; i++)
        {
            curveTotal += timeCurve.Evaluate(i * stepSize);
        }
        timeCurveNormalization = curveTotal / numberSteps;
        // Prevent division by zero
        if (timeCurveNormalization <= 0)
        {
            timeCurveNormalization = 1f;
            Debug.LogWarning("Time curve normalization was <= 0, reset to 1.0");
        }
    }

    private void UpdateTime()
    {
        // Calculate time increment
        float timeIncrement = Time.deltaTime * _timeScale / 86400f;
        // Update time of day and elapsed time
        _timeOfDay += timeIncrement;
        elapsedTime += Time.deltaTime;
        // Handle day rollover
        if (_timeOfDay >= 1f)
        {
            elapsedTime = 0;
            _dayNumber++;
            _timeOfDay -= Mathf.Floor(_timeOfDay); // Only subtract the whole number part
            // Handle year rollover
            if (_dayNumber >= _yearLength && _yearLength > 0)
            {
                _yearNumber++;
                _dayNumber = 0;
            }
        }
    }

    private void UpdateClock()
    {
        if (clockText == null)
            return;
        // Calculate hours and minutes based on time of day (0-1)
        float hourFloat = _timeOfDay * 24f;
        int hour = Mathf.FloorToInt(hourFloat);
        int minute = Mathf.FloorToInt((hourFloat - hour) * 60f);
        // Format time strings
        string hourString;
        string minuteString;
        // Convert to 12-hour format if needed
        int displayHour = hour;
        if (!use24Clock)
        {
            if (hour == 0)
                displayHour = 12;
            else if (hour > 12)
                displayHour = hour - 12;
        }
        // Add leading zeros if needed
        hourString = displayHour < 10 ? "0" + displayHour : displayHour.ToString();
        minuteString = minute < 10 ? "0" + minute : minute.ToString();
        // Set the clock text
        if (use24Clock)
            clockText.text = hourString + ":" + minuteString;
        else
            clockText.text = hourString + ":" + minuteString + (hour < 12 ? " AM" : " PM");
    }

    private void AdjustSunRotation()
    {
        if (dailyRotation == null || sunSeasonalRotation == null)
            return;

        // Calculate daily sun rotation
        float sunAngle = _timeOfDay * 360f;
        Quaternion dailyRot = Quaternion.Euler(new Vector3(0f, 0f, sunAngle));
        if (!float.IsNaN(dailyRot.x) && !float.IsNaN(dailyRot.y) && !float.IsNaN(dailyRot.z) && !float.IsNaN(dailyRot.w))
            dailyRotation.localRotation = dailyRot;

        // --- SEASON SYSTEM ---
        // Determine current season by dividing the year into 4 equal parts
        int seasonCount = seasons.Length;
        int daysPerSeason = Mathf.Max(1, _yearLength / seasonCount);
        int seasonIndex = (_dayNumber / daysPerSeason) % seasonCount;
        currentSeason = seasons[seasonIndex];
        float seasonalAngle = -seasonTilts[seasonIndex];
        Quaternion seasonalRot = Quaternion.Euler(new Vector3(seasonalAngle, 0f, 0f));
        if (!float.IsNaN(seasonalRot.x) && !float.IsNaN(seasonalRot.y) && !float.IsNaN(seasonalRot.z) && !float.IsNaN(seasonalRot.w))
            sunSeasonalRotation.localRotation = seasonalRot;
        // --- SEASON SYSTEM END ---
    }

    private void AdjustMoonRotation()
    {
        if (moon == null || moonDailyRotation == null || moonSeasonalRotation == null)
            return;

        // Calculate moon rotation (180 degrees opposite from sun)
        float moonAngle = (_timeOfDay * 360f + 180f) % 360f;
        Quaternion dailyRot = Quaternion.Euler(new Vector3(0f, 0f, moonAngle));
        if (!float.IsNaN(dailyRot.x) && !float.IsNaN(dailyRot.y) && !float.IsNaN(dailyRot.z) && !float.IsNaN(dailyRot.w))
            moonDailyRotation.localRotation = dailyRot;

        // --- SEASON SYSTEM ---
        int seasonCount = seasons.Length;
        int daysPerSeason = Mathf.Max(1, _yearLength / seasonCount);
        int seasonIndex = (_dayNumber / daysPerSeason) % seasonCount;
        float seasonalAngle = seasonTilts[seasonIndex];
        Quaternion seasonalRot = Quaternion.Euler(new Vector3(seasonalAngle, 0f, 0f));
        if (!float.IsNaN(seasonalRot.x) && !float.IsNaN(seasonalRot.y) && !float.IsNaN(seasonalRot.z) && !float.IsNaN(seasonalRot.w))
            moonSeasonalRotation.localRotation = seasonalRot;
        // --- SEASON SYSTEM END ---
    }

    private void SunIntensity()
    {
        if (sun == null)
            return;
        // Calculate sun intensity based on its orientation relative to "down"
        intensity = Vector3.Dot(sun.transform.forward, Vector3.down);
        intensity = Mathf.Clamp01(intensity);
        // Apply intensity to light
        sun.intensity = intensity * sunVariation + sunBaseIntensity;
    }

    private void MoonIntensity()
    {
        if (moon == null)
            return;
        // Moon is brightest when sun intensity is lowest
        float moonIntensity = 1f - intensity;
        moonIntensity = Mathf.Clamp01(moonIntensity);
        // Set moon intensity (inverse of sun intensity)
        moon.intensity = moonIntensity * moonVariation + moonBaseIntensity;
    }

    private void AdjustSunColor()
    {
        if (sun == null || sunColor == null)
            return;
        // Set sun color based on intensity
        sun.color = sunColor.Evaluate(intensity);
    }

    private void AdjustMoonColor()
    {
        if (moon == null || moonColor == null)
            return;
        // Set moon color based on inverse of sun intensity
        moon.color = moonColor.Evaluate(1f - intensity);
    }

    public void AddModule(DNModuleBase module)
    {
        if (module != null && !moduleList.Contains(module))
            moduleList.Add(module);
    }

    public void RemoveModule(DNModuleBase module)
    {
        if (module != null)
            moduleList.Remove(module);
    }

    private void UpdateModules()
    {
        foreach (DNModuleBase module in moduleList)
        {
            if (module != null)
                module.UpdateModule(intensity);
        }
    }

    // Utility method to manually set the time of day (0-1)
    public void SetTimeOfDay(float newTime)
    {
        _timeOfDay = Mathf.Clamp01(newTime);
        AdjustSunRotation();
        AdjustMoonRotation();
        SunIntensity();
        MoonIntensity();
        AdjustSunColor();
        AdjustMoonColor();
    }
}

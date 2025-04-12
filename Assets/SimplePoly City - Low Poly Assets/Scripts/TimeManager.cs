using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Day-Night Cycle Settings")]
    public Light sunLight;
    public Light moonLight;
    [Range(10f, 600f)]
    public float dayDuration = 300f;
    [Range(0f, 360f)]
    public float initialTimeOfDay = 0f;

    [Header("Lighting Settings")]
    public Color dayAmbientLight = Color.white;
    public Color nightAmbientLight = new Color(0.05f, 0.05f, 0.1f);
    public Color sunsetColor = new Color(1f, 0.5f, 0.3f);
    [Range(0f, 2f)]
    public float maxSunIntensity = 1f;
    [Range(0f, 2f)]
    public float minSunIntensity = 0.1f;
    [Range(0f, 1f)]
    public float maxMoonIntensity = 0.3f;
    [Range(0f, 1f)]
    public float minMoonIntensity = 0.05f;

    [Header("Sky Settings")]
    public Material daySkybox;
    public Material nightSkybox;
    private Material currentSkybox;

    [Header("Season Settings")]
    public int currentDay = 1;
    public int daysPerSeason = 7;
    public int currentYear = 1;

    private float timeOfDay;

    void Start()
    {
        timeOfDay = initialTimeOfDay;
        currentSkybox = daySkybox;
        RenderSettings.skybox = currentSkybox;
        UpdateLighting();
    }

    void Update()
    {
        timeOfDay += (Time.deltaTime / dayDuration) * 360f;
        if (timeOfDay >= 360f)
        {
            timeOfDay = 0f;
            currentDay++;
            if (currentDay > daysPerSeason * 4)
            {
                currentDay = 1;
                currentYear++;
            }
        }
        UpdateLighting();
    }

    void UpdateLighting()
    {
        if (sunLight != null)
        {
            // Calculate sun position
            float sunElevation = Mathf.Sin(timeOfDay * Mathf.Deg2Rad) * 90f;
            float sunAzimuth = timeOfDay;
            sunLight.transform.rotation = Quaternion.Euler(sunElevation, sunAzimuth, 0f);

            // Calculate sun intensity based on elevation
            float sunFactor = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((sunElevation + 12f) / 90f));
            sunLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, sunFactor);

            // Update moon light (opposite to sun)
            if (moonLight != null)
            {
                moonLight.transform.rotation = Quaternion.Euler(-sunElevation, (sunAzimuth + 180f) % 360f, 0f);
                moonLight.intensity = Mathf.Lerp(maxMoonIntensity, minMoonIntensity, sunFactor);
            }

            // Calculate ambient light with sunset/sunrise effect
            bool isSunsetOrSunrise = sunElevation > -20f && sunElevation < 10f;
            Color ambientColor;

            if (isSunsetOrSunrise)
            {
                float sunsetFactor = 1f - Mathf.Abs(sunElevation) / 20f;
                ambientColor = Color.Lerp(
                    Color.Lerp(nightAmbientLight, dayAmbientLight, sunFactor),
                    sunsetColor,
                    sunsetFactor
                );
            }
            else
            {
                ambientColor = Color.Lerp(nightAmbientLight, dayAmbientLight, sunFactor);
            }

            RenderSettings.ambientLight = ambientColor;

            // Switch skybox based on time of day
            bool isDay = sunElevation > 0;
            if ((isDay && currentSkybox != daySkybox) || (!isDay && currentSkybox != nightSkybox))
            {
                SwitchSkybox(isDay);
            }

            // Adjust skybox brightness
            float dayNightFactor = Mathf.Clamp01((sunElevation + 20f) / 40f);
            if (currentSkybox != null)
            {
                // Reduce exposure during night time
                float skyExposure = Mathf.Lerp(0.1f, 1.0f, dayNightFactor);
                currentSkybox.SetFloat("_Exposure", skyExposure);

                // Adjust overall tint to darken the sky at night
                Color skyTint = Color.Lerp(new Color(0.2f, 0.2f, 0.3f), Color.white, dayNightFactor);
                currentSkybox.SetColor("_Tint", skyTint);
            }
        }
    }

    void SwitchSkybox(bool isDay)
    {
        currentSkybox = isDay ? daySkybox : nightSkybox;
        RenderSettings.skybox = currentSkybox;
    }

    public Season GetCurrentSeason()
    {
        int seasonIndex = ((currentDay - 1) / daysPerSeason) % 4;
        return (Season)seasonIndex;
    }

    public float GetTimeOfDay()
    {
        return timeOfDay;
    }

    public int GetCurrentHour()
    {
        return Mathf.FloorToInt((timeOfDay / 360f) * 24f);
    }

    public float GetDayNormalizedTime()
    {
        return timeOfDay / 360f;
    }
}

public enum Season
{
    Summer = 0,
    Monsoon = 1,
    Autumn = 2,
    Winter = 3
}

using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Day-Night Cycle Settings")]
    public Light sunLight;
    public Light moonLight;  // Added moonlight

    [Range(10f, 600f)]
    public float dayDuration = 300f; // Increased for more realism
    [Range(0f, 360f)]
    public float initialTimeOfDay = 0f;

    [Header("Lighting Settings")]
    public Color dayAmbientLight = Color.white;
    public Color nightAmbientLight = Color.black;

    [Range(0f, 2f)]
    public float maxSunIntensity = 1f;
    [Range(0f, 2f)]
    public float minSunIntensity = 0.1f;

    private float timeOfDay;

    void Start()
    {
        timeOfDay = initialTimeOfDay;
        UpdateLighting();
    }

    void Update()
    {
        timeOfDay += (Time.deltaTime / dayDuration) * 360f;
        if (timeOfDay >= 360f) timeOfDay = 0f;

        UpdateLighting();
    }

    void UpdateLighting()
    {
        if (sunLight != null)
        {
            float sunElevation = Mathf.Sin(timeOfDay * Mathf.Deg2Rad) * 90f;
            float sunAzimuth = Mathf.Lerp(-90f, 90f, (timeOfDay / 360f)); // Fixes rise/set direction
            sunLight.transform.rotation = Quaternion.Euler(sunElevation, sunAzimuth, 0f);

            float sunFactor = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.Deg2Rad) * 0.5f + 0.5f));
            sunLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, sunFactor);

            if (moonLight != null)
                moonLight.intensity = Mathf.Lerp(0.1f, 0.3f, 1f - sunFactor);

            Color sunsetColor = Color.Lerp(nightAmbientLight, new Color(1f, 0.5f, 0.3f), Mathf.Abs(Mathf.Sin(timeOfDay * Mathf.Deg2Rad)));
            RenderSettings.ambientLight = Color.Lerp(sunsetColor, dayAmbientLight, sunFactor);
        }
    }

}

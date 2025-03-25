using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Day-Night Cycle Settings")]
    [Tooltip("Directional Light representing the Sun.")]
    public Light sunLight;

    [Tooltip("Duration of a full day-night cycle in seconds.")]
    [Range(10f, 600f)]
    public float dayDuration = 120f;

    [Tooltip("Initial time of day in degrees (0 = sunrise, 180 = sunset).")]
    [Range(0f, 360f)]
    public float initialTimeOfDay = 0f;

    [Header("Lighting Settings")]
    [Tooltip("Ambient light color during the day.")]
    public Color dayAmbientLight = Color.white;

    [Tooltip("Ambient light color during the night.")]
    public Color nightAmbientLight = Color.black;

    [Tooltip("Maximum intensity of the sun during the day.")]
    [Range(0f, 2f)]
    public float maxSunIntensity = 1f;

    [Tooltip("Minimum intensity of the sun during the night.")]
    [Range(0f, 2f)]
    public float minSunIntensity = 0.1f;

    private float timeOfDay;

    void Start()
    {
        // Initialize the time of day
        timeOfDay = initialTimeOfDay;

        // Set the initial ambient light
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, Mathf.Cos(timeOfDay * Mathf.Deg2Rad));
    }

    void Update()
    {
        if (sunLight != null)
        {
            // Increment time of day based on real time
            timeOfDay += (Time.deltaTime / dayDuration) * 360f;

            // Rotate the sun to simulate the day-night cycle
            sunLight.transform.rotation = Quaternion.Euler(new Vector3(timeOfDay, 170f, 0f));

            // Reset timeOfDay after a full cycle
            if (timeOfDay >= 360f)
            {
                timeOfDay = 0f;
            }

            // Adjust the sun's intensity based on the time of day
            float sunIntensityFactor = Mathf.Clamp01(Mathf.Cos(timeOfDay * Mathf.Deg2Rad));
            sunLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, sunIntensityFactor);

            // Adjust the ambient light color based on the time of day
            RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, sunIntensityFactor);
        }
    }
}
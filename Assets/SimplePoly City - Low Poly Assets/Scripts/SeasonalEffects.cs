using UnityEngine;

public class SeasonalEffects : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public ParticleSystem snowEffects;
    public ParticleSystem rainEffects;

    // Assign your Sun (Directional Light) and Skybox Material in the Inspector
    public Light sunLight;
    public Material skyboxMaterial;

    // Store original values to restore them as seasons change
    private float originalSunIntensity;
    private Color originalSkyTint;

    private string lastSeason;

    private void Start()
    {
        // If not assigned, try to auto-find the sun from DayNightCycle
        if (sunLight == null && dayNightCycle != null)
            sunLight = dayNightCycle.sun;

        // Store original sun intensity
        if (sunLight != null)
            originalSunIntensity = sunLight.intensity;

        // Store original skybox tint if available
        if (skyboxMaterial != null && skyboxMaterial.HasProperty("_SkyTint"))
            originalSkyTint = skyboxMaterial.GetColor("_SkyTint");

        lastSeason = dayNightCycle.currentSeason;
        UpdateEffects(lastSeason);
    }

    private void Update()
    {
        if (dayNightCycle.currentSeason != lastSeason)
        {
            lastSeason = dayNightCycle.currentSeason;
            UpdateEffects(lastSeason);
        }
    }

    private void UpdateEffects(string season)
    {
        // Particle effects (your original logic)
        if (season == "Winter")
        {
            if (snowEffects != null) snowEffects.Play();
            if (rainEffects != null) rainEffects.Stop();
        }
        else if (season == "Monsoon")
        {
            if (rainEffects != null) rainEffects.Play();
            if (snowEffects != null) snowEffects.Stop();
        }
        else
        {
            if (snowEffects != null) snowEffects.Stop();
            if (rainEffects != null) rainEffects.Stop();
        }

        // Simple brightness and tint logic
        if (sunLight != null)
        {
            // Dim the sun in Monsoon and Winter, normal in Summer and Autumn
            if (season == "Monsoon" || season == "Winter")
                sunLight.intensity = originalSunIntensity * 0.7f; // 30% dimmer
            else
                sunLight.intensity = originalSunIntensity;
        }

        if (skyboxMaterial != null && skyboxMaterial.HasProperty("_SkyTint"))
        {
            // Add a warm hue in Autumn, reset otherwise
            if (season == "Autumn")
                skyboxMaterial.SetColor("_SkyTint", new Color(1.0f, 0.8f, 0.6f)); // warm autumn tint
            else
                skyboxMaterial.SetColor("_SkyTint", originalSkyTint);
        }
    }
}

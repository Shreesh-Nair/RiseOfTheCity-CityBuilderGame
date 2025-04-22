using UnityEngine;

public class SeasonalEffects : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public ParticleSystem snowEffects;
    public ParticleSystem rainEffects;

    private string lastSeason;

    private void Start() {
        lastSeason = dayNightCycle.currentSeason;
        UpdateEffects(lastSeason);
    }

    private void Update() {
        if (dayNightCycle.currentSeason != lastSeason) {
            lastSeason = dayNightCycle.currentSeason;
            UpdateEffects(lastSeason);
        }
    }

    private void UpdateEffects(string season) {
        if (season == "Winter") {
            snowEffects.Play();
            rainEffects.Stop();
        } else if (season == "Monsoon") {
            rainEffects.Play();
            snowEffects.Stop();
        } else {
            snowEffects.Stop();
            rainEffects.Stop();
        }
    }
}

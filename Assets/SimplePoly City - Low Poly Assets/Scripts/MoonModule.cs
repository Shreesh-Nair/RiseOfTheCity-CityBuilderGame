using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonModule : DNModuleBase
{
    [SerializeField]
    private Light moon;
    [SerializeField]
    private Gradient moonColor;
    [SerializeField]
    private float baseIntensity;

    public Transform moonDailyRotation;

    void Start()
    {
        // Find or create daily rotation transform
        //moonDailyRotation = transform.Find("Moon Daily Rotation");
        if (moonDailyRotation == null)
        {
            Debug.LogError("Moon Daily Rotation not found!");
        }
    }

    public override void UpdateModule(float intensity)
    {
        // Set moon color and intensity (opposite to sun)
        moon.color = moonColor.Evaluate(1 - intensity);
        moon.intensity = (1 - intensity) * baseIntensity + 0.05f;

        // Optional: Position moon opposite to sun (180° offset)
        if (moonDailyRotation != null)
        {
            float moonAngle = (dayNightControl.timeOfDay * 360f + 180f) % 360f;
            moonDailyRotation.localRotation = Quaternion.Euler(0f, 0f, moonAngle);
        }
    }

}

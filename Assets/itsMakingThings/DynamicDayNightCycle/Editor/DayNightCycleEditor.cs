using UnityEngine;
using UnityEditor;

namespace itsmakingthings_daynightcycle
{
    [CustomEditor(typeof(DayNightCycle))]
    public class DayNightCycleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DayNightCycle cycle = (DayNightCycle)target;

            GUILayout.Space(10);
            GUILayout.Label("🔆 Set Time of Day", EditorStyles.boldLabel);

            if (GUILayout.Button("🌅 Set to Daybreak"))
            {
                cycle.SetToDaybreak();
            }

            if (GUILayout.Button("☀ Set to Midday"))
            {
                cycle.SetToMidday();
            }

            if (GUILayout.Button("🌇 Set to Sunset"))
            {
                cycle.SetToSunset();
            }

            if (GUILayout.Button("🌙 Set to Night"))
            {
                cycle.SetToNight();
            }
        }
    }
}
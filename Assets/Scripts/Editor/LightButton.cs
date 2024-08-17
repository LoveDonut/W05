using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightManager))]

public class LightButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LightManager myScript = (LightManager)target;
        if (GUILayout.Button("Turn Off Lights"))
        {
            myScript.TurnOffLights();
        }
        if (GUILayout.Button("Turn On Lights"))
        {
            myScript.TurnOnLights();
        }
        if (GUILayout.Button("Turn On Blink Lights"))
        {
            myScript.TurnOnBlinkLights();
        }
        if (GUILayout.Button("Turn On Flicker Lights"))
        {
            myScript.TurnOnFlickerLights();
        }
        if (GUILayout.Button("Set Basic Light Intensity"))
        {
            myScript.SetBasicLightIntensity();
        }
    }
}

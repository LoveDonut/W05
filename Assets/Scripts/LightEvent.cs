using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEvent : MonoBehaviour
{
    [SerializeField]private LightManager[] lightManagers;
    // Start is called before the first frame update

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            CallTurnOffLights();
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            CallTurnOnLights();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            CallTurnOnBlinkLights();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            CallTurnOnFlickerLights();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            CallSetBasicLightIntensity();
        }
    }
    public void CallTurnOffLights()
    {
        foreach (var lightManager in lightManagers)
        {
            lightManager.TurnOffLights();
        }
    }

    public void CallTurnOnLights()
    {
        foreach (var lightManager in lightManagers)
        {
            lightManager.TurnOnLights();
        }
    }

    public void CallTurnOnBlinkLights()
    {
        foreach (var lightManager in lightManagers)
        {
            lightManager.TurnOnBlinkLights();
        }
    }

    public void CallTurnOnFlickerLights()
    {
        foreach (var lightManager in lightManagers)
        {
            lightManager.TurnOnFlickerLights();
        }
    }

    public void CallSetBasicLightIntensity()
    {
        foreach (var lightManager in lightManagers)
        {
            lightManager.SetBasicLightIntensity();
        }
    }
}

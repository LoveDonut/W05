using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEvent : MonoBehaviour
{
    [SerializeField]private LightManager[] _lightManagers;
    // Start is called before the first frame update
    public virtual void TriggerLightEvent()
    {

    }
    public void CallTurnOffLights()
    {
        foreach (var lightManager in _lightManagers)
        {
            lightManager.TurnOffLights();
        }
    }

    public void CallTurnOnLights()
    {
        foreach (var lightManager in _lightManagers)
        {
            lightManager.TurnOnLights();
        }
    }

    public void CallTurnOnBlinkLights()
    {
        foreach (var lightManager in _lightManagers)
        {
            lightManager.TurnOnBlinkLights();
        }
    }

    public void CallTurnOnFlickerLights()
    {
        foreach (var lightManager in _lightManagers)
        {
            lightManager.TurnOnFlickerLights();
        }
    }

    public void CallSetBasicLightIntensity()
    {
        foreach (var lightManager in _lightManagers)
        {
            lightManager.SetBasicLightIntensity();
        }
    }

    public void SetLightIntensity(float intensity)
    {
        foreach (var lightManager in _lightManagers)
        {
            lightManager.SetLightIntensity(intensity);
        }
    }
}

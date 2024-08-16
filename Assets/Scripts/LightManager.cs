using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _fluorescentLights;
    [SerializeField] private GameObject[] _basicLights;
    // Start is called before the first frame update
    public float BasicLightIntensity = 1f;

    public void TurnOffLights()
    {
        foreach (var light in _fluorescentLights)
        {
            light.GetComponent<BlinkLight>().BlinkTypeValue = BlinkLight.BlinkType.Off;
        }
    }

    public void TurnOnLights()
    {
        foreach (var light in _fluorescentLights)
        {
            light.GetComponent<BlinkLight>().BlinkTypeValue = BlinkLight.BlinkType.Normal;
        }
    }

    public void TurnOnBlinkLights()
    {
        foreach (var light in _fluorescentLights)
        {
            light.SetActive(false);
        }
        foreach (var light in _fluorescentLights)
        {
            var blinkLight = light.GetComponent<BlinkLight>();
            if (blinkLight != null)
            {
                blinkLight.BlinkTypeValue = BlinkLight.BlinkType.Blink;
                light.SetActive(true);
            }
        }
    }

    public void TurnOnFlickerLights()
    {
        foreach (var light in _fluorescentLights)
        {
            light.SetActive(false);
        }
        foreach (var light in _fluorescentLights)
        {
            var blinkLight = light.GetComponent<BlinkLight>();
            if (blinkLight != null)
            {
                blinkLight.BlinkTypeValue = BlinkLight.BlinkType.Flicker;
                light.SetActive(true);
            }
        }
    }
    public void SetBasicLightIntensity()
    {
        foreach (var light in _basicLights)
        {
            light.GetComponent<Light>().intensity = BasicLightIntensity;
        }
    }
}

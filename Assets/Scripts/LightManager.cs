using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _fluorescentLights;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffLights()
    {
        foreach (var light in _fluorescentLights)
        {
            light.SetActive(false);
        }
    }

    public void TurnOnLights()
    {
        foreach (var light in _fluorescentLights)
        {
            light.SetActive(true);
        }
    }

    public void TurnOnBlinkLights()
    {
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
            var blinkLight = light.GetComponent<BlinkLight>();
            if (blinkLight != null)
            {
                blinkLight.BlinkTypeValue = BlinkLight.BlinkType.Flicker;
                light.SetActive(true);
            }
        }
    }
}

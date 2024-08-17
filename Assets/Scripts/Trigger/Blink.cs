using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : LightEvent
{
    [SerializeField] private float time = 0.1f;
    [SerializeField] private float afterIntensity = 1f;
    public override void TriggerLightEvent()
    {
        StartCoroutine(BlinkLight());
    }

    private IEnumerator BlinkLight()
    {
        CallTurnOffLights();
        yield return new WaitForSeconds(time);
        SetLightIntensity(afterIntensity);
        CallTurnOnBlinkLights();
        yield return new WaitForSeconds(5);
        SetLightIntensity(0.7f);
        CallTurnOnFlickerLights();

    }
}


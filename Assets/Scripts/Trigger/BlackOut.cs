using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackOut : LightEvent
{
    [SerializeField] private bool _turnOn = false;
    [SerializeField] private bool _flickerLight = false;
    [SerializeField] private float time = 3f;
    [SerializeField] private float afterIntensity = 0.7f;
    public override void TriggerLightEvent()
    {
        StartCoroutine(BlackOutLight());
    }

    private IEnumerator BlackOutLight()
    {
        CallTurnOffLights();
        yield return new WaitForSeconds(time);
        if (_turnOn)
        {
            SetLightIntensity(afterIntensity);
            if (_flickerLight)
                CallTurnOnFlickerLights();
            else 
                CallTurnOnLights();
        }
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BackRoomEnviroment : MonoBehaviour
{
    public Color newAmbientLightColor = Color.white;
    public PostProcessVolume postProcessVolume;
    public Light[] lights;
    private bool isChanged = false;

    IEnumerator ChangeAmbientLight(Color color)
    {
        isChanged = true;
        yield return new WaitForSeconds(1f);
        foreach (Light light in lights)
        {
            light.intensity = 0;
            light.enabled = !light.enabled;
        }
        foreach (Light light in lights)
        {
            while(light.intensity < 1)
            {
                light.intensity += Time.deltaTime;
                yield return null;
            }
        }
        postProcessVolume.profile.GetSetting<Bloom>().enabled.value = false;
        postProcessVolume.profile.GetSetting<AutoExposure>().enabled.value = false;
    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isChanged)
            {
                StartCoroutine(ChangeAmbientLight(newAmbientLightColor));
            }
        }
    }
}

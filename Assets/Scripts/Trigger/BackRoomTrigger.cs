using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BackRoomTrigger : LightEvent
{
    public PostProcessVolume postProcessVolume;
    public Light[] lights;
    public GameObject fluorescentLight;
    public GameObject Player;

    public override void TriggerLightEvent()
    {
        StartCoroutine(StartBackRoom());
    }

    IEnumerator StartBackRoom()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        postProcessVolume.profile.GetSetting<Bloom>().enabled.value = true;
        postProcessVolume.profile.GetSetting<AutoExposure>().enabled.value = true;
        Instantiate(fluorescentLight, new Vector3(Player.transform.position.x, Player.transform.position.y+5, Player.transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1f);
    }
}

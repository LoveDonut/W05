using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : LightEvent
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private GameObject SameTrigger;
    public override void TriggerLightEvent()
    {
        StartCoroutine(OpenDoor());
        if(SameTrigger != null)
        {
            SameTrigger.SetActive(false);
        }
    }

    private IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(3f);
        foreach (var door in _doors)
        {
            door.ToggleDoor();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : LightEvent
{
    [SerializeField] private Door[] _doors;
    public override void TriggerLightEvent()
    {
        StartCoroutine(OpenDoor());
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

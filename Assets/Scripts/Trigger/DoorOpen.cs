using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : LightEvent
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private GameObject SameTrigger;
    [SerializeField] AudioClip StartBigSound;

    SoundManager soundManager;
    Status status;
    Transform playerTransform;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();   
        status = FindObjectOfType<Status>();
        playerTransform = FindObjectOfType<PlayerController>().transform;

    }

    public override void TriggerLightEvent()
    {
        if (StartBigSound != null)
        {
            soundManager.PlaySoundOnce(StartBigSound, playerTransform.position);
        }

        // start player's sight decrease
        status.IsStart = true;

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

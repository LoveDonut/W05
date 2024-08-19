using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMake : LightEvent
{
    SoundManager soundManager;
    PlayerController playerController;
    [SerializeField] AudioClip monsterSound;
    [SerializeField] Vector3 stairPosition;

    [Header("트리거 작동시 활성화할 오브젝트")]
    public GameObject[] bloods;
    public GameObject FireDoor;
    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void TriggerLightEvent()
    {
        foreach (GameObject blood in bloods)
        {
            blood.SetActive(true);
        }

        FireDoor.SetActive(false);

        // Turn on sound
        soundManager.PlayMonsterSound();

        // shake camera
        playerController.StartCameraShake();
    }
}

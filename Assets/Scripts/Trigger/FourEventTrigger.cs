using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FourEventTrigger : LightEvent
{
    [SerializeField] private bool _turnOn = false;
    [SerializeField] private bool _flickerLight = false;
    [SerializeField] private float time = 3f;
    [SerializeField] private float afterIntensity = 0.7f;
    [SerializeField] AudioClip monsterSound;
    [SerializeField] Vector3 stairPosition;

    [Header("트리거 작동시 활성화할 오브젝트")]
    public GameObject[] bloods;
    public GameObject FireDoor;

    bool isTrig = false;

    SoundManager soundManager;
    PlayerController playerController;


    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void TriggerLightEvent()
    {
        if (isTrig) return;

        foreach (GameObject blood in bloods)
        {
            blood.SetActive(true);
        }

        if (FireDoor != null)
            FireDoor.SetActive(false);

        // Turn on sound
        if (monsterSound != null)
            soundManager.PlayMonsterSound();

        // shake camera
        if (playerController != null)
            playerController.StartCameraShake();

        StartCoroutine(BlackOutLight());

        isTrig = true;
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class EnterBasement : MonoBehaviour
{
    [SerializeField] LockDoor basementDoor;

    [SerializeField] GameObject hands;
    [SerializeField] GameObject enemy;
    [SerializeField] Light playerLight;
    [SerializeField] float lightOfftime = 3f;
    [SerializeField] AudioClip horrorSFX;
    [SerializeField] AudioClip backroomSFX;
    [SerializeField] GameObject[] blockedCubes;
    [SerializeField] LightManager[] lightManagers;

    SoundManager soundManager;
    
    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    

    public void EventOccur()
    {
        Invoke("LightOff", lightOfftime);
    }
    
    void LightOff()
    {
        playerLight.enabled = false;
        hands.SetActive(true);
        enemy.SetActive(true);
        soundManager.PlayBackgroundSound(horrorSFX);

        //TODO : Turn off light
        foreach (LightManager lightManager in lightManagers)
        {
            lightManager.TurnOffLights();
        }
        //

        Invoke("LightOn", lightOfftime);
    }

    void LightOn()
    {
        hands.SetActive(false);
        enemy.SetActive(false);
        soundManager.StopBackgroundSound();

        //TODO : Turn on light
        foreach (LightManager lightManager in lightManagers)
        {
            lightManager.TurnOnLights();
        }
        //
    }

    private void OnTriggerEnter(Collider other)
    {
        basementDoor.Unlock(true); // unlock door
        soundManager.PlayBackgroundSound(backroomSFX);

        foreach (GameObject blockedCube in blockedCubes)
        {
            blockedCube.SetActive(true);
        }

        Destroy(gameObject);
    }
}

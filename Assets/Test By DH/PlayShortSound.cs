using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShortSound : MonoBehaviour
{
    SoundManager soundManager;
    [SerializeField] AudioClip soundClip;
    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soundManager.PlaySoundForSeconds(soundClip, transform.position, 4f);
        }
    }
}

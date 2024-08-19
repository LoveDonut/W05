using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathRoomTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip eventClip;

    private int playerCnt;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = eventClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerCnt++;
    }

    private void OnTriggerExit(Collider other)
    {
        if(playerCnt >= 2)
        {
            audioSource.Play();
        }

    }
}

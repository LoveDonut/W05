using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    #region PrivateMethods
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    void Update()
    {
        transform.position = Camera.main.transform.position;
        Debug.Log($"Position : {transform.position}");
    }
    #endregion

    public void PlayBackgroundSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();

    }

    public void StopBackgroundSound()
    {
        audioSource.Stop();
    }

    public void PlaySound(AudioClip audioClip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(audioClip, pos);
    }
}

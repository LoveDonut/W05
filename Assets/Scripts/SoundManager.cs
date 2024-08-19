using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip monsterSFXWhenOpenDoor;
    [SerializeField] Vector3 monsterSFXPos;

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

    public void PlaySoundOnce(AudioClip audioClip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(audioClip, pos);
    }

    public void PlaySoundForSeconds(AudioClip audioClip, Vector3 pos, float time)
    {
        StartCoroutine(RepeatSound(audioClip, pos, time));
    }

    public void PlayMonsterSoundWhenOpenDoor()
    {
        PlaySoundOnce(monsterSFXWhenOpenDoor, monsterSFXPos);
    }


    IEnumerator RepeatSound(AudioClip audioClip, Vector3 pos, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            AudioSource.PlayClipAtPoint(audioClip, pos);
            elapsedTime += audioClip.length;
            yield return new WaitForSeconds(audioClip.length);
        }
    }
}

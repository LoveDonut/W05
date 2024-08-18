using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class EnterBasement : MonoBehaviour
{
    [SerializeField] LockDoor basementDoor;


    [SerializeField] GameObject monsterPath;

    [SerializeField] float moveSpeed;
    List<Transform> paths;

    [SerializeField] AudioClip backgroundSFX;
    [SerializeField] AudioClip footstepSFX;
    [SerializeField] float startSoundTimeAfterDoorClose = 0.5f;
    [SerializeField] float startBackgroundTimeAfterFootstep = 0.5f;
    SoundManager soundManager;
    AudioSource audioSource;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        paths = new List<Transform>();
        foreach (Transform location in monsterPath.GetComponentsInChildren<Transform>())
        {
            paths.Add(location);
        }
    }

    public void StartMonsterMove()
    {
        StartCoroutine(MoveToPaths());
    }

    IEnumerator MoveToPaths()
    {
        yield return new WaitForSeconds(startSoundTimeAfterDoorClose);
        audioSource.clip = footstepSFX;
        audioSource.Play();
        yield return new WaitForSeconds(startBackgroundTimeAfterFootstep);
        soundManager.PlayBackgroundSound(backgroundSFX);

        int count = 0;
        Transform currentGoal = paths[count];
        while (currentGoal.position != paths[paths.Count-1].position)
        {
            Vector3.MoveTowards(transform.position, currentGoal.position, Time.deltaTime * moveSpeed);
            if(transform.position == currentGoal.position)
            {
                currentGoal.position = paths[count++].position;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        basementDoor.Unlock(true); // unlock door
        audioSource.Stop();
        soundManager.StopBackgroundSound();
        Destroy(gameObject);
    }
}

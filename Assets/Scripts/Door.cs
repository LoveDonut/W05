using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Sounds
    protected SoundManager soundManager;
    [SerializeField] AudioClip doorSFX;

    protected bool isOpen = false;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public virtual void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        soundManager.PlaySoundOnce(doorSFX, transform.position);
        isOpen = !isOpen;
    }

    protected virtual void OpenDoor()
    {
        Debug.Log("열림");
        // 문을 여는 애니메이션이나 로직을 구현
        transform.Rotate(Vector3.up, 90f); // 예시로 90도 회전
    }

    protected virtual void CloseDoor()
    {
        Debug.Log("닫힘");
        // 문을 닫는 애니메이션이나 로직을 구현
        transform.Rotate(Vector3.up, -90f); // 예시로 -90도 회전
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : Door
{
    [SerializeField] AudioClip lockedSFX;
    [SerializeField] AudioClip eventSFX;

    SoundManager soundManager;
    public enum EDoorType 
    {
        KeyDoor,
        CutterDoor,
        BasementDoor,
        ExitDoor
    }

    [SerializeField] EDoorType doorType;
    bool isLock;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Start()
    {
        if(doorType == EDoorType.BasementDoor)
        {
            isLock = false;
        }
        else
        {
            isLock = true;
        }
    }

    public override void ToggleDoor()
    {
        if(isLock)
        {
            AudioSource.PlayClipAtPoint(lockedSFX, transform.position);
            return;
        }

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        isOpen = !isOpen;
    }

    protected override void CloseDoor()
    {
        if (doorType != EDoorType.ExitDoor) 
        {
            base.CloseDoor();
        }
        else
        {
            //Exit door is closed differently from other types of door

        }
    }

    protected override void OpenDoor()
    {
        if (doorType != EDoorType.ExitDoor)
        {
            base.OpenDoor();
        }
        else
        {
            //Exit door is Opened differently from other types of door

        }
    }

    public void Unlock(bool isInBasement)
    {
        isLock = false;
        OpenDoor();
        if (!isInBasement)
        {
            AudioSource.PlayClipAtPoint(eventSFX, transform.position);
            Invoke("PlayMonsterSound", eventSFX.length * 1.5f);
        }
    }

    public void Lock()
    {
        CloseDoor();
        isLock = true;
    }

    void PlayMonsterSound()
    {
        soundManager.PlayMonsterSoundWhenOpenDoor();
    }

    public EDoorType GetDoorType()
    {
        return doorType;
    }
}

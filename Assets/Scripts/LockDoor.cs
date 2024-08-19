using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : Door
{
    [SerializeField] AudioClip lockedSFX;
    [SerializeField] AudioClip eventSFX;
    public enum EDoorType 
    {
        KeyDoor,
        CutterDoor,
        BasementDoor,
        ExitDoor,
        DefalutDoor
    }

    [SerializeField] protected EDoorType doorType;
    protected bool isLock;
    bool isInBasement;

    void Start()
    {
        isLock = true;
    }

    public override void ToggleDoor()
    {
        if(isLock)
        {
            AudioSource.PlayClipAtPoint(lockedSFX, transform.position);
            return;
        }

        if (!isRotating)
        {
            if (isOpen)
            {
                StartCoroutine(CloseDoor());
            }
            else
            {
                if (transform.CompareTag("IronDoor"))
                {
                    StartCoroutine(OldDoorFall());
                }
                else
                {
                    StartCoroutine(OpenDoor());
                }
            }
            isOpen = !isOpen;
        }
    }

    protected override IEnumerator CloseDoor()
    {
        if (doorType != EDoorType.ExitDoor) 
        {
            //base.CloseDoor();
            yield return StartCoroutine(base.CloseDoor());
        }
        else
        {
            //Exit door is closed differently from other types of door
        }
    }

    protected override IEnumerator OpenDoor()
    {
        if (doorType != EDoorType.ExitDoor)
        {
            if (!isInBasement)
            {
                AudioSource.PlayClipAtPoint(eventSFX, transform.position);
            }

            yield return StartCoroutine (base.OpenDoor());
        }
        else
        {
            //Exit door is Opened differently from other types of door
            Debug.Log("Game Clear ~!");
        }
    }

    public void Unlock(bool isInBasement)
    {
        this.isInBasement = isInBasement;
        isLock = false;
        OpenDoor();
        if (doorType == EDoorType.CutterDoor)
        {
            Transform chain = transform.Find("Chain");
            if (chain != null)
            {
                chain.gameObject.SetActive(false);
            }
        }
        else if(doorType == EDoorType.KeyDoor)
        {
            Transform locker = transform.Find("Locker");
            if (locker != null)
            {
                locker.gameObject.SetActive(false);
            }
        }
    }

    public void Lock()
    {
        StartCoroutine(CloseDoor());
        isLock = true;
    }

    /*
    void PlayEventSound()
    {
        soundManager.PlayMonsterSound();
    }
    */

    public EDoorType GetDoorType()
    {
        return doorType;
    }

    private IEnumerator OldDoorFall()
    {
        Debug.Log("Old Iron Door is falling.");

        if (!isInBasement)
        {
            AudioSource.PlayClipAtPoint(eventSFX, transform.position);
        }

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + Vector3.left * 90f);

        // collision avoidance
        gameObject.GetComponent<BoxCollider>().enabled = false;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 12f);
            yield return null; // wait until next frame
        }

        gameObject.GetComponent<BoxCollider>().enabled = true;

        // error correction
        transform.rotation = targetRotation;
    }
}

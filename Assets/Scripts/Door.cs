using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Sounds
    protected SoundManager soundManager;
    [SerializeField] protected AudioClip doorOpenSFX;
    [SerializeField] protected AudioClip doorCloseSFX;

    protected bool isOpen = false;
    protected bool isRotating = false;

    protected float rotationSpeed = 4f;
    protected float rotationAngle = 90f;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public virtual void ToggleDoor()
    {
        if (!isRotating)
        {
            if (isOpen)
            {
                StartCoroutine(CloseDoor());
            }
            else
            {
                StartCoroutine(OpenDoor());
            }
            isOpen = !isOpen;
        }
        
    }

    protected virtual IEnumerator OpenDoor()
    {
        isRotating = true;

        Debug.Log("The Door Opened");

        soundManager.PlaySoundOnce(doorOpenSFX, transform.position, 0.5f);

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + Vector3.up * rotationAngle);

        // collision avoidance
        gameObject.GetComponent<BoxCollider>().enabled = false;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; // wait until next frame
        }

        gameObject.GetComponent<BoxCollider>().enabled = true;

        // error correction
        transform.rotation = targetRotation;

        isRotating = false;
    }

    protected virtual IEnumerator CloseDoor()
    {
        isRotating = true;

        Debug.Log("The Door Closed");

        soundManager.PlaySoundOnce(doorCloseSFX, transform.position, 0.5f);

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles - Vector3.up * rotationAngle);

        // collision avoidance
        gameObject.GetComponent<BoxCollider>().enabled = false;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null; // wait until next frame
        }
        
        gameObject.GetComponent<BoxCollider>().enabled = true;

        // error correction
        transform.rotation = targetRotation;

        isRotating = false;
    }
}

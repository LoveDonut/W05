using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LockDoor;

public class TwoDoor : LockDoor
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    private void Start()
    {
        if(doorType == EDoorType.ExitDoor)
        {
            isLock = true;
        }
        else
        {
            isLock = false;
        }

        leftDoor = transform.Find("LeftDoor").gameObject;
        rightDoor = transform.Find("RightDoor").gameObject;
    }

    public override void ToggleDoor()
    {
        Debug.Log("양문 접근");
        if (isLock)
        {
            Debug.Log("The Door is Locked");
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
                StartCoroutine(OpenDoor());
            }
            isOpen = !isOpen;
        }
    }

    protected override IEnumerator CloseDoor()
    {
        if (doorType == EDoorType.ExitDoor)
        {
            // Exit Door
            yield return StartCoroutine(base.CloseDoor());
        }
        else        // defalut door
        {
            isRotating = true;

            Debug.Log("The Door Closed");

            soundManager.PlaySoundOnce(doorCloseSFX, transform.position);

            Quaternion leftTargetRotation = Quaternion.Euler(leftDoor.transform.eulerAngles - Vector3.up * rotationAngle);
            Quaternion rightTargetRotation = Quaternion.Euler(rightDoor.transform.eulerAngles + Vector3.up * rotationAngle);

            // collision avoidance
            leftDoor.GetComponent<BoxCollider>().enabled = false;
            rightDoor.GetComponent<BoxCollider>().enabled = false;

            while (Quaternion.Angle(leftDoor.transform.rotation, leftTargetRotation) > 0.1f)
            {
                leftDoor.transform.rotation = Quaternion.Lerp(leftDoor.transform.rotation, leftTargetRotation, Time.deltaTime * rotationSpeed);
                rightDoor.transform.rotation = Quaternion.Lerp(rightDoor.transform.rotation, rightTargetRotation, Time.deltaTime * rotationSpeed);
                yield return null; // wait until next frame
            }

            leftDoor.GetComponent<BoxCollider>().enabled = true;
            rightDoor.GetComponent<BoxCollider>().enabled= true;

            // error correction
            leftDoor.transform.rotation = leftTargetRotation;
            rightDoor.transform.rotation= rightTargetRotation;

            isRotating = false;

        }
    }

    protected override IEnumerator OpenDoor()
    {
        if (doorType == EDoorType.ExitDoor)
        {
            //base.OpenDoor();
            yield return StartCoroutine(base.OpenDoor());
        }
        else        // default door
        {
            isRotating = true;

            Debug.Log("The Door Opened");

            soundManager.PlaySoundOnce(doorOpenSFX, transform.position);

            Quaternion leftTargetRotation = Quaternion.Euler(leftDoor.transform.eulerAngles + Vector3.up * rotationAngle);
            Quaternion rightTargetRotation = Quaternion.Euler(rightDoor.transform.eulerAngles - Vector3.up * rotationAngle);

            // collision avoidance
            leftDoor.GetComponent<BoxCollider>().enabled = false;
            rightDoor.GetComponent<BoxCollider>().enabled = false;

            while (Quaternion.Angle(leftDoor.transform.rotation, leftTargetRotation) > 0.1f)
            {
                leftDoor.transform.rotation = Quaternion.Lerp(leftDoor.transform.rotation, leftTargetRotation, Time.deltaTime * rotationSpeed);
                rightDoor.transform.rotation = Quaternion.Lerp(rightDoor.transform.rotation, rightTargetRotation, Time.deltaTime * rotationSpeed);
                yield return null; // wait until next frame
            }
            Debug.Log("반복문 끝");

            leftDoor.GetComponent<BoxCollider>().enabled = true;
            rightDoor.GetComponent<BoxCollider>().enabled = true;

            // error correction
            leftDoor.transform.rotation = leftTargetRotation;
            rightDoor.transform.rotation = rightTargetRotation;

            isRotating = false;
        }
    }


}

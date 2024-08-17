using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Key : Item
{
    PlayerController playerController;

    public override void Use()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("No PlayerController...");
            return;
        }
        Ray ray = playerController.ray;
        RaycastHit hit;

        Debug.Log("Use Key!");
        if (Physics.Raycast(ray, out hit, playerController.interactionDistance, LayerMask.GetMask("LockDoor")))
        {
            LockDoor lockDoor = hit.transform.GetComponent<LockDoor>();
            if (lockDoor != null)
            {
                Debug.Log("잠긴 문 존재");

                if ((lockDoor.GetDoorType() == LockDoor.EDoorType.KeyDoor))
                {
                    Debug.Log("잠긴 문 해제");
                    lockDoor.Unlock();
                }
            }
        }
    }
}

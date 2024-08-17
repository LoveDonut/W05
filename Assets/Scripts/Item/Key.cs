using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Key : Item
{
    PlayerController playerController;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public override void Use()
    {
        if(playerController == null)
        {
            return;
        }
        Ray ray = playerController.ray;
        RaycastHit hit;

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

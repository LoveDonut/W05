using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : Item
{
    PlayerController playerController;
    public override bool Use()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            return false;
        }
        Ray ray = playerController.ray;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playerController.interactionDistance, LayerMask.GetMask("LockDoor")))
        {
            LockDoor lockDoor = hit.transform.GetComponent<LockDoor>();
            if (lockDoor != null)
            {

                if ((lockDoor.GetDoorType() == LockDoor.EDoorType.ExitDoor))
                {
                    lockDoor.Unlock(false);
                    return true;
                }
            }
        }

        return false;
    }
}

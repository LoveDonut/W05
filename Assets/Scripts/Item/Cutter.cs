using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : Item
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

                if ((lockDoor.GetDoorType() == LockDoor.EDoorType.CutterDoor))
                {
                    Debug.Log("잠긴 문 해제");
                    lockDoor.Unlock(false);
                    return true;
                }
            }
        }

        return false;
    }


}

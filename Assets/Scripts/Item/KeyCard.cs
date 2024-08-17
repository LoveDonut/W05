using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : Item
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
                Debug.Log("��� �� ����");

                if ((lockDoor.GetDoorType() == LockDoor.EDoorType.ExitDoor))
                {
                    Debug.Log("��� �� ����");
                    lockDoor.Unlock();
                }
            }
        }
    }
}

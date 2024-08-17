using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : Door
{
    public enum EDoorType 
    {
        KeyDoor,
        CutterDoor
    }

    [SerializeField] EDoorType doorType;
    bool isLock;

    void Start()
    {
        isLock = true;
    }

    public override void ToggleDoor()
    {
        if(isLock)
        {
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

    public void Unlock()
    {
        isLock = false;
    }

    public EDoorType GetDoorType()
    {
        return doorType;
    }
}

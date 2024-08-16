using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : Door
{
    public enum EDoorType 
    {
        KeyDoor,
        CutterDoor,
        ExitDoor
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

    public void Unlock()
    {
        isLock = false;
    }

    public EDoorType GetDoorType()
    {
        return doorType;
    }
}

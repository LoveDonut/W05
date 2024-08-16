using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected bool isOpen = false;

    public virtual void ToggleDoor()
    {
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

    protected void OpenDoor()
    {
        Debug.Log("열림");
        // 문을 여는 애니메이션이나 로직을 구현
        transform.Rotate(Vector3.up, 90f); // 예시로 90도 회전
    }

    protected void CloseDoor()
    {
        Debug.Log("닫힘");
        // 문을 닫는 애니메이션이나 로직을 구현
        transform.Rotate(Vector3.up, -90f); // 예시로 -90도 회전
    }
}

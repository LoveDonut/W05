using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;

    public void ToggleDoor()
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

    private void OpenDoor()
    {
        Debug.Log("열림");
        // 문을 여는 애니메이션이나 로직을 구현
        transform.Rotate(Vector3.up, 90f); // 예시로 90도 회전
    }

    private void CloseDoor()
    {
        Debug.Log("닫힘");
        // 문을 닫는 애니메이션이나 로직을 구현
        transform.Rotate(Vector3.up, -90f); // 예시로 -90도 회전
    }
}

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
        Debug.Log("����");
        // ���� ���� �ִϸ��̼��̳� ������ ����
        transform.Rotate(Vector3.up, 90f); // ���÷� 90�� ȸ��
    }

    protected void CloseDoor()
    {
        Debug.Log("����");
        // ���� �ݴ� �ִϸ��̼��̳� ������ ����
        transform.Rotate(Vector3.up, -90f); // ���÷� -90�� ȸ��
    }
}

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
        Debug.Log("����");
        // ���� ���� �ִϸ��̼��̳� ������ ����
        transform.Rotate(Vector3.up, 90f); // ���÷� 90�� ȸ��
    }

    private void CloseDoor()
    {
        Debug.Log("����");
        // ���� �ݴ� �ִϸ��̼��̳� ������ ����
        transform.Rotate(Vector3.up, -90f); // ���÷� -90�� ȸ��
    }
}

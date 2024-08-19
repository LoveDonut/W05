using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Sounds
    protected SoundManager soundManager;
    [SerializeField] AudioClip doorSFX;

    protected bool isOpen = false;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

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
        soundManager.PlaySoundOnce(doorSFX, transform.position);
        isOpen = !isOpen;
    }

    protected virtual void OpenDoor()
    {
        Debug.Log("����");
        // ���� ���� �ִϸ��̼��̳� ������ ����
        transform.Rotate(Vector3.up, 90f); // ���÷� 90�� ȸ��
    }

    protected virtual void CloseDoor()
    {
        Debug.Log("����");
        // ���� �ݴ� �ִϸ��̼��̳� ������ ����
        transform.Rotate(Vector3.up, -90f); // ���÷� -90�� ȸ��
    }
}

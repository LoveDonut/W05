using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnterBasement : MonoBehaviour
{
    [SerializeField] LockDoor basementDoor;

    [SerializeField] AudioClip doorCloseSFX;

    EnterBasement enterBasement;

    private void Awake()
    {
        enterBasement = GetComponentInParent<EnterBasement>();

    }

    private void OnTriggerEnter(Collider other)
    {
        basementDoor.Lock(); // close door
        AudioSource.PlayClipAtPoint(doorCloseSFX, basementDoor.transform.position);

        if (enterBasement != null) 
        {
            enterBasement.StartMonsterMove();
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnButton1(InputValue value)
    {
        transform.position = new Vector3(30, -4.5f, 0);
    }

    void OnButton2(InputValue value)
    {
        transform.position = new Vector3(30, -4.5f, 20);
    }

    void OnButton3(InputValue value)
    {
        transform.position = new Vector3(30, -4.5f, 40);
    }

    void OnButton4(InputValue value)
    {
        transform.position = new Vector3(30, -4.5f, 60);
    }
}

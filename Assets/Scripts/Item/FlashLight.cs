using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : Item
{
    [SerializeField] GameObject spotLight;

    public override bool Use()
    {
        spotLight.SetActive(!spotLight.activeInHierarchy);

        Debug.Log("flash turn on/off!");

        return false;
    }

}

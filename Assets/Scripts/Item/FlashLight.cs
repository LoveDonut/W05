using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : Item
{
    [SerializeField] GameObject spotLight;

    public override void Use()
    {
        spotLight.SetActive(!spotLight.activeInHierarchy);

        Debug.Log("flash turn on/off!");
    }

}

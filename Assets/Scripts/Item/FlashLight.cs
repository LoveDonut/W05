using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : Item
{
    Light spotLight;

    public override bool Use()
    {
        spotLight = GameObject.Find("Main Camera").GetComponentInChildren<Light>();
        if (spotLight != null)
        {
            spotLight.enabled = !spotLight.enabled;
        }

        return false;
    }

    public void TurnOff()
    {
        spotLight = GameObject.Find("Main Camera").GetComponentInChildren<Light>();
        if (spotLight != null)
        {
            spotLight.enabled = false;
        }
    }
}

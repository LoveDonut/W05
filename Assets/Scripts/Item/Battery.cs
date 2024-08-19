using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Item
{
    Status status;
    void Awake()
    {
        status = FindObjectOfType<Status>();
    }

    public override bool Use()
    {
        status.IncreaseLight();
        return true;
    }

}

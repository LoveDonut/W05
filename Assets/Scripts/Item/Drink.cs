using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : Item
{
    Status status;

    void Awake()
    {
        status = FindObjectOfType<Status>();
    }

    public override bool Use()
    {
        status.IncreaseSight();
        return true;
    }

}

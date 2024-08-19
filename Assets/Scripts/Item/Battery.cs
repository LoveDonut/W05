using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Item
{
    Status status;
    Inventory inventory;

    void Awake()
    {
        status = FindObjectOfType<Status>();
        inventory = FindObjectOfType<Inventory>();
    }

    public override bool Use()
    {
        // Check if the inventory has a flashlight
        if (!HasFlashlight())
        {
            Debug.Log("You can't use the battery without a flashlight.");
            return false; // Prevent the battery from being used
        }

        // If the inventory has a flashlight, use the battery
        status.IncreaseLight();
        return true;
    }

    private bool HasFlashlight()
    {
        // Check the inventory for an item of type Flashlight
        foreach (GameObject item in inventory.inventory)
        {
            Item inventoryItem = item.GetComponent<Item>();
            if (inventoryItem != null && inventoryItem.GetItemType() == EItemType.Flashlight)
            {
                return true; // Flashlight found
            }
        }
        return false; // No flashlight found
    }

}

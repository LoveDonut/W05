using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public enum EItemType
    {
        None,
        Key,
        Cutter,
        Drink,
        Flashlight,
        KeyCard
    }

    [SerializeField] protected EItemType itemType;
    [SerializeField] protected GameObject ItemOnUI;

    public abstract bool Use();

    // the number of items
    public int count { get; set; }

    public EItemType GetItemType()
    {
        return itemType;
    }

    public GameObject GetItemOnUI()
    {
        return ItemOnUI; 
    }
}

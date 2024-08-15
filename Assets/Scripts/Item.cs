using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum EItemType
    {
        key,
        cutter,
        map,
        drink,
        light,
        keyCard
    }

    [SerializeField] EItemType itemType;

    // the number of items
    public int count { get; set; }

    public EItemType GetItemType()
    {
        return itemType;
    }
}

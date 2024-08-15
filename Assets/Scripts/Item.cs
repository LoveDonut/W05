using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public enum EItemType
    {
        key,
        cutter,
        buildingMap,
        drink,
        flashlight,
        keyCard
    }

    [SerializeField] EItemType itemType;

    public abstract void Use();

    // the number of items
    public int count { get; set; }

    public EItemType GetItemType()
    {
        return itemType;
    }
}

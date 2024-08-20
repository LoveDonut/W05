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
        KeyCard,
        Battery
    }

    [SerializeField] protected EItemType itemType;
    [SerializeField] protected Sprite itemSprite;
    public bool isInFour;
    [Header("Sounds")]
    public AudioClip getSound;
    public AudioClip useSound;

    // the number of items
    public int count { get; set; }

    FourEventTrigger fourEventTrigger;

    public abstract bool Use();

    public void StartEvent()
    {
        fourEventTrigger = FindObjectOfType<FourEventTrigger>();
        fourEventTrigger.TriggerLightEvent();
    }

    public EItemType GetItemType()
    {
        return itemType;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite; 
    }
}

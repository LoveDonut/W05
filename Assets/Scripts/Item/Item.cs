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
    [Header("Sounds")]
    public AudioClip getSound;
    public AudioClip useSound;



    public abstract bool Use();

    public EItemType GetItemType()
    {
        return itemType;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite; 
    }
}

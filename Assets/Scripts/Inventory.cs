using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject itemGrid;
    [SerializeField] Transform itemUIPanel;
    [SerializeField] BackRoomTrigger backroomTrigger;
    public List<GameObject> inventory;
    SoundManager soundManager;

    // default : -1 when not selected 
    int selectedItemIndex = -1;

    Coroutine coroutineInstance;

    public Item SelectedItem {  get; private set; }

    #region PrivateMethods
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        inventory = new List<GameObject>();
        SelectedItem = null;
    }

    //Remove Item when player uses it
    void RemoveItem()
    {
        if (selectedItemIndex == -1) return;

        Item.EItemType itemType = inventory[selectedItemIndex].GetComponent<Item>().GetItemType();

        if (inventory[selectedItemIndex].GetComponent<Item>().count > 1)
        {
            inventory[selectedItemIndex].GetComponent<Item>().count--;
            return;
        }

        inventory.RemoveAt(selectedItemIndex);
        Destroy(itemUIPanel.GetChild(selectedItemIndex).gameObject);
        selectedItemIndex = -1;
        SelectedItem = null;
    }

    void UpdateInventoryUI()
    {
        itemUIPanel.gameObject.SetActive(true);
//        Debug.Log($"inventory cout : {inventory.Count} / selectedItemIndex : {selectedItemIndex}");
        for (int i = 0; i < inventory.Count; i++)
        {
            if (i == selectedItemIndex)
            {
                itemUIPanel.GetChild(i).GetComponent<Image>().enabled = true;
            }
            else
            {
                itemUIPanel.GetChild(i).GetComponent<Image>().enabled = false;
            }
            TextMeshProUGUI text = itemUIPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            if (text != null && text.enabled)
            {
                Debug.Log("Update!");
                text.SetText((inventory[i].GetComponent<Item>().count + 1).ToString());
            }
        }


        if (coroutineInstance != null)
        {
            StopCoroutine(coroutineInstance);
        }
        coroutineInstance = StartCoroutine(TurnOffUI());
    }

    IEnumerator TurnOffUI()
    {
        yield return new WaitForSeconds(1f);
        itemUIPanel.gameObject.SetActive(false);
    }

    #endregion

    public void AddItem(GameObject item)
    {
        Item.EItemType itemType = item.GetComponent<Item>().GetItemType();

        // drink can have more than one
        if (itemType == Item.EItemType.Drink || itemType == Item.EItemType.Battery)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Item.EItemType currentItemType = inventory[i].GetComponent<Item>().GetItemType();
                if (itemType == currentItemType)
                {
                    inventory[i].GetComponent<Item>().count++;
                    UpdateInventoryUI();

                    if (soundManager != null)
                        soundManager.PlaySoundOnce(item.GetComponent<Item>().getSound, item.transform.position);
                    return;
                }
            }
        }

        inventory.Add(item);

        // show item on UI
        GameObject newGrid = Instantiate(itemGrid, itemUIPanel);
        newGrid.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Item>().GetItemSprite();

        if(!(itemType == Item.EItemType.Drink || itemType == Item.EItemType.Battery))
        {
            newGrid.transform.GetChild(1).gameObject.SetActive(false);
        }

//        Debug.Log($"Items Acquired : {item.GetComponent<Item>().GetItemType()}");

        if (itemType == Item.EItemType.Key)
        {
            Key[] keys = FindObjectsOfType<Key>();
            if (keys.Length > 0)
            {
                foreach (Key key in keys)
                {
                    key.gameObject.SetActive(false);
                }
            }
        }
        else if (itemType == Item.EItemType.Flashlight)
        {
            FlashLight[] flashlights = FindObjectsOfType<FlashLight>();
            if (flashlights.Length > 0)
            {
                foreach (FlashLight flashlight in flashlights)
                {
                    flashlight.gameObject.SetActive(false);
                }
            }
        }

        UpdateInventoryUI();

        if(soundManager != null)
            soundManager.PlaySoundOnce(item.GetComponent<Item>().getSound, item.transform.position);

        if (itemType == Item.EItemType.KeyCard)
        {
            backroomTrigger.TriggerLightEvent();
        }
    }

    //player only use item when he selects it
    public void UseItem()
    {
        if(SelectedItem == null)
        {
            return;
        }

        if(SelectedItem.GetItemType() == Item.EItemType.Flashlight)
        {
            if(soundManager != null)
                soundManager.PlaySoundOnce(SelectedItem.useSound, Camera.main.transform.position);
        }

        if (SelectedItem.Use())
        {
            if(soundManager != null)
                soundManager.PlaySoundOnce(SelectedItem.useSound, Camera.main.transform.position, 0.3f);

            RemoveItem();
        }
    }

    public void SelectItem(int index)
    {
        if(index >= inventory.Count || index == -1)
        {
            return;
        }

        // if player change item when hold flashlight, turn it off before change
        if (SelectedItem != null && SelectedItem.GetItemType() == Item.EItemType.Flashlight)
        {
            SelectedItem.GetComponent<FlashLight>().TurnOff();
        }

        selectedItemIndex = index;
        SelectedItem = inventory[index].GetComponent<Item>();

        // update InventoryUI;
        UpdateInventoryUI();

        // Equip Item to player
        Debug.Log($"Equip Item {SelectedItem.GetItemType()}");
    }
}

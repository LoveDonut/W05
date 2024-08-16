using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject itemGrid;
    [SerializeField] Transform itemUIPanel;
    List<GameObject> inventory;

    // default : -1 when not selected 
    int selectedItemIndex = -1;

    #region PrivateMethods
    void Start()
    {
        inventory = new List<GameObject>();

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
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        Debug.Log($"inventory cout : {inventory.Count} / selectedItemIndex : {selectedItemIndex}");
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
        }
    }
    #endregion

    public void AddItem(GameObject item)
    {
        Item.EItemType itemType = item.GetComponent<Item>().GetItemType();

        // drink can have more than one
        if (itemType == Item.EItemType.drink)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                Item.EItemType currentItemType = inventory[i].GetComponent<Item>().GetItemType();
                if (itemType == currentItemType)
                {
                    inventory[i].GetComponent<Item>().count++;
                    return;
                }
            }
        }

        inventory.Add(item);

        // show item on UI
        GameObject newGrid = Instantiate(itemGrid, itemUIPanel);
        Instantiate(item, newGrid.transform.position, Quaternion.identity, newGrid.transform);
    }

    //player only use item when he selects it
    public void UseItem()
    {
        //event occur
        Debug.Log($"Use Item {selectedItemIndex}");

        //remove item after use except flashlight
        if (inventory[selectedItemIndex].GetComponent<Item>().GetItemType() != Item.EItemType.flashlight)
        {
            RemoveItem();
        }
    }

    public void SelectItem(int index)
    {
        if(index >= inventory.Count || index == -1)
        {
            return;
        }
        selectedItemIndex = index;

        // update InventoryUI;
        UpdateInventoryUI();

        // Equip Item to player
        Debug.Log($"Equip Item {index}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inventory : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] List<GameObject> items;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventory.AddItem(items[0]);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.AddItem(items[1]);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            inventory.AddItem(items[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItem(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectItem(2);
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            inventory.UseItem();
        }
    }

    void SelectItem(int index)
    {
        inventory.SelectItem(index);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    
    public InventorySlot[] inventory; //array, fixed size
    public int[] itemCount = new int[5];
    public GameObject inventoryIconPrefab;
    public RectTransform selectionBorder;
    public RectTransform parent;
    [SerializeField] private int maxStack = 9;
    private int slotsInHand = 6;

    int selectedSlot = 0;
    

    void Start()
    {
        SelectSlot(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if(Input.mouseScrollDelta.y > 0)
        {
            if(selectedSlot < slotsInHand - 1)
            {
                SelectSlot(selectedSlot + 1);
            }
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            if(selectedSlot>0)
            {
                SelectSlot(selectedSlot - 1);
            }
        }
    }

    public bool FindEmptySlot(ItemScriptable item)
    {
        //find stackable spot
        if(item.stackable)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                InventorySlot slot = inventory[i];
                ItemIcon itemInSlot = slot.GetComponentInChildren<ItemIcon>();
                ItemScriptable itemScriptable = itemInSlot.itemScriptable;

                //ItemScriptable itemScriptableInSlot = itemInSlot.
                if (itemInSlot != null && itemScriptable == item && itemInSlot.count < maxStack)
                {
                    itemInSlot.updateCount();
                    itemCount[itemScriptable.itemID]++;
                    return true;
                }
            }
        }
        

        //find empty inventory slot
        for (int i = 0; i < inventory.Length; i++)
        {
            InventorySlot slot = inventory[i];
            ItemIcon itemInSlot = slot.GetComponentInChildren<ItemIcon>();
            if(itemInSlot == null)
            {
                AddToSlot(item, slot);
                return true;
            }
        }

        return false;
    }

    private void AddToSlot(ItemScriptable item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryIconPrefab, slot.transform);
        ItemIcon newItemIcon = newItem.GetComponent<ItemIcon>();
        newItemIcon.InitialiseItem(item);
    }

    public void SelectSlot(int newSlot)
    {
        try
        {
            //select new slot
            if (inventory[newSlot].Select())
            {
                //deselect old slot
                if (newSlot != selectedSlot)
                {
                    inventory[selectedSlot].Deselect();
                    selectedSlot = newSlot;
                }
            }
        }
        catch(Exception e)
        {
#if UNITY_EDITOR
            print(e);
#endif
        }
    }
    public void DeselectSlot()
    {
        inventory[selectedSlot].Deselect();
    }
}

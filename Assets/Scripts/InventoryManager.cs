using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    
    public InventorySlot[] inventory; //array, fixed size
    public int[] itemCount = new int[5]; //bu neydi hatirlamiyorum
    public GameObject inventoryIconPrefab;
    public RectTransform selectionBorder;
    public RectTransform parent;
    [SerializeField] private int maxStack = 9;
    private int slotsInHand = 6;

    int selectedSlot = 0;

    [SerializeField] private Player player;
    [SerializeField] ItemScriptable wood;

    void Start()
    {
        SelectSlot(0);
        FindEmptySlot(wood);
        FindEmptySlot(wood);
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardSelect();

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
                    //itemCount[itemScriptable.itemID]++;
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
            inventory[newSlot].Select();

            //deselect old slot
            if (newSlot != selectedSlot)
            {
                inventory[selectedSlot].Deselect();
                selectedSlot = newSlot;
            }

            ItemIcon itemInSlot = inventory[newSlot].GetComponentInChildren<ItemIcon>();
            if(itemInSlot != null)
            {
                int itemID = itemInSlot.itemScriptable.itemID;
                HoldItem(itemID);
            }
            else
            {
                player.Empty();
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

    private void KeyboardSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }
    }

    private void HoldItem(int itemID)
    {
        if(itemID == 0)
        {
            print("unidentified item");
            player.Empty();
        }
        else if(itemID == 1) //sword
        {
            player.WeaponSelect();
        }
        else if(itemID == 2) //pickaxe
        {
            player.PickaxeSelect();
        }
        else if(itemID == 3) //axe
        {
            player.AxeSelect();
        }
        else
        {
            player.Empty();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public InventorySlot[] inventory; //array, fixed size
    public int[] itemCount = new int[5];
    public GameObject inventoryIconPrefab;
    public RectTransform selectionBorder;
    public RectTransform parent;
    [SerializeField] private int maxStack = 9;

    int selectedSlot = 0;
    

    void Start()
    {
        
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
        //deselect old slot
        inventory[selectedSlot].DeselectSlot();
        //select new slot
        inventory[newSlot].SelectSlot();
        //add if check
        selectedSlot = newSlot;
    }

}

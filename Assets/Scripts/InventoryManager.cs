using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public InventorySlot[] inventory; //array, fixed size
    public int[] itemCount = new int[5];
    public GameObject inventoryIconPrefab;

    [SerializeField] private int maxStack = 9;

    public bool fork, pencil, notebook = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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

}

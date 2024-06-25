using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        print("drop");
        if (transform.childCount == 0)
        {
            ItemIcon draggedItem = eventData.pointerDrag.GetComponent<ItemIcon>();
            draggedItem.parentAfterDrag = transform;
        }
        else
        {
            //SwitchPlaces
            ItemIcon draggedItem = eventData.pointerDrag.GetComponent<ItemIcon>();
            //get current child into inventoryItem's position
            Transform oldParent = eventData.pointerDrag.GetComponent<ItemIcon>().parentAfterDrag;
            transform.GetChild(0).transform.SetParent(oldParent);
            draggedItem.parentAfterDrag = transform; //yes
        }
    }

    public void DeselectSlot()
    {
        ItemIcon itemInSlot = GetComponentInChildren<ItemIcon>();
        if (itemInSlot!= null)
        {
            itemInSlot.Deselect();
        }
    }

    public bool SelectSlot()
    {
        ItemIcon itemInSlot = GetComponentInChildren<ItemIcon>();
        if (itemInSlot != null)
        {
            itemInSlot.Select();
            return true;
        }
        return false;
    }

}

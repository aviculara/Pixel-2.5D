using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Transform gridGroup;
    [SerializeField] GameObject selectionBorder;

    public void OnDrop(PointerEventData eventData)
    {
        print("drop");
        if (gridGroup.childCount == 0)
        {
            ItemIcon draggedItem = eventData.pointerDrag.GetComponent<ItemIcon>();
            draggedItem.parentAfterDrag = gridGroup;
        }
        else
        {
            //SwitchPlaces
            ItemIcon draggedItem = eventData.pointerDrag.GetComponent<ItemIcon>();
            //get current child into inventoryItem's position
            Transform oldParent = eventData.pointerDrag.GetComponent<ItemIcon>().parentAfterDrag;
            gridGroup.GetChild(0).transform.SetParent(oldParent);
            draggedItem.parentAfterDrag = gridGroup; 
        }
    }

    public void Deselect()
    {
        selectionBorder.SetActive(false);
    }

    public bool Select()
    {     
        selectionBorder.SetActive(true);
        return true;
    }

}

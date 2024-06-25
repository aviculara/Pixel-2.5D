using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemIcon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ItemScriptable itemScriptable;
    //public int slotNo;
    public int count = 1;

    [SerializeField] private Canvas canvas;
    [HideInInspector] public Transform parentAfterDrag;

    //private RectTransform rectTransform;
    public Image image;
    //public Image smallerImage;
    public TextMeshProUGUI countText;
    public GameObject selectionBorder;

    public void InitialiseItem(ItemScriptable item)
    {
        itemScriptable = item;
        image.sprite = item.image;
        //smallerImage.sprite = item.image;
        if (count <= 1)
        {
            countText.gameObject.SetActive(false);
        }
        else
        {
            countText.gameObject.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        image.raycastTarget = false;
        //smallerImage.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root); //highest level parent
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        image.raycastTarget = true;
        //smallerImage.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        //rectTransform.anchoredPosition = prevPosition;
    }

    // Start is called before the first frame update

    private void Awake()
    {
        //image = GetComponent<Image>();
        //smallerImage = transform.GetChild(0).GetComponent<Image>();
        Deselect();
    }

    private void Start()
    {
        InitialiseItem(itemScriptable);
    }

    public void updateCount()
    {
        //update text
        count++;
        if(count > 1)
        {
            countText.gameObject.SetActive(true);
        }
        countText.text = count.ToString();
    }

    public void Deselect()
    {
        selectionBorder.SetActive(false);
    }

    public void Select()
    {
        selectionBorder.SetActive(true);
    }

}

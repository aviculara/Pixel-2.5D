using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainInventory;
    bool inMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        mainInventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryButton()
    {
        if(inMenu)
        {
            mainInventory.SetActive(false);
            inMenu = false;
        }
        else
        {
            mainInventory.SetActive(true);
            inMenu = true;
        }
    }

}

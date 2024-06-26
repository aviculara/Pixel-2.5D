using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] bool weaponSelected = false;
    [SerializeField] bool pickaxeSelected = false;
    [SerializeField] bool axeSelected = false;

    [SerializeField] GameObject attackIcon;
    [SerializeField] GameObject pickaxeIcon;
    [SerializeField] GameObject axeIcon;
    [SerializeField] GameObject defendIcon;
    [SerializeField] GameObject specialIcon;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeaponSelect()
    {
        weaponSelected = true;
        pickaxeSelected = false;
        axeSelected = false;
        //operations in other scripts
        SetIcons();
    }

    public void PickaxeSelect()
    {
        pickaxeSelected = true;
        weaponSelected = false;
        axeSelected = false;
        SetIcons();
    }

    public void AxeSelect()
    {
        axeSelected = true;
        pickaxeSelected = false;
        weaponSelected = false;
        SetIcons();
    }

    public void Empty()
    {
        axeSelected = false;
        pickaxeSelected = false;
        weaponSelected = false;
        SetIcons();
    }

    public void SetIcons()
    {
        attackIcon.SetActive(weaponSelected);
        defendIcon.SetActive(weaponSelected);
        specialIcon.SetActive(weaponSelected);

        axeIcon.SetActive(axeSelected);

        pickaxeIcon.SetActive(pickaxeSelected);
    }
}

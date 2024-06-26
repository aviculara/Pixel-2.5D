using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Class playerClass;

    private Move moveScript;
    private WarriorClass warriorScript;
    private RangerClass rangerScript;
    private Animator animator;

    [Header("Mining")]
    private float mineCooldownLeft = 0;
    [SerializeField] Image mineIcon;
    float mineCooldown = 0.45f;

    public enum Class
    {
        Warrior,
        Ranger
    }

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<Move>();
        if(playerClass == Class.Warrior)
        {
            warriorScript = GetComponent<WarriorClass>();
        }
        else if(playerClass == Class.Ranger)
        {
            rangerScript = GetComponent<RangerClass>();
        }
        animator = moveScript.animator;

        mineIcon.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mineCooldownLeft > 0)
        {
            mineCooldownLeft -= Time.unscaledDeltaTime;
        }
        mineIcon.fillAmount = mineCooldownLeft / mineCooldown;

        if(pickaxeSelected)
        {
            if (InputAttack() && mineCooldownLeft <= 0)
            {
                animator.SetTrigger("Attack");
                mineCooldownLeft = mineCooldown;
            }
        }
        //same for axe
    }

    #region Selection
    public void WeaponSelect()
    {
        weaponSelected = true;
        pickaxeSelected = false;
        axeSelected = false;
        //operations in other scripts
        warriorScript.canAttack = weaponSelected; //add other classes later
        SetIcons();
    }

    public void PickaxeSelect()
    {
        pickaxeSelected = true;
        weaponSelected = false;
        axeSelected = false;
        warriorScript.canAttack = weaponSelected;
        SetIcons();
    }

    public void AxeSelect()
    {
        axeSelected = true;
        pickaxeSelected = false;
        weaponSelected = false;
        warriorScript.canAttack = weaponSelected;
        SetIcons();
    }

    public void Empty()
    {
        axeSelected = false;
        pickaxeSelected = false;
        weaponSelected = false;
        warriorScript.canAttack = weaponSelected;
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
    #endregion

    private bool InputAttack()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

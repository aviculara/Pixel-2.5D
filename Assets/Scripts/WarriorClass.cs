using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorClass : MonoBehaviour
{
    private Move moveScr;
    private Animator animator; //assigned in script
    public bool inputOn;
    public Collider2D enemyCollider;

    [Header("Attack")]
    private Sword sword;
    public Sword[] swords; //dir = index
    public Image swordIcon;
    private float swordCooldownLeft=0;

    [Header("Shield")]
    public Image shieldIcon;
    private bool shieldActive;
    
    [Header("Special Attack")]
    public Image specialIcon;
    private float specialCooldownLeft = 0;
    public GameObject rightArrow,downArrow,leftArrow,upArrow;
    [SerializeField] float yOffset, xOffset;

    [Header("Editor")]
    public int swordDamage = 20;
    public float shieldMultiplier = 0.2f;
    public int specialDamage = 40;
    public float specialCooldown = 5.4f;
    public float atkCooldown = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        moveScr = GetComponent<Move>();
        moveScr.playerClass = Move.Class.Warrior;
        animator = moveScr.animator;
        //sword = transform.GetChild(0).GetComponent<Sword>();

        shieldIcon.fillAmount = 0;
        specialIcon.fillAmount = 0;
        swordIcon.fillAmount = 0;

        rightArrow.SetActive(false);
        downArrow.SetActive(false);
        leftArrow.SetActive(false);
        upArrow.SetActive(false);

        inputOn = true; //temp
    }

    // Update is called once per frame
    void Update()
    {
        if(swordCooldownLeft>0)
        {
            swordCooldownLeft -= Time.unscaledDeltaTime ;
        }
        swordIcon.fillAmount = swordCooldownLeft / atkCooldown;
        if(specialCooldownLeft>0)
        {
            specialCooldownLeft -= Time.unscaledDeltaTime;
        }
        specialIcon.fillAmount = specialCooldownLeft/ specialCooldown;
        
        if(inputOn)
        {
            if(InputSecondary())
            {
                if(!shieldActive)
                {
                    animator.SetBool("Shield", true);
                    shieldActive = true;
                    shieldIcon.fillAmount = 1;
                }
                
            }
            else if(InputAttack() && swordCooldownLeft <= 0)
            {
                Attack();
            }
            else if(InputSpecial() && specialCooldownLeft <=0)
            {
                SpecialAttack();
            }
            if(!InputSecondary() && shieldActive)
            {
                shieldActive = false;
                animator.SetBool("Shield", false);
                shieldIcon.fillAmount = 0;
            }
        }
    }

    #region Inputs
    private bool InputAttack()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        else return false;
    }

    private bool InputSecondary()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            return true;
        }
        else return false;
    }
    private bool InputSpecial()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        else return false;
    }
    #endregion

    private void Attack()
    {
        swordCooldownLeft = atkCooldown;
        //sword in the right direction = swords[move.direction]
        Sword currentSword = swords[moveScr.direction];
        currentSword.DamageEnemies(swordDamage);
        //if in range, enemy game object.damage
        animator.SetTrigger("Attack");
    }

    private void SpecialAttack()
    {
        specialCooldownLeft = specialCooldown;
        animator.SetTrigger("Attack");
        int direction = moveScr.direction;
        GameObject newArrow = null;
        //Instantiate blue arrow
        switch (direction)
        {
            case 0: //right
                newArrow = Instantiate(rightArrow, new Vector3(transform.position.x + 0.4f,
                transform.position.y + 0.0f, transform.position.z), Quaternion.identity);
                break;
            case 1: //down
                newArrow = Instantiate(downArrow, new Vector3(transform.position.x + 0.05f,
                transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
                break;
            case 2: //left
                newArrow = Instantiate(leftArrow, new Vector3(transform.position.x - 0.4f,
                transform.position.y + 0.0f, transform.position.z), Quaternion.identity);
                break;
            case 3: //up
                newArrow = Instantiate(upArrow, new Vector3(transform.position.x + 0.1f,
                transform.position.y + 0.7f, transform.position.z), Quaternion.identity);
                break;
        }
        if(newArrow != null)
        {
            newArrow.GetComponent<Arrow>().SetDirection(direction);
            newArrow.SetActive(true);
            Destroy(newArrow, 5f);
        }
        else
        {
#if UNITY_EDITOR
            print("invalid direction");
#endif
        }
    }

    #region old warrior
    IEnumerator oldAttack()
    {
        //cooldownAtk = true;
        swordCooldownLeft = atkCooldown;
        moveScr.animator.SetTrigger("Attack");
        if (sword.inRange)
        {
            //enemyCollider.GetComponent<PlayerMove>().takeDamage(swordDamage);
            //find better efficiency in sword
            //enemyCollider.gameObject.GetComponent<PlayerMove>().hp -= swordDamage;
            //enemyCollider.gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 87f, 87f, 255f);
            print("damage enemy");
        }
        yield return new WaitForSeconds(atkCooldown);
        //cooldownAtk = false;
        swordCooldownLeft = 0;
    }

    IEnumerator oldSpecialAttack()
    {
        //cooldownUlt = true;
        specialCooldownLeft = specialCooldown;
        moveScr.animator.SetTrigger("Attack");
        GameObject newArrow = Instantiate(rightArrow,
            new Vector3(transform.position.x + 0.4f //* moveScr.orientation
            , transform.position.y + 0.2f, transform.position.z),
            Quaternion.identity);
        Arrow arrowScript = newArrow.GetComponent<Arrow>();
        //arrowScript.orientation = gameObject.GetComponent<PlayerMove>().orientation;
        arrowScript.arrowDamage = specialDamage;

        //if (player1)
        //{
        //    newArrow.GetComponent<Arrow>().enemyTag = "PlayerTwo";
        //}
        //else if (player2)
        //{
        //    newArrow.GetComponent<Arrow>().enemyTag = "PlayerOne";
        //}
        newArrow.SetActive(true);
        yield return new WaitForSeconds(specialCooldown);
        specialCooldownLeft = 0;
        //cooldownUlt = false;
    }
    #endregion


}

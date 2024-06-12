using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorClass : MonoBehaviour
{
    
    public bool player1, player2;
    public GameObject blueArrow;
    public Image swordIcon;
    private float swordSeconds=0;
    public Image shieldIcon;
    
    public Image ultIcon;
    private float ultSeconds = 0;
    //found in script:
    //public PlayerMove enemyMoveScr;
    public PlayerMove moveScr;
    private int jumpforce;
    private bool cooldownAtk=false;
    private bool cooldownUlt=false;
    public Collider2D enemyCollider;

    private Sword sword;
    [Header("Editor")]
    public int swordDamage = 20;
    public float shieldMultiplier = 0.2f;
    public int ultDamage = 40;
    public float cdUlt = 5.4f;
    public float cdAtk = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        moveScr = transform.GetComponent<PlayerMove>();
        moveScr.Class = "Warrior";
        sword = transform.GetChild(0).GetComponent<Sword>();
        if (moveScr.player1)
        {
            player1 = true;
            player2 = false;
        }
        else if(moveScr.player2)
        {
            player1 = false;
            player2 = true;
        }
        jumpforce = moveScr.jumpForce;
        shieldIcon.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(swordSeconds>0)
        {
            swordSeconds -= Time.deltaTime ;
        }
        swordIcon.fillAmount = swordSeconds / cdAtk;
        if(ultSeconds>0)
        {
            ultSeconds -= Time.deltaTime;
        }
        ultIcon.fillAmount = ultSeconds/ cdUlt;
        if (moveScr.inputOn)
        {
            if (InputSecondary())
            {
                moveScr.animator.SetBool("Shield", true);
                shieldIcon.fillAmount = 1;
                moveScr.jumpForce = 0;
                moveScr.damageMultiplier = shieldMultiplier;
            }

            else if (InputAttack() && !cooldownAtk)
            {
                StartCoroutine(Attack());
            }

            else if (moveScr.animator.GetBool("Shield"))
            {
                moveScr.animator.SetBool("Shield", false);
                shieldIcon.fillAmount = 0;
                moveScr.jumpForce = jumpforce;
                moveScr.damageMultiplier = 1f;
            }
            else if (InputSpecial() && !cooldownUlt)
            {
                StartCoroutine(SpecialAttack());
            }
        }
    }

    #region Inputs
    private bool InputAttack()
    {
        if (player1 && Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        else if (player2 && Input.GetKeyDown(KeyCode.RightControl))
        {
            return true;
        }
        else return false;
    }

    private bool InputSecondary()
    {
        if (player1 && Input.GetKey(KeyCode.Alpha3))
        {
            return true;
        }
        else if (player2 && Input.GetKey(KeyCode.RightShift))
        {
            return true;
        }
        else return false;
    }
    private bool InputSpecial()
    {
        if (player1 && Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }
        else if (player2 && Input.GetKeyDown(KeyCode.Slash)) //US klavyeye cevirmek lazim
        {
            return true;
        }
        else return false;
    }
    #endregion

    IEnumerator Attack()
    {
        cooldownAtk = true;
        swordSeconds = cdAtk;
        moveScr.animator.SetTrigger("Attack");
        if (sword.inRange)
        {
            enemyCollider.GetComponent<PlayerMove>().takeDamage(swordDamage);
            //find better efficiency in sword
            //enemyCollider.gameObject.GetComponent<PlayerMove>().hp -= swordDamage;
            //enemyCollider.gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 87f, 87f, 255f);
        }
        yield return new WaitForSeconds(cdAtk);
        cooldownAtk = false;
        swordSeconds = 0;
    }

    IEnumerator SpecialAttack()
    {
        cooldownUlt = true;
        ultSeconds = cdUlt;
        moveScr.animator.SetTrigger("Attack");
        GameObject newArrow = Instantiate(blueArrow,
            new Vector3(transform.position.x + 0.4f * moveScr.orientation
            , transform.position.y + 0.2f, transform.position.z),
            Quaternion.identity);
        Arrow arrowScript = newArrow.GetComponent<Arrow>();
        arrowScript.orientation = gameObject.GetComponent<PlayerMove>().orientation;
        arrowScript.arrowDamage = ultDamage;

        if (player1)
        {
            newArrow.GetComponent<Arrow>().enemyTag = "PlayerTwo";
        }
        else if (player2)
        {
            newArrow.GetComponent<Arrow>().enemyTag = "PlayerOne";
        }
        newArrow.SetActive(true);
        yield return new WaitForSeconds(cdUlt);
        ultSeconds = 0;
        cooldownUlt = false;
    }
    
}

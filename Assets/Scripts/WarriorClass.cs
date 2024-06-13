using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorClass : MonoBehaviour
{
    public GameObject blueArrow;
    public Image swordIcon;
    private float swordSeconds=0;
    public Image shieldIcon;
    
    public Image ultIcon;
    private float ultSeconds = 0;

    public Move moveScr;
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
        moveScr = transform.GetComponent<Move>();
        moveScr.playerClass = Move.Class.Warrior;
        //sword = transform.GetChild(0).GetComponent<Sword>();

        shieldIcon.fillAmount = 0;
        ultIcon.fillAmount = 0;
        swordIcon.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(swordSeconds>0)
        {
            swordSeconds -= Time.unscaledDeltaTime ;
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
                moveScr.defenseModifier = shieldMultiplier;
            }

            else if (InputAttack() && !cooldownAtk)
            {
                StartCoroutine(Attack());
            }

            else if (moveScr.animator.GetBool("Shield"))
            {
                moveScr.animator.SetBool("Shield", false);
                shieldIcon.fillAmount = 0;
                moveScr.defenseModifier = 1f;
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
            new Vector3(transform.position.x + 0.4f //* moveScr.orientation
            , transform.position.y + 0.2f, transform.position.z),
            Quaternion.identity);
        Arrow arrowScript = newArrow.GetComponent<Arrow>();
        arrowScript.orientation = gameObject.GetComponent<PlayerMove>().orientation;
        arrowScript.arrowDamage = ultDamage;

        //if (player1)
        //{
        //    newArrow.GetComponent<Arrow>().enemyTag = "PlayerTwo";
        //}
        //else if (player2)
        //{
        //    newArrow.GetComponent<Arrow>().enemyTag = "PlayerOne";
        //}
        newArrow.SetActive(true);
        yield return new WaitForSeconds(cdUlt);
        ultSeconds = 0;
        cooldownUlt = false;
    }
    
}

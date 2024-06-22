using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangerClass : MonoBehaviour
{
    public bool player1, player2;
    public GameObject sampleArrow;
    public GameObject greenArrow;
    public Image arrowIcon;
    public Image vanishIcon;
    public Image ultArrowIcon;
    private float arrowSeconds=0;
    private float vanishSeconds=0;
    private float ultSeconds=0;

    public Collider2D enemyCollider;
    public PlayerMove moveScr;
    private bool vanishing=false;
    private int OGrunspeed, OGjumpforce;
    private bool cooldown2nd = false;
    private bool cooldownUlt = false;
    private bool cooldownAtk = false;

    [Header("Editor")]
    public int arrowDamage = 10;
    public float vanishMultiplier = 1.5f;
    public int ultDamage = 20;
    public float cd2nd = 0.5f;
    public float cdUlt = 5.4f;
    public float cdAtk = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        moveScr = transform.GetComponent<PlayerMove>();
        moveScr.Class = "Ranger";
        if (moveScr.player1)
        {
            player1 = true;
            player2 = false;
        }
        else if (moveScr.player2)
        {
            player1 = false;
            player2 = true;
        }
        OGjumpforce = moveScr.jumpForce;
        OGrunspeed = moveScr.runSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowSeconds > 0)
        {
            arrowSeconds -= Time.deltaTime;
        }
        arrowIcon.fillAmount = arrowSeconds / cdAtk;
        if (vanishSeconds > 0)
        {
            vanishSeconds -= Time.deltaTime;
        }
        vanishIcon.fillAmount = vanishSeconds / cd2nd;
        if (ultSeconds > 0)
        {
            ultSeconds -= Time.deltaTime;
        }
        ultArrowIcon.fillAmount = ultSeconds / cdUlt;

        if (moveScr.inputOn)
        {
            if (vanishing) { }
            else if (InputAttack() && !cooldownAtk)
            {
                StartCoroutine(Attack());
            }
            else if (InputSecondary() && !cooldown2nd)
            {
                //become invisible and take 0 damage
                StartCoroutine(Vanish());
            }
            else if (InputSpecial() && !cooldownUlt)
            {
                StartCoroutine(GreenArrows());
            }
        }
        //if (moveScr.animator.GetBool("Vanish")) { }
    }
    #region Inputs
    private bool InputAttack()
    {
        if (player1 && Input.GetKeyDown(KeyCode.Space))
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
        if (player1 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            return true;
        }
        else if (player2 && Input.GetKeyDown(KeyCode.RightShift))
        {
            return true;
        }
        else return false;
    }

    private bool InputSpecial()
    {
        if (player1 && Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        else if (player2 && (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Keypad0))) //US klavyeye cevirmek lazim
        {
            return true;
        }
        else return false;
    }
    #endregion

    IEnumerator Attack()
    {
        cooldownAtk = true;
        arrowSeconds = cdAtk;
        moveScr.animator.SetTrigger("Attack");
        GameObject newArrow = Instantiate(sampleArrow,
            new Vector3(transform.position.x - 0.48f * moveScr.orientation
            , transform.position.y - 0.7f, transform.position.z),
            Quaternion.identity);
        Arrow arrowScript = newArrow.GetComponent<Arrow>();
        //arrowScript.orientation = moveScr.orientation;
        arrowScript.arrowDamage = this.arrowDamage;
        if (player1)
        {
            newArrow.GetComponent<Arrow>().enemyTag = "PlayerTwo";
        }
        else if (player2)
        {
            newArrow.GetComponent<Arrow>().enemyTag = "PlayerOne";
        }
        newArrow.SetActive(true);
        yield return new WaitForSeconds(cdAtk);
        cooldownAtk = false;
        arrowSeconds = 0;
    }

    IEnumerator Vanish()
    {
        SpriteRenderer playerSprite = moveScr.gameObject.GetComponent<SpriteRenderer>();
        //moveScr.animator.SetBool("Vanish",true);
        moveScr.animator.SetTrigger("Vanish");
        vanishing = true;
        playerSprite.color = new Color(0.35f, 0.35f, 0.59f, 1f);
        vanishSeconds = cd2nd;
        moveScr.runSpeed = (int)(OGrunspeed * vanishMultiplier);
        //moveScr.jumpForce = 0;
        moveScr.damageMultiplier = 0;
        yield return new WaitForSeconds(1f);
        //moveScr.animator.SetBool("Vanish", false);
        vanishing = false;
        playerSprite.color = new Color(1f, 1f, 1f, 1f);
        moveScr.runSpeed = OGrunspeed;
        //moveScr.jumpForce = OGjumpforce;
        moveScr.damageMultiplier = 1;
        //StartCoroutine(CooldownVanish());
        cooldown2nd = true;
        yield return new WaitForSeconds(cd2nd);
        cooldown2nd = false;
        vanishSeconds = 0;
    }

    IEnumerator CooldownVanish()
    {
        cooldown2nd = true;
        yield return new WaitForSeconds(cd2nd);
        cooldown2nd = false;
    }

    IEnumerator GreenArrows()
    {
        cooldownUlt = true;
        moveScr.animator.SetTrigger("Attack");
        ultSeconds = cdUlt;
        newGreenArrow();
        yield return new WaitForSeconds(0.3f);
        moveScr.animator.SetTrigger("Attack");
        newGreenArrow();
        yield return new WaitForSeconds(0.3f);
        moveScr.animator.SetTrigger("Attack");
        newGreenArrow();
        yield return new WaitForSeconds(cdUlt);
        cooldownUlt = false;
        ultSeconds = 0;
    }

    private void newGreenArrow()
    {
        GameObject newArrow = Instantiate(greenArrow,
            new Vector3(transform.position.x - 0.48f * moveScr.orientation
            , transform.position.y - 0.7f, transform.position.z),
            Quaternion.identity);
        Arrow arrowScript = newArrow.GetComponent<Arrow>();
        //arrowScript.orientation = moveScr.orientation;
        arrowScript.arrowDamage = ultDamage;

        if (player1)
        {
            arrowScript.enemyTag = "PlayerTwo";
        }
        else if (player2)
        {
            arrowScript.enemyTag = "PlayerOne";
        }
        newArrow.SetActive(true);
    }


}


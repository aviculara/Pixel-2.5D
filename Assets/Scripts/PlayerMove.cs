using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Image hpBar;
    public bool player1, player2;
    private bool onGround;
    public bool Warrior;
    public bool Ranger;
    public float damageMultiplier = 1f;
    public int orientation;
    public bool inputOn;
    public string Class;

    private float scaleX, scaleY, scaleZ;
    private ArenaManager gameManager;
    [Header("Editor")]
    public int hp = 100;
    public int runSpeed = 10, jumpForce = 100;
    

    // Start is called before the first frame update
    private void Awake()
    {
        if (this.gameObject.CompareTag("PlayerOne"))
        {
            player1 = true;
            //transform.GetChild(0).GetComponent<NearAttack>().player1 = true;
        }
        else if (this.gameObject.CompareTag("PlayerTwo"))
        {
            player2 = true;
            //transform.GetChild(0).GetComponent<NearAttack>().player2 = true;
        }
    }
    void Start()
    {
        hp = 100;
        /*
        if(Warrior)
        {
            WarriorClass attackScript = GetComponent<WarriorClass>();
        }
        */

        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        if(controller != null)
        {
            gameManager = controller.GetComponent<ArenaManager>();
        }

        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        scaleZ = transform.localScale.z;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(scaleX * orientation, scaleY, scaleZ);
        hpBar.fillAmount = (float)hp / 100f;
        if(inputOn)
        {
            if (InputLeft() || InputRight())
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);

            }
            else if (!InputAttack() && !InputUp())
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", true);
            }
            if (InputUp() && onGround)
            {
                rb.AddForce(Vector2.up * jumpForce);
                animator.SetTrigger("Jump");
            }
            if (InputLeft())
            {
                transform.Translate(Vector3.left * Time.deltaTime * runSpeed);
            }
            if (InputRight())
            {
                transform.Translate(Vector3.right * Time.deltaTime * runSpeed);
            }
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    #region Inputs
    private bool InputRight()
    {
        if (player1 && Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else if (player2 && Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        else return false;
    }

    private bool InputLeft()
    {
        if (player1 && Input.GetKey(KeyCode.A))
        {
            return true;
        }
        else if (player2 && Input.GetKey(KeyCode.LeftArrow))
        {
            return true;
        }
        else return false;
    }

    private bool InputUp()
    {
        if (player1 && Input.GetKeyDown(KeyCode.W))
        {
            return true;
        }
        else if (player2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }
        else return false;
    }

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
    #endregion

    public void takeDamage(int damage)
    {
        if(inputOn)
        {
            int effectiveDamage = (int)((float)damage * damageMultiplier);
            if (effectiveDamage != 0)
            {
                if (effectiveDamage < hp)
                {
                    hp -= effectiveDamage;
                    StartCoroutine(hurtColor());
                }
                else
                {
                    hp = 0;
                    //animator.SetTrigger("Death");
                    //gameManager.turnOffInputs();
                    gameManager.timeOver();
                }
            }
        }
    }

    IEnumerator hurtColor()
    {
        SpriteRenderer playerSprite = gameObject.GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(1f, 0.34f, 0.34f, 1f);
        yield return new WaitForSeconds(0.25f);
        playerSprite.color = new Color(1f, 1f, 1f, 1f);
    }
}

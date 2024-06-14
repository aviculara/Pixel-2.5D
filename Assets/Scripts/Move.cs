using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int runSpeed = 5;
    public Class playerClass;
    public Animator animator;
    public int direction = 0;
    public bool walking = false;
    public bool inputOn;

    public float defenseModifier;

    // Start is called before the first frame update
    void Start()
    {
        inputOn = true; //temp
        animator.SetInteger("Direction", direction);
        animator.SetBool("Walk", walking);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputOn)
        {
            if (Input.GetKey(KeyCode.A)) //run left, direction 2
            {
                transform.Translate(Vector3.left * Time.deltaTime * runSpeed);
                //animator.SetBool("walkLeft", true);
                if(direction !=2)
                {
                    direction = 2;
                    animator.SetInteger("Direction", direction);
                }
                if (!walking)
                {
                    walking = true;
                    animator.SetBool("Walk", walking);
                }
            }
            //else { animator.SetBool("walkLeft", false); }

            if (Input.GetKey(KeyCode.D)) //run right, direction 0
            {
                transform.Translate(Vector3.right * Time.deltaTime * runSpeed);
                //animator.SetBool("walkRight", true);
                if (direction != 0)
                {
                    direction = 0;
                    animator.SetInteger("Direction", direction);
                }
                if (!walking)
                {
                    walking = true;
                    animator.SetBool("Walk", walking);
                }
            }
            //else { animator.SetBool("walkRight", false); }

            if (Input.GetKey(KeyCode.W)) //run up, direction 3
            {
                transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                //animator.SetBool("walkUp", true);
                if (direction != 3)
                {
                    direction = 3;
                    animator.SetInteger("Direction", direction);
                }
                if (!walking)
                {
                    walking = true;
                    animator.SetBool("Walk", walking);
                }
            }
            //else { animator.SetBool("walkUp", false); }

            if (Input.GetKey(KeyCode.S)) //run down, direction 1
            {
                transform.Translate(Vector3.forward * Time.deltaTime * runSpeed * -1);
                //animator.SetBool("walkDown", true);
                if (direction != 1)
                {
                    direction = 1;
                    animator.SetInteger("Direction", direction);
                }
                if(!walking)
                {
                    walking = true;
                    animator.SetBool("Walk", walking);
                }
            }
            //else { animator.SetBool("walkDown", false); }

            if(!(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
            {
                if(walking)
                {
                    walking = false;
                    animator.SetBool("Walk", walking);
                }
            }
        }        
    }

    public enum Class
    {
        Warrior,
        Ranger
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public int runSpeed = 5;
    public Class playerClass;
    public Animator animator;
    public bool inputOn;

    public float defenseModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) //run left
        {
            transform.Translate(Vector3.left * Time.deltaTime * runSpeed);
            animator.SetBool("walkLeft", true);
        }
        else { animator.SetBool("walkLeft", false); }

        if (Input.GetKey(KeyCode.D)) //run right
        {
            transform.Translate(Vector3.right * Time.deltaTime * runSpeed);
            animator.SetBool("walkRight", true);
        }
        else { animator.SetBool("walkRight", false); }

        if(Input.GetKey(KeyCode.W)) //run up
        { 
            transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
            animator.SetBool("walkUp", true);
        }
        else { animator.SetBool("walkUp", false); }

        if (Input.GetKey(KeyCode.S)) //run down
        {
            transform.Translate(Vector3.forward * Time.deltaTime * runSpeed * -1);
            animator.SetBool("walkDown", true);
        }
        else { animator.SetBool("walkDown", false); }
    }

    public enum Class
    {
        Warrior,
        Ranger
    }
}

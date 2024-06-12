using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool player1, player2;
    public bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.CompareTag("PlayerOne"))
        {
            player1 = true;
            player2 = false;
        }
        else if(transform.parent.CompareTag("PlayerTwo"))
        {
            player2 = true;
            player1 = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((player1 && other.gameObject.CompareTag("PlayerTwo")) || (player2 && other.gameObject.CompareTag("PlayerOne")))
        {
            inRange = true;
            transform.parent.GetComponent<WarriorClass>().enemyCollider = other;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((player1 && other.gameObject.CompareTag("PlayerTwo")) || (player2 && other.gameObject.CompareTag("PlayerOne")))
        {
            inRange = false;
        }
    }

}

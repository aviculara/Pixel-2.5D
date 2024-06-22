using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public string enemyTag;
    public int fireSpeed;
    public int arrowDamage;
    public int direction;
    private Vector3 directionVector = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(directionVector * Time.deltaTime * fireSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(enemyTag))
        {
            //other.GetComponent<PlayerMove>().takeDamage(arrowDamage);
            //Destroy(this.gameObject);
        }
        else if(other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        
    }

    private void OnEnable()
    {
        //if(enemyTag == "PlayerTwo") 
        //{
        //    //arrow sprites are made for player two
        //    //flip orientation if arrow is made by player one = enemy is player two
        //    orientation *= -1;
        //}
        //gameObject.transform.localScale = new Vector3(transform.localScale.x * orientation,
        //    transform.localScale.y, transform.localScale.z);
    }

    public void SetDirection(int playerDirection)
    {
        switch (playerDirection)
        {
            case 0:
                //right
                directionVector = Vector3.right;
                break;
            case 1:
                //down
                directionVector = Vector3.forward * (-1);
                break;
            case 2:
                //left
                directionVector = Vector3.left;
                break;
            case 3:
                //up
                directionVector = Vector3.forward;
                break;
        }
    }
}

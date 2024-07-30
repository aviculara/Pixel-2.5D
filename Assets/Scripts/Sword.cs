using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public bool inRange = false;
    public GameObject enemyObject;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    //public string dir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }        
    }

    public void DamageEnemies(int damage)
    {
        foreach(GameObject enemy in enemiesInRange)
        {
            //find enemy script and do damage
            print("did " + damage + " damage to " + enemy.name);
            enemy.GetComponentInChildren<HP>().TakeDamage();
        }
    }
}

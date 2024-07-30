using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Vector3 startingPosition; //of the game object. don't change in code
    private Vector3 roamingPosition;
    private State state = State.Roam;
    public int direction = 0;

    [SerializeField] float speed = 2.5f;
    [SerializeField] float attackRange;
    [SerializeField] float deaggroRange = 50f;
    [SerializeField] Transform player;
    //[SerializeField] bool ranged = true;

    public enum State
    {
        Roam,
        Attack,
        Follow
    }
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            default:
                transform.position = Vector3.MoveTowards(transform.position, roamingPosition, speed * Time.deltaTime);
                float minDistance = 1f;
                if(Vector3.Distance(transform.position, roamingPosition) < minDistance)
                {
                    roamingPosition = GetRoamingPosition();
                }
                FindTarget();
                break;
            case State.Attack:
                //attack

                FindTarget();
                break;
            case State.Follow:
                //follow
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                FindTarget();
                break;
        }
    }

    public Vector3 RandomDirection()
    {
        //characters are on x-z plane
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return randomDir;
    }

    private Vector3 GetRoamingPosition()
    {
        //a random position is created by a random direction and a random distance
        float randomDistance = Random.Range(2f, 10f);
        Vector3 newRoamPosition = startingPosition + NewNearbyDirection() * randomDistance;
        //print("moving " + randomDistance);
        return newRoamPosition;
    }

    private Vector3 NewNearbyDirection()
    {
        //     3
        // 2 - + - 4 (0) 
        //     1
        // pick a random number between -2 and +2, divide by 2, add to current direction
        // .5 values represent diagonal directions
        // mod 4 of the new direction represents its direction value for the animator
        // right : Vector3(1,0,0)   ||  up : Vector3(0,0,1) 
        int newDirection;
        Vector3 directionVector;
        newDirection = Random.Range(-2, 2) + direction * 2;
        newDirection = newDirection % 8;
        directionVector = EightDirectionsToVector(newDirection);
        direction = SetDirectionByEight(newDirection);
        return directionVector;
    }

    private void FindTarget()
    {
        //print(Vector3.Distance(player.position, transform.position));
        switch(state)
        {
            default:
                if(Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = State.Attack;
                    print("I'm attacking.");
                }
                break;
            case State.Attack:
                //add later: once aggro'd go into following. can de-aggro once the player is too far or gone for too long
                //add later: if ranged enemy, can run away when player gets too close
                //if(Vector3.Distance(player.position, transform.position) > attackRange)
                //{
                //    state = State.Follow;
                //    print("back to roaming");
                //}
                //else if(Vector3.Distance(player.position, transform.position) > deaggroRange)
                //{
                //    state = State.Roam;
                //}
                state = State.Roam;
                break;
        }

    }

    public Vector3 EightDirectionsToVector(int directionCode)
    {
        Vector3 directionVector;
        switch(directionCode)
        {
            case 0: //right
                directionVector = new Vector3(1, 0, 0);
                break;
            case 1: //down-right
                directionVector = new Vector3(1, 0, -1);
                break;
            case 2: //down
                directionVector = new Vector3(0, 0, -1);
                break;
            case 3: //down-left
                directionVector = new Vector3(-1, 0, -1);
                break;
            case 4: //left
                directionVector = new Vector3(-1, 0, 0);
                break;
            case 5: //up-left
                directionVector = new Vector3(-1, 0, 1);
                break;
            case 6: //up
                directionVector = new Vector3(0, 0, 1);
                break;
            case 7: // up-right
                directionVector = new Vector3(1, 0, 1);
                break;
            default:
                directionVector = new Vector3(0, 0, 0);
                print("invalid direction, " + directionCode);
                break;

        }
        return directionVector.normalized;
    }

    public int SetDirectionByEight(int directionCode)
    {
        int dir;
        //prioritizes up-down
        switch(directionCode)
        {
            case 0: //right
                dir = 0;
                break;
            case 1: //down-right
            case 2: //down
            case 3: //down-left
                dir = 1;
                break;
            case 4: //left
                dir = 2;
                break;
            case 5: //up-left
            case 6: //up
            case 7: //up-right
                dir = 3;
                break;
            default:
                dir = 0;
                print("invalid direction, " + directionCode);
                break;
        }
        return dir;
    }
}

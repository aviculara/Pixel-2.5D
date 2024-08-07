using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Vector3 startingPosition; //of the game object. don't change in code
    private Vector3 movingPosition;
    private State state = State.Roam;
    public int direction = 0;   // for animator
    private int directionOfPlayer = 0;

    [SerializeField] float speed = 2.5f;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpread;
    [SerializeField] float minPlayerDistance;
    [SerializeField] float aggroRange;
    [SerializeField] float deaggroRange = 50f;
    [SerializeField] float lineOfSightRange = 10f;
    [SerializeField] Transform player;
    [SerializeField] Animator animator;
    //[SerializeField] bool ranged = true;

    [SerializeField] GameObject rightArrow, downArrow, leftArrow, upArrow;

    //private Vector3[] directionPoints = new Vector3[8];
    private Vector3[] directionVectors = new Vector3[8];
    private float[] distanceToTarget = new float[8];
    private int[] directionWeights = new int[8];
    private int playerLayer = 3;

    float minDistance = 1f;

    public enum State
    {
        Roam,
        Attack,
        //Shoot,
        Follow
    }
    // Start is called before the first frame update
    void Start()
    {
        SetDirectionVectors();
        startingPosition = transform.position;
        playerLayer = LayerMask.GetMask("Player");
        //movingPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        DebugDrawDirectionWeights();
        //DebugDrawRanges();
        switch(state)
        {
            default:
                transform.position = Vector3.MoveTowards(transform.position, movingPosition, speed * Time.deltaTime);     
                if(Vector3.Distance(transform.position, movingPosition) < minDistance)
                {
                    movingPosition = TempRoamingPosition();
                }
                //check if player is nearby
                FindTarget();
                break;
            case State.Attack:
                //check if in attacking position
                if(InAttackPosition(out directionOfPlayer))
                {
                    //shoot
                }
                else
                {
                    //move into attacking position
                    transform.Translate(directionVectors[directionOfPlayer] * speed * Time.deltaTime);
                }
                //start shooting if in attacking position
                FindTarget();
                break;
            case State.Follow:
                
                transform.position = Vector3.MoveTowards(transform.position, movingPosition, speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, movingPosition) < minDistance)
                {
                    movingPosition = NewFollowingDirection();
                }
                FindTarget();
                break;
        }
        
        //OLD VERSION
        //switch(state)
        //{
        //    default:
        //        transform.position = Vector3.MoveTowards(transform.position, movingPosition, speed * Time.deltaTime);
        //        float minDistance = 1f;
        //        if(Vector3.Distance(transform.position, movingPosition) < minDistance)
        //        {
        //            movingPosition = GetRoamingPosition();
        //        }
        //        //FindTarget();
        //        break;
        //    case State.Attack:
        //        //attack

        //        FindTarget();
        //        break;
        //    case State.Follow:
        //        //follow
        //        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //        FindTarget();
        //        break;
        //}
    }

    private Vector3 TempRoamingPosition()
    {
        float randomDistance = Random.Range(2f, 5f);
        return transform.position + WeightedRandomDirection() * randomDistance;
    }

    private Vector3 NewFollowingDirection(Transform target = null)
    {
        if(target == null)
        {
            target = player;
        }
        float randomDistance = Random.Range(2f, 5f);
        Vector3 randomDirection = WeightedRandomDirection(target);
        //Debug.DrawRay(transform.position, randomDirection * randomDistance, Color.blue);
        //print("following in direction " + direction);
        return transform.position + randomDirection * randomDistance;
    }

    #region old
    //public Vector3 RandomDirection()
    //{
    //    //characters are on x-z plane
    //    Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    //    return randomDir;
    //}

    //private Vector3 GetRoamingPosition()
    //{
    //    //a random position is created by a random direction and a random distance
    //    float randomDistance = Random.Range(2f, 10f);
    //    //Vector3 newRoamPosition = startingPosition + NewNearbyDirection() * randomDistance;
    //    Vector3 newRoamPosition = transform.position + NewNearbyDirection() * randomDistance;
    //    //print("moving " + randomDistance);
    //    return newRoamPosition;
    //}

    //private Vector3 NewNearbyDirection()
    //{
    //    //     3
    //    // 2 - + - 4 (0) 
    //    //     1
    //    // pick a random number between -2 and +2, divide by 2, add to current direction
    //    // .5 values represent diagonal directions
    //    // mod 4 of the new direction represents its direction value for the animator
    //    // right : Vector3(1,0,0)   ||  up : Vector3(0,0,1) 
    //    int newDirection;
    //    Vector3 directionVector;
    //    newDirection = Random.Range(-2, 2) + direction * 2; //problem
    //    newDirection = PositiveMod(newDirection,8);
    //    print(newDirection);
    //    directionVector = EightDirectionsToVector(newDirection);
    //    direction = SetDirectionByEight(newDirection);
    //    return directionVector;
    //}

    #endregion

    private void FindTarget()
    {
        //change behaviour based on player distance
        //print(Vector3.Distance(player.position, transform.position));
        switch(state)
        {
            case State.Roam:
                if(Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = State.Attack;
                    print("I'm attacking.");
                }
                else if(Vector3.Distance(player.position, transform.position) <= aggroRange)
                {
                    state = State.Follow;
                    movingPosition = NewFollowingDirection();
                    print("Found the player and following.");
                }
                break;
            case State.Follow:
                if(Vector3.Distance(player.position, transform.position) <= attackRange)
                {
                    state = State.Attack;
                    print("I'm attacking.");
                }
                else if(Vector3.Distance(player.position, transform.position) > deaggroRange)
                {
                    state = State.Roam;
                    print("back to roaming");
                }
                break;
            case State.Attack:
                //add later: de-aggro once the player is gone for too long
                //add later: if ranged enemy, can run away when player gets too close
                if(Vector3.Distance(player.position, transform.position) > attackRange)
                {
                    state = State.Follow;
                    movingPosition = NewFollowingDirection();
                    print("i'm following in direction " + direction);
                }
                break;
            default:
                state = State.Roam;
                break;
        }

    }

    private Vector3 EightDirectionsToVector(int directionCode)
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

    //public int SetDirectionByEight(int directionCode)
    //{
    //    int dir;
    //    //prioritizes up-down
    //    switch(directionCode)
    //    {
    //        case 0: //right
    //            dir = 0;
    //            break;
    //        case 1: //down-right
    //        case 2: //down
    //        case 3: //down-left
    //            dir = 1;
    //            break;
    //        case 4: //left
    //            dir = 2;
    //            break;
    //        case 5: //up-left
    //        case 6: //up
    //        case 7: //up-right
    //            dir = 3;
    //            break;
    //        default:
    //            dir = 0;
    //            print("invalid direction, " + directionCode);
    //            break;
    //    }
    //    return dir;
    //}

    private void SetDirectionVectors()
    {
        for(int i = 0; i < directionVectors.Length; i++)
        {
            directionVectors[i] = EightDirectionsToVector(i);
        }
    }

    //public int WeightedRandomDirection()
    //{
    //    //calculate the position of the 8 points, write into the array
    //    SetDistances(startingPosition);
    //    //calculate the weight of the 8 points based on their distance to target
    //    SetDirectionWeights(startingPosition);
    //    WeightLineOfSight();
    //    //pick a random direction with the weights
    //    int totalWeight = 0;
    //    for(int i = 0; i < directionWeights.Length; i++)
    //    {
    //        totalWeight += directionWeights[i];
    //    }
    //    int randomValue = Random.Range(0, totalWeight);
    //    for(int i = 0; i < directionWeights.Length; i++)
    //    {
    //        if(randomValue < directionWeights[i])
    //        {
    //            return i;
    //        }
    //        else
    //        {
    //            randomValue -= directionWeights[i];
    //        }
    //    }
    //    print("random overshoot");
    //    return 0;
    //}

    public Vector3 WeightedRandomDirection(Transform targetTransform = null)
    {
        Vector3 targetVector;
        if(targetTransform == null)
        {
            targetVector = startingPosition;
        }
        else
        {
            targetVector = targetTransform.position;
        }
        //calculate the position of the 8 points, write into the array
        SetDistances(targetVector);
        //calculate the weight of the 8 points based on their distance to target
        SetDirectionWeights(targetVector);
        WeightLinesOfSight();
        //pick a random direction with the weights
        int totalWeight = 0;
        for(int i = 0; i < directionWeights.Length; i++)
        {
            totalWeight += directionWeights[i];
        }
        int randomValue = Random.Range(0, totalWeight);
        for(int i = 0; i < directionWeights.Length; i++)
        {
            if(randomValue < directionWeights[i])
            {
                //print("chose direction " + i);
                //direction = SetDirectionByEight(i);
                direction = i;
                return directionVectors[i];
            }
            else
            {
                randomValue -= directionWeights[i];
            }
        }
        print("random overshoot");
        //direction = SetDirectionByEight(0);
        direction = 0;
        return directionVectors[0];
    }
    public void SetDistances(Vector3 target)
    {
        /* 
         * Calculates the points unit distance away from enemy in 8 directions
         * Also calculates distance of those points to player 
         */
        //Vector3 playerPos = player.transform.position;
        //print(transform.position);
        for(int i = 0; i < distanceToTarget.Length; i++)
        {
            Vector3 directionPoint = transform.position + EightDirectionsToVector(i);
            distanceToTarget[i] = Vector3.Distance(directionPoint, target); //this might need to seperate
            //Debug.DrawLine(directionPoints[i], directionPoints[i] + Vector3.up * 0.1f, Color.red);
            //print(directionPoints[i]);
        }
    }

    public void SetDirectionWeights(Vector3 target)
    {   /* gives weights to the 8 possible directions based on how close they are to target
         * when roaming:
         * best direction weight = 100        worst direction weight = 1
         * in direction of target = 75        away from target = 25
         */

        int maxDistanceIndex = 0;
        int minDistanceIndex = 0;
        float currentDistance = Vector3.Distance(transform.position, target);
        for(int i = 0; i < distanceToTarget.Length; i++)
        {
            if(distanceToTarget[i] < distanceToTarget[minDistanceIndex])
            {
                minDistanceIndex = i;
            }
            else if(distanceToTarget[i] > distanceToTarget[maxDistanceIndex])
            {
                maxDistanceIndex = i;
            }

            if(currentDistance > distanceToTarget[i])   //this direction moves enemy closer to player
            {
                switch(state)
                {
                    default:
                        directionWeights[i] = 75;
                        break;
                    case State.Follow:
                        directionWeights[i] = 40;
                        break;
                }
            }
            else if(currentDistance < distanceToTarget[i])  //this direction moves enemy further from player
            {
                switch(state)
                {
                    default:
                        directionWeights[i] = 25;
                        break;
                    case State.Follow:
                        directionWeights[i] = 1;
                        break;
                }

            }
            else
            {
                switch(state)
                {
                    default:
                        directionWeights[i] = 50;
                        break;
                    case State.Follow:
                        directionWeights[i] = 5;
                        break;
                }
            }
        }
        directionWeights[maxDistanceIndex] = 1;
        directionWeights[minDistanceIndex] = 100;
        //print("best direction is " + minDistanceIndex + " , worst direction is " + maxDistanceIndex);

    }

    private void WeightLinesOfSight()
    {
        /*
         * reduce weights if obstacle within line of sight
         */
        for(int i = 0; i < distanceToTarget.Length; i++ )
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, directionVectors[i], out hit, lineOfSightRange))
            {
                print("Found an object: " + hit.transform.name);
                if(hit.transform.CompareTag("Obstacle"))
                {
                    directionWeights[i] = 1;
                }
                else if(hit.transform.CompareTag("Player"))
                {
                    directionWeights[i] = 150;
                }

            }
            Debug.DrawRay(transform.position, directionVectors[i] * lineOfSightRange, Color.yellow);
        }
        

    }

    private void DebugDrawDirectionWeights()
    {
        for(int i = 0; i < directionVectors.Length; i++)
        {
            Debug.DrawRay(transform.position, directionVectors[i] * directionWeights[i] /20 , Color.red);
        }
    }

    private void DebugDrawRanges()
    {
        for(int i = 0; i < directionVectors.Length; i +=2)
        {
            Debug.DrawRay(transform.position, directionVectors[i] * attackRange, Color.red);
            Debug.DrawRay(transform.position, directionVectors[i] * aggroRange, Color.yellow);
            Debug.DrawRay(transform.position, directionVectors[i] * deaggroRange, Color.cyan);
        }
    }

    private void Shoot()
    {
        //attack
    }

    private bool InAttackPosition(out int directionOfPlayer)
    {
        /*
         * check if player is in an attackable direction
         * out the direction of player
         * obstacles need to be in same layer as player to detect and avoid obstacles
         */
        directionOfPlayer = 4;
        //int[] attackDirectionWeights = new int[4];
        float leastDistance = 100f;

        //is player within targeting range
        for(int i = 0; i< directionVectors.Length; i +=2)
        {
            directionWeights[i] = 0;
            float distanceToPlayer;
            int attackWeight;
            //perpendicular directions are at +2 and -2 index (mod 8) of the direction vectors
            //Vector3 centerPosition = transform.position;
            Vector3 clockwisePosition = transform.position + directionVectors[PositiveMod(i + 2 , 8)] * attackSpread/2;
            Vector3 antiClockwisePosition = transform.position + directionVectors[PositiveMod(i - 2, 8)] * attackSpread / 2;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionVectors[i], out hit, attackRange, playerLayer)
                || Physics.Raycast(clockwisePosition, directionVectors[i], out hit, attackRange, playerLayer)
                || Physics.Raycast(antiClockwisePosition, directionVectors[i], out hit, attackRange, playerLayer))
            {
                Debug.DrawRay(clockwisePosition, directionVectors[i] * attackRange, Color.black);
                Debug.DrawRay(transform.position, directionVectors[i] * attackRange, Color.black);
                Debug.DrawRay(antiClockwisePosition, directionVectors[i] * attackRange, Color.black);

                if(hit.transform.CompareTag("Player"))
                {
                    //shoot player in this direction
                    directionOfPlayer = i;
                    print("Found " + hit.transform.name + "in direction " + directionOfPlayer);
                    return true;
                }
                else if(hit.transform.CompareTag("Obstacle"))
                {
                    print("found obstacle in direction " + i);
                    directionWeights[i] = 0;
                    //don't go in this direction
                }
            }
            else 
            {
                distanceToPlayer = Vector3.Distance(transform.position + directionVectors[i] * attackRange, player.position);
                //attackWeight = (int)((deaggroRange - distanceToPlayer)*10);
                //print(distanceToPlayer + " with weight " + attackWeight);
                //directionWeights[i] = attackWeight;

                if(distanceToPlayer < leastDistance)
                {
                    leastDistance = distanceToPlayer;
                    directionOfPlayer = i;
                }
            }
            //i++;
            //directionWeights[i] = 0;
            ////distanceToPlayer = Vector3.Distance(transform.position + directionVectors[i] * attackRange, player.position);
            ////attackWeight = (int)((deaggroRange - distanceToPlayer) * 10);
            ////print(distanceToPlayer + " with weight " + attackWeight);
            ////directionWeights[i] = attackWeight;

        }
        print("so far best direction is " + directionOfPlayer);
        //player isnt within targeting range, return false and out best direction to move
        if(
            Vector3.Distance(transform.position + directionVectors[PositiveMod(directionOfPlayer + 2, 8)] * attackRange, player.position) //perpendicular cw direction
            >
            Vector3.Distance(transform.position + directionVectors[PositiveMod(directionOfPlayer - 2, 8)] * attackRange, player.position)) //perpendicular ccw direction
        {
            directionOfPlayer = PositiveMod((directionOfPlayer - 2), 8);
        }
        else
        {
            directionOfPlayer = PositiveMod((directionOfPlayer + 2), 8);
        }
        //WeightLinesOfSight();
        //directionOfPlayer = ArrayMax(directionWeights);
        print("player is in direction " + directionOfPlayer);
        return false;
    }

    public static int PositiveMod(int dividend, int modulus)
    {
        int result = (dividend % modulus);
        if(result < 0)
        {
           result = (result +modulus) % modulus;
        }
        return result;
    }

    public static int ArrayMax(int[] array)
    {
        int maxIndex = 0;
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i] > array[maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private void OnEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Obstacle"))
        {
            state = State.Follow;
            movingPosition = NewFollowingDirection();
            print("switched to following");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy3AI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Melee,
        Dead
    }

    public FSMStates currentState;
    public float attackDistance = 7;
    public float meleeDistance = 2;
    public float chaseDistance = 15;
    public float enemySpeed = 5;
    public GameObject player;
    GameObject[] wanderPoints;
    Vector3 nextDestination;
    Animator anim;
    float distanceToPlayer;
    float elapsedTime;
    public GameObject[] spellProjectiles;
    public GameObject handEnd;
    public float shootRate = 2.0f;
    EnemyHealth enemyHealth;
    int health;
    public GameObject deadVFX;
    Transform deadTransform;
    bool isDead;
    public AudioClip spellFX;
    public AudioClip punchFX;
    int currentDestinationIndex = 0;
    NavMeshAgent agent;
    public Transform enemyEyes;
    public float fieldOfView = 45f;

    // Start is called before the first frame update
    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint3");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        handEnd = GameObject.FindGameObjectWithTag("HandEnd");

        enemyHealth = GetComponent<EnemyHealth>();

        health = enemyHealth.currentHealth;

        isDead = false;

        Initialize();

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        health = enemyHealth.currentHealth;

        switch(currentState)
        {
            case FSMStates.Patrol:
            UpdatePatrolState();
            break;
            case FSMStates.Chase:
            UpdateChaseState();
            break;
            case FSMStates.Attack:
            UpdateAttackState();
            break;
            case FSMStates.Melee:
            UpdateMeleeState();
            break;
            case FSMStates.Dead:
            UpdateDeadState();
            break;
        }

        elapsedTime += Time.deltaTime;

        if(health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    void Initialize()
    {
        currentState = FSMStates.Patrol; 
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        print ("Patrolling");
        anim.SetInteger("animState", 1);

        agent.stoppingDistance = 0;
        agent.speed = 3.5f;

        if(Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if(distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);

        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateChaseState()
    {
        print ("Chasing");
        anim.SetInteger("animState", 1);

        agent.stoppingDistance = attackDistance;
        agent.speed = 5f;

        nextDestination = player.transform.position;

        if(distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if(distanceToPlayer <= meleeDistance)
        {
            currentState = FSMStates.Melee;
        }
        else if(distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);

        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateAttackState()
    {
        print("attack");

        nextDestination = player.transform.position;

        agent.stoppingDistance = attackDistance;

        if(distanceToPlayer <= meleeDistance)
        {
            currentState = FSMStates.Melee;
        }
        else if(distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if(distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if(distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);
        anim.SetInteger("animState", 2);
        EnemySpellCast();
    }

    void UpdateMeleeState()
    {
        print("melee");

        nextDestination = player.transform.position;

        agent.stoppingDistance = meleeDistance;

        if(distanceToPlayer <= meleeDistance)
        {
            currentState = FSMStates.Melee;
        }
        else if(distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if(distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if(distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);
        anim.SetInteger("animState", 3);
    }

    void UpdateDeadState()
    {
        anim.SetInteger("animState", 4);
        isDead = true;
        deadTransform = gameObject.transform;
        print("Enemy is dead!");

        Destroy(gameObject, 3);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemySpellCast()
    {
        if(!isDead)
        {
            if(elapsedTime >= shootRate)
            {
                var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
                AudioSource.PlayClipAtPoint(spellFX, transform.position);
                Invoke("SpellCasting", 2);
                elapsedTime = 0.0f;
            }
        }
        
    }

    void SpellCasting()
    {
        int randProjectileIndex = Random.Range(0, spellProjectiles.Length);

        GameObject spellProjectile = spellProjectiles[randProjectileIndex];

        Instantiate(spellProjectile, handEnd.transform.position, handEnd.transform.rotation);
    }

    private void OnDrawGizmo()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if(Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if(Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    print ("Player in sight!");
                    return true;
                }

                return false;
            }
            return false;
        }
        return false;
    }

    private void OnDestroy()
    {
        Instantiate(deadVFX, transform.position, transform.rotation);
    }
}

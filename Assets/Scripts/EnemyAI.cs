using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates {
        //Idle,
        Patrol,
        Chase,
        Attack
        //Dead
    }

    public FSMStates currentState;

    public int damageMin = 5;
    public int damageMax = 10;

    public float attackDistance = 5;
    public float chaseDistance = 10;
    public float enemySpeed = 5;
    public GameObject player;
    //public GameObject[] spellProjectiles;
    public GameObject spellProjectile;
    public GameObject wandTip;
    public float shootRate = 2.0f;
    //public GameObject deadVFX;

    private float elapsedTime = 0;

    //EnemyHealth enemyHealth;
    //int health;

    GameObject[] wanderPoints;

    Vector3 nextDestination;

    Animator anim;

    float distanceToPlayer;

    int currentDestinationIndex = 0;

    Transform deadTransform;

    bool isDead = false;

    NavMeshAgent agent;

    public Transform enemyEyes;
    public float fieldOfView;

    private Vector3 lastHit = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");

        //anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");

        //wandTip = GameObject.FindGameObjectWithTag("WandTip");

        //enemyHealth = GetComponent<EnemyHealth>();

        //health = enemyHealth.currentHealth;

        isDead = false;

        agent = GetComponent<NavMeshAgent>();

        Initialize();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //health = enemyHealth.currentHealth;

        switch(currentState) {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            //case FSMStates.Dead:
            //    UpdateDeadState();
            //    break;
        }

        elapsedTime += Time.deltaTime;

        //if (health <= 0) {
        //    //currentState = FSMStates.Dead;
        //}
    }

    void Initialize() {
        currentState = FSMStates.Patrol;

        FindNextPoint();
    }

    void UpdatePatrolState() {
        //print("Patrolling!");

        //anim.SetBool("Attacking", false);

        agent.stoppingDistance = 0;

        if (Vector3.Distance(transform.position, nextDestination) <= 3) {
            FindNextPoint();
        } else if (distanceToPlayer < chaseDistance && IsPlayerInFOV()) {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateChaseState() {
        //print("Chasing!");

        //anim.SetBool("Attacking", false);
        //anim.SetInteger("animState", 2);

        agent.stoppingDistance = attackDistance;

        //agent.speed = 5;

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > chaseDistance) {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }

    void UpdateAttackState() {
        //print("Attack!");

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance) {
            currentState = FSMStates.Attack;
        } else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance) {
            currentState = FSMStates.Chase;
        } else if (distanceToPlayer > chaseDistance) {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);

        //anim.SetBool("Attacking", true);
        //anim.SetInteger("animState", 3);

        EnemySpellCast();
    }

    void UpdateDeadState() {
        isDead = true;

        //anim.SetInteger("animState", 4);

        deadTransform = gameObject.transform;

        Destroy(gameObject, 3);
    }

    void FindNextPoint() {
        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;

        nextDestination = wanderPoints[currentDestinationIndex].transform.position;
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = (target - transform.position).normalized;

        directionToTarget.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemySpellCast() {
        if (elapsedTime >= shootRate) {
            Debug.Log("SHOOT!");
            Instantiate(spellProjectile, wandTip.transform.position, wandTip.transform.rotation);
            //Invoke("Shoot");
            elapsedTime = 0f;
        }

        //if (elapsedTime >= shootRate) {

        //    var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

        //    Invoke("SpellCasting", animDuration / 2);
        //    elapsedTime = 0f;
        //}
    }

    void Shoot() {
        Instantiate(spellProjectile, wandTip.transform.position, wandTip.transform.rotation);
    }

    //void SpellCasting() {
    //    //GameObject spellProjectile =
    //    //    spellProjectiles[Random.Range(0, spellProjectiles.Length)];

    //    if (!isDead) {
    //        Instantiate(spellProjectile, wandTip.transform.position, wandTip.transform.rotation);
    //    }
    //}

    //private void OnDestroy() {
    //    deadTransform = gameObject.transform;
    //    Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
    //}

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 directionToPlayer = (player.transform.position - enemyEyes.position).normalized;

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);

        //Debug.DrawLine(enemyEyes.position, enemyEyes.position + (directionToPlayer * chaseDistance), Color.red);
        Debug.DrawLine(enemyEyes.position, lastHit, Color.red);
        //Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
        //Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.yellow);
    }


    private bool IsPlayerInFOV() {
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        directionToPlayer.y = 0;

        RaycastHit hit;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView) {

            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance)) {
                lastHit = hit.point;
                if (hit.collider.CompareTag("Player")) {
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }
    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            Debug.Log("Hit player!");
            var playerHealth = collider.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(Random.Range(damageMin, damageMax));

            //var playerController = collider.GetComponent<PlayerController>();
            //playerController.Knockback(transform.position);
        }
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    public RecoveryCounter recoveryCounter;
    public Animator anim;
    public Stats stats;
    public float patrolRange;
    public float attentionRange = 5f;
    public float stoppingDistance = 0.2f;

    [SerializeField] private Vector3 patrolPoint1;
    [SerializeField] private Vector3 patrolPoint2;
    [SerializeField] private float point1;
    [SerializeField] private float point2;
    public GameObject graphic;

    private float originalXPosition;
    private GameObject playerObject;
    private bool isAttacking = false;
    [SerializeField] private float targetPositionX;
    private Rigidbody2D rb;


    [Header("Reference")]
    private EnemyBase enemyBase;

    [Header("Properties")]
    [SerializeField] private LayerMask layerMask; //What can the Walker actually touch?
    [SerializeField] enum EnemyType { Bug, Zombie, Boss, Bowman, Spearman}; //Bugs will simply patrol. Zombie's will immediately start chasing you forever until you defeat them.
    [SerializeField] EnemyType enemyType;

    public float changeDirectionEase = 1; //How slowly should we change directions? A higher number is slower!
    [System.NonSerialized] public float direction = 1;
    private float distanceFromTargetX; //How far is this enemy from the player?
    [System.NonSerialized] public float directionSmooth = 1; //The float value that lerps to the direction integer.
    [SerializeField] private bool followPlayer;
    [SerializeField] private bool flipWhenTurning = true; //Should the graphic flip along localScale.x?
    public float hurtLaunchPower = 10; //How much force should be applied to the player when getting hurt?
    public float jumpPower = 7;
    [System.NonSerialized] public bool jump = false;
    [System.NonSerialized] public float launch = 1; //The float added to x and y moveSpeed. This is set with hurtLaunchPower, and is always brought back to zero
    public float maxSpeed = 7;
    [SerializeField] private float maxSpeedDeviation; //How much should we randomly deviate from maxSpeed? Ensures enemies don't move at exact same speed, thus syncing up.
    private float movementSpeed;
    [SerializeField] private bool neverStopFollowing = false; //Once the player is seen by an enemy, it will forever follow the player.
    private Vector3 origScale;
    [SerializeField] private Vector2 rayCastSize = new Vector2(1.5f, 1); //The raycast size: (Width, height)
    private Vector2 rayCastSizeOrig;
    public float slowedSpeed;
    private Vector2 moveDirection;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform arrowSpawnPos;
    [SerializeField] private GameObject arrowPrefab;
    private int walkingDir; // this is used for determining which way to walk when targeting the basehouse, 1 for right -1 left

    private float sitStillMultiplier = 1; //If 1, the enemy will move normally. But, if it is set to 0, the enemy will stop completely. 
    [SerializeField] private bool sitStillWhenNotFollowing = false; //Controls the sitStillMultiplier

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip stepSound;
    public bool canMove = true;
    private bool hasPerformedFirstAttack;
    public bool isKnockedBack;

    // States
    private enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }

    private EnemyState currentState;

    // Start is called before the first frame update
    void Start()
    {
        patrolPoint1 = transform.position;
        patrolPoint2.y = transform.position.y;
        movementSpeed = stats.moveSpeed;
        movementSpeed = Random.Range(movementSpeed - maxSpeedDeviation, movementSpeed + maxSpeedDeviation);
        originalXPosition = transform.position.x;
        anim = GetComponentInChildren<Animator>();
        recoveryCounter = GetComponent<RecoveryCounter>();
        enemyBase = GetComponent<EnemyBase>();
        currentState = EnemyState.Patrolling;
        point1 = patrolPoint1.x;
        point2 = patrolPoint2.x;
        rb = GetComponent<Rigidbody2D>();
        targetPositionX = point1;
        launch = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove && !isKnockedBack)
        {
            switch (currentState)
            {
                case EnemyState.Patrolling:
                    Patrolling();
                    break;
                case EnemyState.Attacking:
                    Attacking();
                    break;
                case EnemyState.Chasing:
                    Chasing();
                    break;
                default:
                    break;
            }

            if (!isKnockedBack)
                ComputeVelocity();
        }
    }

    protected void ComputeVelocity()
    {
        if (!recoveryCounter.attackRecovering)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                walkingDir = -1;
            }
            else if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                walkingDir = 1;
            }

            if (walkingDir == -1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);

            }
        }
    }

    public void Patrolling()
    {

        if (target == null)
        {
            //Debug.Log("patrolling");

            float tolerance = 0.1f; // Adjust this value as needed

            if (Mathf.Abs(transform.position.x - point1) < tolerance)
            {
                targetPositionX = point2;
            }

            if (Mathf.Abs(transform.position.x - point2) < tolerance)
            {
                targetPositionX = point1;
            }

            if (IsPlayerInRange(attentionRange) && !PlayerController.Instance.frozen)
            {
                target = PlayerController.Instance.gameObject;
                targetPositionX = PlayerController.Instance.transform.position.x;
                currentState = EnemyState.Chasing;
            }

            anim.SetBool("isRunning", true);
            moveDirection = new Vector2(targetPositionX - transform.position.x, 0).normalized;

            //rb.AddForce(moveDirection * stats.moveSpeed);
            if (!isKnockedBack && !recoveryCounter.attackRecovering)
                rb.velocity = moveDirection * stats.moveSpeed;
            // +  launch * new Vector2(-walkingDir, 1)
        }
        else
        {
            currentState = EnemyState.Chasing;
        }
    }

    public void Attacking()
    {
        //Debug.Log("attacking");

        anim.SetBool("isRunning", false);
        rb.velocity = Vector2.zero;
        distanceFromTargetX = target.transform.position.x - transform.position.x;

        walkingDir = target.transform.position.x < transform.position.x ? -1 : 1;

        if (!recoveryCounter.attackRecovering && !target.GetComponent<RecoveryCounter>().recovering)
        {
            if (hasPerformedFirstAttack)
            {
                anim.SetTrigger("attack1");
                recoveryCounter.ResetAttackCounter();
            }

            if (!hasPerformedFirstAttack)
            {
                recoveryCounter.ResetAttackCounter();
                hasPerformedFirstAttack = true;
            }
        }

        if (Mathf.Abs(distanceFromTargetX) > stoppingDistance && !target.GetComponent<EnemyBase>().isDead)
        {
            currentState = EnemyState.Chasing;
            return;
        }

        if (target.GetComponent<EnemyBase>().isDead)
        {
            target = null;
            targetPositionX = point1;
            currentState = EnemyState.Patrolling;
            return;
        }

        // if (!IsPlayerInRange(attentionRange))
        // {
        //     currentState = EnemyState.Patrolling;
        // }
    }

    public void Chasing()
    {
        //hasPerformedFirstAttack = false;
        //Debug.Log("Chasing");
        targetPositionX = target.transform.position.x;
        distanceFromTargetX = target.gameObject.transform.position.x - transform.position.x;
        directionSmooth += ((direction * sitStillMultiplier) - directionSmooth) * Time.deltaTime * changeDirectionEase;

        if (Mathf.Abs(distanceFromTargetX) > stoppingDistance && !target.GetComponent<EnemyBase>().isDead && target != null && !recoveryCounter.attackRecovering)
        {
            anim.SetBool("isRunning", true);

            //You can do it using the hurtlauncher
            Vector2 moveDirection = new Vector2(targetPositionX - transform.position.x, 0).normalized;
            //rb.AddForce(moveDirection * stats.moveSpeed);

            rb.velocity = moveDirection * stats.moveSpeed;
        }
        if (Mathf.Abs(distanceFromTargetX) <= stoppingDistance && !target.GetComponent<EnemyBase>().isDead && target != null)
        {
            currentState = EnemyState.Attacking;
        }

        if (IsTargetInRange(attentionRange) == false)
        {
            target = null;
            targetPositionX = point1;
            currentState = EnemyState.Patrolling;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        // Draw a line between patrol points
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(patrolPoint1, patrolPoint2);
    }

    public void Flip()
    {

    }

    private bool IsTargetInRange(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("Companion"))
            {
                return true;
            }
        }
        hasPerformedFirstAttack = false;
        return false;
    }

    private bool IsPlayerInRange(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCompanionInRange(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Companion"))
            {
                return true;
            }
        }
        return false;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void KnockBack()
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            StartCoroutine(KnockBackCoroutine());
        }
    }

    public GameObject GetArrowPrefab()
    {
        return arrowPrefab;
    }
    
    public Transform GetArrowSpawnPos()
    {
        return arrowSpawnPos;
    }

    public IEnumerator KnockBackCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        isKnockedBack = false;
    }

}
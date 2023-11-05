using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class Companion : MonoBehaviour
{
    public RecoveryCounter recoveryCounter;
    public Animator anim;
    public Stats stats;
    public float patrolRange;
    public float attentionRange = 5f;
    public float stoppingDistance = 0.2f;

    public GameObject graphic;

    private float originalXPosition;
    private GameObject playerObject;
    private bool isAttacking = false;
    private float targetPositionX;
    private Rigidbody2D rb;


    [Header("Reference")]
    private EnemyBase enemyBase;

    [Header("Properties")]
    [SerializeField] private LayerMask layerMask; //What can the Walker actually touch?

    public float changeDirectionEase = 1; //How slowly should we change directions? A higher number is slower!
    [System.NonSerialized] public float direction = 1;
    private float distanceFromPlayerX; //How far is this enemy from the player?
    [System.NonSerialized] public float directionSmooth = 1; //The float value that lerps to the direction integer.
    [SerializeField] private bool followPlayer;
    [SerializeField] private bool flipWhenTurning = true; //Should the graphic flip along localScale.x?
    public float hurtLaunchPower = 10; //How much force should be applied to the player when getting hurt?
    public float jumpPower = 7;
    [System.NonSerialized] public bool jump = false;
    [System.NonSerialized] public float launch = 1; //The float added to x and y moveSpeed. This is set with hurtLaunchPower, and is always brought back to zero
    public float maxSpeed = 7;
    [SerializeField] private float maxSpeedDeviation; //How much should we randomly deviate from maxSpeed? Ensures enemies don't move at exact same speed, thus syncing up.
    [SerializeField] private bool neverStopFollowing = false; //Once the player is seen by an enemy, it will forever follow the player.
    private Vector3 origScale;
    [SerializeField] private Vector2 rayCastSize = new Vector2(1.5f, 1); //The raycast size: (Width, height)
    private Vector2 rayCastSizeOrig;
    public float slowedSpeed;
    private Vector2 moveDirection;
    private int walkingDir; // this is used for determining which way to walk when targeting the basehouse, 1 for right -1 left

    private float sitStillMultiplier = 1; //If 1, the enemy will move normally. But, if it is set to 0, the enemy will stop completely. 
    [SerializeField] private bool sitStillWhenNotFollowing = false; //Controls the sitStillMultiplier

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip stepSound;
    private GameObject attackTarget;
    public bool isKnockedBack;
    public AnimatorOverrideController[] overrideControllers;

    // States
    public enum CompanionState
    {
        Following,
        Waiting,
        Attacking
    }

    public CompanionState currentState;

    // Start is called before the first frame update
    void Start()
    {
        originalXPosition = transform.position.x;
        anim = GetComponentInChildren<Animator>();
        recoveryCounter = GetComponent<RecoveryCounter>();
        enemyBase = GetComponent<EnemyBase>();
        currentState = CompanionState.Following;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKnockedBack)
        {
            ComputeVelocity();

            switch (currentState)
            {
                case CompanionState.Following:
                    Following();
                    break;
                case CompanionState.Attacking:
                    Attacking();
                    break;
                case CompanionState.Waiting:
                    Waiting();
                    break;
                default:
                    break;
            }
        }
    }

    protected void ComputeVelocity()
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
    }

    public void Following()
    {
        targetPositionX = PlayerController.Instance.transform.position.x;
        distanceFromPlayerX = PlayerController.Instance.gameObject.transform.position.x - transform.position.x;
        directionSmooth += ((direction * sitStillMultiplier) - directionSmooth) * Time.deltaTime * changeDirectionEase;

        if (Mathf.Abs(distanceFromPlayerX) > stoppingDistance)
        {
            anim.SetBool("isRunning", true);
            Vector2 moveDirection = new Vector2(PlayerController.Instance.transform.position.x - transform.position.x, 0).normalized;
            //Vector2 moveDirection = new Vector2(PlayerController.Instance.transform.position.x - transform.position.x, PlayerController.Instance.transform.position.y - transform.position.y).normalized;
            rb.velocity = moveDirection * stats.moveSpeed;
        }
        else
        {
            anim.SetBool("isRunning", false);
            rb.velocity = Vector2.zero;
        }
    }

    public void Attacking()
    {
        if(isEnemyInRange(attentionRange) && !attackTarget.GetComponent<EnemyBase>().isDead){
        
        float distanceFromTargetX = attackTarget.transform.position.x - transform.position.x;

        if (Mathf.Abs(distanceFromTargetX) > stoppingDistance)
        {
            anim.SetBool("isRunning", true);
            Vector2 moveDirection = new Vector2(attackTarget.transform.position.x - transform.position.x, 0).normalized;
            rb.velocity = moveDirection * stats.moveSpeed; 
        }
        else
        {
            anim.SetBool("isRunning", false);
            rb.velocity = Vector2.zero;
            Attack(attackTarget);
        }
        }
        else{
            currentState = CompanionState.Following;
        }
    }

    public void Waiting()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("isRunning", false);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);
    }

    public void Attack(GameObject target){
        if(!recoveryCounter.attackRecovering && !target.GetComponent<RecoveryCounter>().recovering){
            Debug.Log("Companion attacked");
            anim.SetTrigger("attack1");
            recoveryCounter.ResetAttackCounter();
        }
    }

    private bool isEnemyInRange(float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                if(col.gameObject.GetComponent<EnemyBase>() != null && col.gameObject.GetComponent<Stats>().health > 0)
                attackTarget = col.gameObject;
                Debug.Log(attackTarget + " is the current target to attack");
                return true;
            }
        }
        return false;
    }
    
    public void KnockBack()
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            StartCoroutine(KnockBackCoroutine());
        }
    }

    public IEnumerator KnockBackCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        isKnockedBack = false;
    }

    public void Evolve(){
                // Check if the reference is not null and if the GameObject has an Animator component.
        foreach (AnimatorOverrideController newAnimator in overrideControllers)
        {
            if (newAnimator != null && TryGetComponent(out Animator animator))
            {
                animator.runtimeAnimatorController = newAnimator;
                return;
            }
            else
            {
                Debug.LogError("Override Controller or Animator component not found.");
            }

        }

    }


}
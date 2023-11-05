using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


//using UnityEditor.Callbacks;
using UnityEngine;
public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }
    private float origAttackCD;
    public float changedAttackCD;
    private float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rollForce = 10f;
    public float rollDuration = 1f;
    public float chargeDuration = 1f;
    public Animator anim;
    private Transform playerTransform;
    private float direction = 1f; // 1 for right, -1 for left
    private bool isGrounded = true;
    private Rigidbody2D playerRigidbody;
    private int currentAttack = 1;
    private float lastAttackTime = 0f;
    public float comboTimeThreshold = 0.3f;
    private float rollEndTime = 0f;
    private bool isCharging = false;
    private float chargeStartTime = 0f;
    private RecoveryCounter recoveryCounter;
    private bool isRolling;
    public Stats stats;
    public bool frozen = false;
    public bool isBlocking;
    public GameObject companionGameobject;
    //[SerializeField] private PlayerPosition playerStartPos;
    private Companion companionAI;
    public bool companionIsOut = true;
    public bool hasCompanion = false;
    public PlayerSounds playerSounds;
    public bool isClimbing;

        private void Awake()
    {
        //transform.position = new Vector3(playerStartPos.x, 0);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        recoveryCounter = GetComponent<RecoveryCounter>();
        moveSpeed = stats.moveSpeed;
        anim = GetComponentInChildren<Animator>();
        playerTransform = transform;
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSounds = GetComponentInChildren<PlayerSounds>();
        origAttackCD = recoveryCounter.attackCooldown;
        if(companionGameobject != null)
        companionAI = companionGameobject.GetComponent<Companion>();

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            HUD.Instance.ShowUI();
        }

        if (!frozen)
        {
            // Check for left mouse click (attack)
            if (Input.GetMouseButtonDown(0) && !isRolling)
            {
                if (!isGrounded)
                {
                    StartCoroutine(SlowdownYVelocity());
                    // playerRigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                }

                float elapsedTime = Time.time - lastAttackTime;
                if (elapsedTime <= comboTimeThreshold)
                {
                    if (!recoveryCounter.attackRecovering)
                    {
                        currentAttack++;
                        if (currentAttack > 3)
                            currentAttack = 1;
                    }
                }
                else
                {
                    currentAttack = 1;
                }

                lastAttackTime = Time.time;

                if (!recoveryCounter.attackRecovering)
                {
                    switch (currentAttack)
                    {
                        case 1:
                            anim.SetTrigger("attack1");
                            recoveryCounter.ResetAttackCounter();
                            recoveryCounter.attackCooldown = changedAttackCD;
                            break;
                        case 2:
                            anim.SetTrigger("attack2");
                            recoveryCounter.ResetAttackCounter();
                            //So when we are attacking quickly the cooldown is less
                            recoveryCounter.attackCooldown = changedAttackCD;
                            break;
                        case 3:
                            anim.SetTrigger("attack3");
                            recoveryCounter.ResetAttackCounter();
                            recoveryCounter.attackCooldown = origAttackCD;
                            break;
                    }
                }
            }

            anim.SetFloat("moveY", playerRigidbody.velocity.y);

            // Get the input for horizontal movement
            float horizontalInput = Input.GetAxis("Horizontal");

            // Calculate the new position
            Vector3 newPosition = playerTransform.position + new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);
            // Update the position using Transform
            playerTransform.position = newPosition;

            anim.SetBool("isClimbing", isClimbing);

            if(isClimbing)
            {
            float verticalInput = Input.GetAxis("Vertical"); // Detect input for climbing (up or down).
            Vector2 climbVelocity = new Vector2(0, verticalInput * 0.8f);
            GetComponent<Rigidbody2D>().velocity = climbVelocity;
            if(verticalInput == 0)
            anim.SetTrigger("climb");
            }

            // Update the direction (flip sprite)
            if (horizontalInput != 0)
            {
                direction = Mathf.Sign(horizontalInput);
                playerTransform.localScale = new Vector3(direction, 1f, 1f);
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }


            // Check for Space key (jump)
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isRolling)
            {
                playerRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                anim.SetTrigger("jump");
                stats.stamina -= 5;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                isBlocking = true;
                anim.SetBool("isBlocking", true);
                //playerRigidbody.velocity = Vector2.zero;
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                isBlocking = false;
                anim.SetBool("isBlocking", false);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                HUD.Instance.ShowObject(HUD.Instance.infoObject);
            }


            // Temporary way to heal pet
            if (Input.GetKeyDown(KeyCode.P) && hasCompanion && companionIsOut) 
            {
                companionAI.stats.Heal(3);
                stats.damage -=3;
                companionAI.stats.GainXp(20);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                List<string> myKeys = new List<string>();

                foreach (var key in Inventory.Instance.inventory.Keys)
                {
                    myKeys.Add(key);
                }

                string allKeys = string.Join(", ", myKeys);
                Debug.Log(allKeys);
            }

            if (Input.GetKeyDown(KeyCode.R) && isGrounded && !isRolling)
            {
                if (companionIsOut && hasCompanion)
                {
                    companionGameobject.SetActive(false);

                    companionIsOut = false;
                }
                else
                {
                    if(companionGameobject != null && companionGameobject.GetComponent<Stats>().health > 0 && hasCompanion){

                    companionGameobject.transform.position = new Vector2(transform.position.x + (direction * 0.3f), transform.position.y);
                    anim.SetTrigger("summon");
                        StartCoroutine(SummonCompanion(0.2f));

                    }

                    GameObject foundObject = GameObject.Find(companionGameobject.name);
                    if (foundObject == null && companionGameobject == null)
                    {
                        GameObject GO = Instantiate(companionGameobject, new Vector2(transform.position.x - 0.3f, transform.position.y), Quaternion.identity);
                        companionGameobject = GO;
                        HUD.Instance.companionStats = companionGameobject.GetComponent<Stats>();
                        companionIsOut = true;
                        companionGameobject.SetActive(true);
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
            {
                companionAI.currentState = Companion.CompanionState.Following;
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
            {
                companionAI.currentState = Companion.CompanionState.Waiting;
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A))
            {
                companionAI.currentState = Companion.CompanionState.Attacking;
            }


            // Check for Left Control key (roll)
            if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && stats.SpendStamina(5) && !isRolling)
            {
                playerRigidbody.AddForce(new Vector2(direction * rollForce, 0f), ForceMode2D.Impulse);
                anim.SetTrigger("roll");
                rollEndTime = Time.time + rollDuration;
                stats.SpendStamina(5);
            }

            if (Time.time >= rollEndTime)
            {
                anim.ResetTrigger("roll");
                isRolling = false;
            }
            else
            {
                isRolling = true;
            }

            // Check for Right Mouse Button (charge)
            if (Input.GetMouseButtonDown(1))
            {
                chargeStartTime = Time.time;
                anim.SetBool("isCharging", true);
                // Set the weight of the "Combat" layer to 1 (active).
                //anim.SetLayerWeight(1, 1.0f);
                // Set the weight of the "Movement" layer to 0 (inactive).
                //anim.SetLayerWeight(0, 0.0f);
            }

            if (Input.GetMouseButtonUp(1))
            {
                float chargeDuration = Time.time - chargeStartTime;
                if (chargeDuration >= 1f)
                {
                    // Set the weight of the "Combat" layer to 1 (active).
                    //anim.SetLayerWeight(1, 1.0f);
                    // Set the weight of the "Movement" layer to 0 (inactive).
                    //anim.SetLayerWeight(0, 0.0f);
                    anim.SetTrigger("heavyAttack");

                }
                anim.SetBool("isCharging", false);
            }
        }
    }

    // Sets the type of ground we are running on in our PlayerSounds script.
    public void SetGroundType()
    {
        //If we want to add variable ground types with different sounds, it can be done here
        switch (playerSounds.groundType)
        {
            case "Grass":
                playerSounds.stepSound = playerSounds.grassSound;
                break;
            case "Dirt":
                playerSounds.stepSound = playerSounds.dirtSound;
                break;
        }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Check if player is grounded
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;
    //         anim.SetBool("isGrounded", isGrounded);
    //     }
    // }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.CompareTag("Ladder") && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    //     {
    //         isClimbing = true;
    //         GetComponent<Rigidbody2D>().gravityScale = 0;
    //     }
    // }

        void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            anim.SetBool("isClimbing", false);
            isClimbing = false;
            // Restore gravity.
            GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        }
    }


    IEnumerator SummonCompanion(float wait)
    {
        yield return new WaitForSeconds(wait);
        companionIsOut = true;
        companionGameobject.SetActive(true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if player is no longer grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            anim.SetBool("isGrounded", isGrounded);
        }
    }

        IEnumerator SlowdownYVelocity()
    {
        float initialVelocity = playerRigidbody.velocity.y;
        float startTime = Time.time;

        while (Time.time < startTime + 0.2f)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / 0.2f;
            float newVelocity = Mathf.Lerp(initialVelocity, 0f, t);
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, newVelocity);
            yield return null;
        }
    }

}
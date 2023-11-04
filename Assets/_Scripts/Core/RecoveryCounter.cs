using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCounter : MonoBehaviour
{
    //This script can be attached to any gameObject that has an EnemyBase or Breakable script attached to it.
    //It ensures the EnemyBase or Breakable must recover by a certain length of time before the player can attack it again.

    //[System.NonSerialized] 

    public float recoveryTime = 1f;
    public float attackCooldown = 1f;
    [System.NonSerialized] public float counter;
    [SerializeField] public float attackCounter;
    [SerializeField] public bool attackRecovering = false;
    [System.NonSerialized] public bool recovering = false;
    private float originalSpeed;

    private void Start()
    {
        if(this.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = this.gameObject.GetComponent<Enemy>();
            // Store the original speed before getting slowed
            //originalSpeed = enemy.maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vi recover når attackCounter er mindre end attackCooldown
        if (attackCounter < attackCooldown)
        {
            attackCounter += Time.deltaTime;
            attackRecovering = true;

            if (this.gameObject.GetComponent<Enemy>() != null)
            {
                //If it's a enemy slow it down when it's recovering
                //Enemy enemy = this.gameObject.GetComponent<Enemy>();

                // Slow down the enemy
                // enemy.maxSpeed = enemy.slowedSpeed;
                // enemy.velocity.x = enemy.slowedSpeed;
            }
        }
        else
        {
            // if (this.gameObject.GetComponent<enemy>() != null)
            // {
            //     //If it's a enemy reset it's speed
            //     enemy enemy = this.gameObject.GetComponent<enemy>();
            //     // Slow down the enemy
            //     enemy.maxSpeed = originalSpeed;
            // }
            attackRecovering = false;
        }


        //Vi recover når counteren er mindre end recoveryTime
        if (counter <= recoveryTime)
        {
            counter += Time.deltaTime;
            recovering = true;

        //     if (this.gameObject.GetComponent<enemy>() != null)
        //     {
        //         //If it's a enemy slow it down when it's recovering
        //         enemy enemy = this.gameObject.GetComponent<enemy>();

        //         // Slow down the enemy
        //         enemy.maxSpeed = enemy.slowedSpeed;
        //         enemy.velocity.x = enemy.slowedSpeed;
        //     }
         }
         else
        // {
        //     if (this.gameObject.GetComponent<enemy>() != null)
             {
        //         //If it's a enemy reset it's speed
        //         enemy enemy = this.gameObject.GetComponent<enemy>();
        //         // Slow down the enemy
        //         enemy.maxSpeed = originalSpeed;
        //     }
            recovering = false;
        }
    }

    public void ResetAttackCounter()
    {
        attackCounter = 0f;
    }

    public void ResetCounter()
    {
        counter = 0f;
    }


}

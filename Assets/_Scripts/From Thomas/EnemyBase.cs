using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyBase : MonoBehaviour
{
    private bool isAttackable = true;
    [SerializeField] public float knockbackForce = 0.2f;
    [SerializeField] private RecoveryCounter recoveryCounter;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 knockbackDirection;
    public bool isDead;

    void Start()
    {
        recoveryCounter = GetComponent<RecoveryCounter>();
    }

    public void Hurt(int damage, Vector2 attackDirection)
    {
        //if (!recoveryCounter.recovering)
        //{
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            if (enemy.stats.health > 1)
            {
                Debug.Log("Enemy took damage: " + damage);
                enemy.stats.health -= damage;
                enemy.anim.SetTrigger("hurt");

                // Apply knockback to the enemy
                enemy.KnockBack();
                Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
                enemyRigidbody?.AddForce(new Vector2(attackDirection.x * knockbackForce, 1), ForceMode2D.Impulse);
                recoveryCounter.ResetCounter();
            }
            else
            {
                if (!isDead)
                {
                    recoveryCounter.ResetCounter();
                    enemy.stats.health = 0;
                    Die();
                }
            }
        }
        else if (gameObject.TryGetComponent(out PlayerController player))
        {
            // Apply knockback to the player
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
            playerRigidbody?.AddForce(new Vector2(attackDirection.x * knockbackForce, 1), ForceMode2D.Impulse);
            player.playerSounds.PlayHurtSound();

            if (player.stats.health > 0 && !PlayerController.Instance.isBlocking)
            {
                player.stats.health -= damage;
                // HUD.Instance.UpdatePlayerHealth(player.stats.health - damage);
                player.anim.SetTrigger("hurt");
                recoveryCounter.ResetCounter();
                if (player.stats.health <= 0)
                {
                    player.stats.health = 0;
                    // HUD.Instance.UpdatePlayerHealth(0);

                    Die();
                }
            }
        }
        else if (gameObject.TryGetComponent(out Companion companion))
        {
            if (companion.stats.health > 1)
            {
                companion.stats.health -= damage;
                // Apply knockback to the player's companion
                Rigidbody2D companionRb = companion.GetComponent<Rigidbody2D>();
                companionRb?.AddForce(new Vector2(attackDirection.x * knockbackForce, 1), ForceMode2D.Impulse);
                companion.KnockBack();
                companion.anim.SetTrigger("hurt");
                recoveryCounter.ResetCounter();
            }
            else
            {
                // if (!isDead)
                // {
                companion.stats.health = 0;
                Die();
                // }
            }
        }
        //}
    }
    public void Die()
    {
        // Logic for dying
        if (GetComponent<Enemy>() != null)
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            //GetComponent<Enemy>().anim.SetTrigger("hurt");
            this.gameObject.GetComponent<Enemy>().anim.SetTrigger("dead");
            this.gameObject.GetComponent<Enemy>().canMove = false;

            isDead = true;
            Destroy(gameObject, 0.5f);

            // If enemy dies and companion is out set companion xp stats and display UI
            if (PlayerController.Instance.companionIsOut)
            {
                int exp = GetComponent<Enemy>().stats.givesXP;
                PlayerController.Instance.companionGameobject.GetComponentInChildren<CompanionUI>().GainExpCompanionUI(exp);
                PlayerController.Instance.companionGameobject.GetComponent<Stats>().GainXp(exp);
            }
        }

        if (GetComponent<PlayerController>() != null)
        {
            //GetComponent<PlayerController>().anim.SetTrigger("hurt");

            this.gameObject.GetComponent<PlayerController>().anim.SetTrigger("dead");
            isDead = true;
            HUD.Instance.ShowDeathMenu();
        }

        if (GetComponent<Companion>() != null)
        {
            // GetComponent<Companion>().anim.SetTrigger("hurt");

            this.gameObject.GetComponent<Companion>().anim.SetTrigger("dead");
            isDead = true;

            PlayerController.Instance.companionIsOut = false;
            this.gameObject.SetActive(false);
        }
    }

    public void Heal(int amount)
    {
        // Logic for healing
    }

    public void SetAttackable(bool attackable)
    {
        isAttackable = attackable;
    }
}
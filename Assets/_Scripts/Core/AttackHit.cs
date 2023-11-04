using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script can be placed on any collider that is a trigger. It can hurt enemies or the player, 
so we use it for both player attacks and enemy attacks. 
*/

public class AttackHit : MonoBehaviour
{
    private enum AttacksWhat { Enemy, PlayerController, Destructible, Companion};
    [SerializeField] private AttacksWhat attacksWhat;
    [SerializeField] private bool oneHitKill;
    [SerializeField] private float startCollisionDelay; //Some enemy types, like EnemyBombs, should not be able blow up until a set amount of time
    private Vector2 targetSide; //Is the attack target on the left or right side of this object?
    [SerializeField] private GameObject parent; //This must be specified manually, as some objects will have a parent that is several layers higher
    [SerializeField] private bool isBomb = false; //Is the object a bomb that blows up when touching the player?
    [SerializeField] private int hitPower = 1;
    public GameObject hitParticlePrefab; // The particle effect to spawn when the arrow hits an enemy
    [SerializeField] private bool isHeavyAttack;

    // Use this for initialization
    void Start()
    {
        if (parent.GetComponent<PlayerController>() != null)
        {
            hitPower = PlayerController.Instance.stats.damage;

            if (isHeavyAttack) hitPower += 2;
        }

        if (parent.GetComponent<Enemy>() != null)
        {
            hitPower = parent.GetComponent<Enemy>().stats.damage;
        }

        if (parent.GetComponent<Companion>() != null)
        {
            hitPower = parent.GetComponent<Companion>().stats.damage;
        }
    }

void OnTriggerStay2D(Collider2D col)
{
    PlayerController player = col.GetComponent<PlayerController>();
    Companion companion = col.GetComponent<Companion>();
    Enemy enemy = col.GetComponent<Enemy>();
    RecoveryCounter recoveryCounter = col.GetComponent<RecoveryCounter>();

    if (attacksWhat == AttacksWhat.PlayerController && (player != null || (companion != null && GetComponentInParent<PlayerController>() == null)))
    {
        if (parent.transform.position.x < col.transform.position.x)
        {
            targetSide.x = 1;
        }
        else
        {
            targetSide.x = -1;
        }

        if (player != null && !recoveryCounter.recovering)
        {
            col.GetComponent<EnemyBase>().Hurt(hitPower, targetSide);
        }

        if (companion != null && !recoveryCounter.recovering)
        {
            col.GetComponent<EnemyBase>().Hurt(hitPower, targetSide);
        }
    }
    else if (attacksWhat == AttacksWhat.Enemy && enemy != null)
    {
        enemy.SetTarget(parent.gameObject);

        if (parent.transform.position.x < col.transform.position.x)
        {
            targetSide.x = 1;
        }
        else
        {
            targetSide.x = -1;
        }

        if (enemy != null && !recoveryCounter.recovering)
        {
             col.GetComponent<EnemyBase>().Hurt(hitPower, targetSide);
        }
    }
}

    //Temporarily disable this collider to ensure bombs can launch from inside enemies without blowing up!
    IEnumerator TempColliderDisable()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(startCollisionDelay);
        GetComponent<Collider2D>().enabled = true;
    }
}

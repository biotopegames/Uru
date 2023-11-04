using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int fullHealth;
    public int health;
    public int damage;
    public string name;
    public int stamina;
    public int maxStamina;
    public int defense;
    public float moveSpeed;
    public int lvl = 1;
    public int currentXp = 0;
    public int xpNeededToLevelUp = 100;
    public int xpPoints;
    public float attackSpeed;
    public float hpRegeneration;
    public int givesXP = 100;
    public string lore;
    private Coroutine hpRegenerationCoroutine;
    public int staminaRegenerationRate = 1;
    private Coroutine staminaRegenerationCoroutine;

    private void Start()
    {
        StartHpRegeneration();
        StartStaminaRegeneration();
        fullHealth = health;
        maxStamina = stamina;
    }

    public void Update(){
        if(stamina < 0){
            stamina = 0;
        }

            if(stamina > maxStamina){
            stamina = maxStamina;
        }
    }

public void PlayerTakeDamage(int dmg)
{
    if(health > 0)
    health -= dmg;
    else
    {
        //DIE
    }
}



public void LevelUp(int leftOverXp)
{
    lvl++;
    xpPoints += 5; // Award points to spend on attributes
    currentXp = leftOverXp; // Set current XP to the leftover XP

    // Increase the XP needed to level up for the next level.
    xpNeededToLevelUp = (int)(xpNeededToLevelUp * 1.2f);
}

public void GainXp(int xpAmount)
{
    currentXp += xpAmount;

    while (currentXp >= xpNeededToLevelUp)
    {
        currentXp -= xpNeededToLevelUp; // Deduct XP needed to level up
        LevelUp(currentXp); // Level up with the leftover XP
    }
}

    public void Heal(int amount)
    {
        health += amount;
        if (health > fullHealth)
        {
            health = fullHealth;
        }
    }

    private void StartHpRegeneration()
    {
        StopHpRegeneration();
        hpRegenerationCoroutine = StartCoroutine(HpRegenerationCoroutine());
    }

    private void StartStaminaRegeneration()
    {
            StopStaminaRegeneration();
            staminaRegenerationCoroutine = StartCoroutine(StaminaRegenerationCoroutine());
    }

    private void StopHpRegeneration()
    {
        if (hpRegenerationCoroutine != null)
        {
            StopCoroutine(hpRegenerationCoroutine);
            hpRegenerationCoroutine = null;
        }
    }

    private void StopStaminaRegeneration()
    {
        if (staminaRegenerationCoroutine != null)
        {
            StopCoroutine(staminaRegenerationCoroutine);
            staminaRegenerationCoroutine = null;
        }
    }

    private IEnumerator StaminaRegenerationCoroutine()
    {
        while (true)
        {
            if(stamina < maxStamina)
            {
            stamina += staminaRegenerationRate;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator HpRegenerationCoroutine()
    {
        Debug.Log("regenerating...");
        while (true)
        {
            if (health < fullHealth)
            {
                health += (int)hpRegeneration;

            }
            yield return new WaitForSeconds(1f);
        }
    }

    public bool SpendStamina(int amount){
        if(stamina > amount){
            stamina -= amount;
            return true;
        }
        return false;
    }
}


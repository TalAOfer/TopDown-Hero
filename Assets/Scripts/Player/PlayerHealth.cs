using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("OnPlayerDamaged")]
    public GameEvent OnPlayerDamaged;
    [Header("OnPlayerDeath")]
    public GameEvent OnPlayerDeath;

    private int maxHealth = 6;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        OnPlayerDamaged.Raise(this, new int[] { maxHealth, currentHealth });
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            OnPlayerDeath.Raise();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnPlayerDamaged.Raise(this, new int[] {maxHealth, currentHealth});
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}

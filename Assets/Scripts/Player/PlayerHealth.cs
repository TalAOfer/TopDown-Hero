using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public GameEvent OnUpdateHealth;
    public GameEvent OnPlayerDamaged;
    public GameEvent OnPlayerDeath;
    
    private bool isDamagable = true;
    [SerializeField] private float hurtCooldown = 1f;

    private int maxHealth = 6;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        OnUpdateHealth.Raise(this, new int[] { maxHealth, currentHealth });
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            OnPlayerDeath.Raise();
        }
    }

    public void TakeDamage(int amount, Vector3 enemyDirection)
    {
        if (isDamagable)
        {
            isDamagable = false;
            currentHealth -= amount;
            OnUpdateHealth.Raise(this, new int[] {maxHealth, currentHealth});
            OnPlayerDamaged.Raise(this, enemyDirection);
            StartCoroutine(EnableDamagable());
        }
    }

    private IEnumerator EnableDamagable()
    {
        yield return new WaitForSeconds(hurtCooldown);
        isDamagable = true;
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

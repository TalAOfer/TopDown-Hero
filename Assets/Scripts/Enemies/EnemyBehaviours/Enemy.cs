using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Enemy_SO EnemyData;

    public bool isDead = false;

    private int maxHealth,
                currentHealth;

    [SerializeField]
    private float knockbackAmount = 2.0f;
    private Vector2 force;

    private Animator anim;
    private Rigidbody2D rb;
    void Start()
    {
        maxHealth = EnemyData.hp;
        currentHealth = EnemyData.hp;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector3 playerPosition)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        anim.SetTrigger("Hurt");
        
        //HitDirection
        Vector2 direction = (this.transform.position - playerPosition).normalized;
        //Apply Knockback
        if (currentHealth > 0)
        {
            force = (direction * knockbackAmount * (damage / 2));
            rb.AddForce(force, ForceMode2D.Impulse);
        } else
        {
            StartCoroutine(Die());
        }
    }



    private IEnumerator Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}

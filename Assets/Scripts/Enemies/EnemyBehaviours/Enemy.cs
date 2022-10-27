using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int MaxHealth;
    private int currentHealth;

    [SerializeField]
    private float knockbackAmount = 2.0f;
    private Vector2 force;

    private Animator anim;
    private Rigidbody2D rb;
    void Start()
    {
        currentHealth = MaxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector3 playerPosition)
    {
        currentHealth -= damage;

        anim.SetTrigger("Hurt");
        
        //HitDirection
        Vector2 direction = (this.transform.position - playerPosition).normalized;
        //Apply Knockback
        force = (direction * knockbackAmount);
        rb.AddForce(force, ForceMode2D.Impulse);

        if (currentHealth < 0) {
            Die();
        }
    }



    private void Die()
    {
        anim.SetBool("isDead", true);

        this.gameObject.SetActive(false);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}

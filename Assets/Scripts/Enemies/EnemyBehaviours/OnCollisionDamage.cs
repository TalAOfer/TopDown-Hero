using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDamage : MonoBehaviour
{
    private Enemy enemy;
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemy) return;
        if (!enemy.isDead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(1, transform.position);
            }
        }
    }
}

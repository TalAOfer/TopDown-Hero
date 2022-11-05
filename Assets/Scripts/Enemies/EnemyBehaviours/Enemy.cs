using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Enemy_SO EnemyData;

    public bool isDead = false;
    private Player player;
    private GameObject Player_GO;

    private EnemyChaseAnimatorHandler chaseAnimatorScript;
    private PatrolEnemy patrolScript;
    private AIPath chaseScript;
    private AIDestinationSetter destinationScript;

    private int maxHealth,
                currentHealth;

    [SerializeField]
    private float knockbackAmount = 2.0f;
    private Vector2 force;

    private Animator anim;
    private Rigidbody2D rb;
    public bool stateLock;

    public enum EnemyStates
    {
        PATROL,
        CHASE,
        HURT
    }

    public EnemyStates CurrentState
    {
        set
        {
            if (!stateLock)
            {

                currentState = value;

                switch (currentState)
                {
                    case EnemyStates.PATROL:
                        patrolScript.enabled = true;
                        chaseAnimatorScript.enabled = false;
                        chaseScript.enabled = false;
                        break;
                    case EnemyStates.CHASE:
                        chaseScript.enabled = true;
                        chaseAnimatorScript.enabled = true;
                        patrolScript.enabled = false;
                        break;
                }
            }

        }
    }

    private EnemyStates currentState;
    void Start()
    {
        maxHealth = EnemyData.hp;
        currentHealth = EnemyData.hp;

        Player_GO = GameObject.FindGameObjectWithTag("Player");
        destinationScript = GetComponent<AIDestinationSetter>();
        chaseAnimatorScript = GetComponent<EnemyChaseAnimatorHandler>();
        patrolScript = GetComponent<PatrolEnemy>();
        chaseScript = GetComponent<AIPath>();
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        destinationScript.target = Player_GO.transform;
        CurrentState = EnemyStates.PATROL;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 5f) {
            CurrentState = EnemyStates.PATROL;
        } else
        {
            CurrentState = EnemyStates.CHASE;
        }
    }

    public void TakeDamage(int damage, Vector3 playerPosition)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        StartCoroutine(Hurt());
        
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

    private IEnumerator Hurt()
    {
        if (isDead)
        {
        Debug.Log("happened");
        }
        chaseScript.canMove = false;
        anim.SetTrigger("Hurt");
        stateLock = true;
        yield return new WaitForSeconds(0.5f);
        chaseScript.canMove = true;
        stateLock = false;
    }
}

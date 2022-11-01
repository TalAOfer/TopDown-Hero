using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player;

public class PlayerCombat : MonoBehaviour
{
    private int currAttack = 1;

    [SerializeField]
    PlayerForm_SO DefaultFormData,
                  OgreFormData;

    private PlayerForm_SO currentData;

    [SerializeField]
    private LayerMask enemyLayers;

    [SerializeField] private float attackResetDelay;
    private float lastAttackEndedIn = 0;
    private bool canAttack = true;

    private Player playerController;
    private string direction = "down";

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRange = 0.5f;

    private Vector2 attackInput;
    private bool isRooted;

    private int playerDamage = 20;

    private Vector3 attackPointRight = new (0.75f, 0, 0),
                    attackPointLeft = new (-0.75f, 0.15f, 0),
                    attackPointUp = new (0, 0.75f, 0),
                    attackPointDown = new (0, -0.75f, 0);

    private void Awake()
    {   
        playerController = gameObject.GetComponent<Player>();
        currentData = DefaultFormData;
        isRooted = currentData.isStuckWhenAttacking;
        playerDamage = currentData.damage;
    }

    void Update()
    {
        if (attackInput.x > 0.1f && attackInput.y == 0) {
            direction = "right";
            attackPoint.transform.localPosition = attackPointRight;
            PerformAttack();
        }

        else if (attackInput.x < -0.1f && attackInput.y == 0)
        {
            direction = "left";
            attackPoint.transform.localPosition = attackPointLeft;
            PerformAttack();
        }

        else if (attackInput.x == 0 && attackInput.y > 0.1f)
        {
            attackPoint.transform.localPosition = attackPointUp;
            direction = "up";
            PerformAttack();
        }

        else if (attackInput.x == 0 && attackInput.y < -0.1f)
        {
            attackPoint.transform.localPosition = attackPointDown;
            direction = "down";
            PerformAttack();
        }
    }

    public void UpdateData(Component sender, object data)
    {
        switch ((string) data)
        {
            case "ogre":
                currentData = OgreFormData;
                break;
            case "default":
                currentData = DefaultFormData;
                break;
        }
        playerDamage = currentData.damage;
        isRooted = currentData.isStuckWhenAttacking;
        canAttack = true;
    }

    private void OnAttack(InputValue value)
    {
        attackInput = value.Get<Vector2>();
    }

    private void PerformAttack()
    {
        //If Enough time passed since last attack, attack
        if (Time.time >= lastAttackEndedIn && canAttack)
        {
            if (isRooted)
            {
                playerController.EnableInputFreeze();
            }

            canAttack = false;
            //perform attack1
            if (currAttack == 1)
            {
                currAttack = 2;
                HandleAttack1Direction();
                //HandleEnemyHits();
            }

            //perform attack2
            else if (currAttack == 2)
            {
                currAttack = 1;
                HandleAttack2Direction();
                //HandleEnemyHits();
            }
        }
    }

    public void HandleEnemyHits()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(playerDamage, transform.position);
        }
    }

    public void HandleAttack1Direction()
    {
        switch(direction)
        {
            case "right":
                playerController.CurrentState = PlayerStates.ATTACK1_RIGHT;
                break;
            case "left":
                playerController.CurrentState = PlayerStates.ATTACK1_LEFT;
                break;
            case "up":
                playerController.CurrentState = PlayerStates.ATTACK1_UP;
                break;
            case "down":
                playerController.CurrentState = PlayerStates.ATTACK1_DOWN;
                break;
        }
    }

    public void HandleAttack2Direction()
    {
        switch (direction)
        {
            case "right":
                playerController.CurrentState = PlayerStates.ATTACK2_RIGHT;
                break;
            case "left":
                playerController.CurrentState = PlayerStates.ATTACK2_LEFT;
                break;
            case "up":
                playerController.CurrentState = PlayerStates.ATTACK2_UP;
                break;
            case "down":
                playerController.CurrentState = PlayerStates.ATTACK2_DOWN;
                break;
        }
    }

    public void EndAttack1()
    {
        playerController.ExitAttackMode();
        canAttack = true;
        lastAttackEndedIn = Time.time;
    }

    public void EndAttack2()
    {
        playerController.ExitAttackMode();
        canAttack = true;
        lastAttackEndedIn = Time.time;
    }

    public void EndAttack()
    {
        playerController.ExitAttackMode();
        canAttack = true;
        lastAttackEndedIn = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

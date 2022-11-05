using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyChaseAnimatorHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private AIPath chaseScript;
    Vector2 direction;
    private Animator anim;
    private Enemy enemy;
    private string prevDirection = "";
    private string currentDirection = "down";
    private float nextAnimationChangeTime;
    private float animationChangeDelay = 0.6f;

    void Start()
    {
        nextAnimationChangeTime = Time.time;
        InitChaserAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
    }

    private void OnEnable()
    {
        InitChaserAnimator();
        HandleAnimation();
    }

    private void InitChaserAnimator()
    {
        chaseScript = GetComponent<AIPath>();
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
    }

    private void CheckDirection()
    {
        if (!chaseScript.canMove && Time.time < nextAnimationChangeTime)
        {
            return;
        }

        direction = chaseScript.desiredVelocity;

        float xAbsoluteVelocity = Mathf.Abs(direction.x);
        float yAbsoluteVelocity = Mathf.Abs(direction.y);

        if (direction.x > 0 && xAbsoluteVelocity >= yAbsoluteVelocity)
        {
            prevDirection = currentDirection;
            currentDirection = "right";
            if (prevDirection !=  currentDirection)
            {
                HandleAnimation();
            }
        }
        else if (direction.x < 0 && xAbsoluteVelocity >= yAbsoluteVelocity)
        {
            prevDirection = currentDirection;
            currentDirection = "left";
            if (prevDirection != currentDirection)
            {
                HandleAnimation();
            }
        }
        else if (direction.y > 0 && yAbsoluteVelocity > xAbsoluteVelocity)
        {
            prevDirection = currentDirection;
            currentDirection = "up";
            if (prevDirection != currentDirection)
            {
                HandleAnimation();
            }
        }
        else if (direction.y < 0 && yAbsoluteVelocity > xAbsoluteVelocity)
        {
            prevDirection = currentDirection;
            currentDirection = "down";
            if (prevDirection != currentDirection)
            {
                HandleAnimation();
            }
        }
    }

    public void HandleAnimation()
    {
        if (enemy.isDead)
        {
            return;
        }

        nextAnimationChangeTime = Time.time + animationChangeDelay;

        switch (currentDirection)
        {
            case "left":
                anim.Play("Walk_Left");
                break;
            case "right":
                anim.Play("Walk_Right");
                break;
            case "up":
                anim.Play("Walk_Up");
                break;
            case "down":
                anim.Play("Walk_Down");
                break;
        }
    }
}

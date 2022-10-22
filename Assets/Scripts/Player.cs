using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerCombat playerCombat;

    private bool stateLock = false;

    private bool canFlip = true;

    public enum PlayerStates
    {
        IDLE,
        WALK,
        ATTACK1,
        ATTACK2
    }

    public PlayerStates CurrentState
    {
        set
        {
        if (!stateLock) { 

            currentState = value;

            switch (currentState) {
                    case PlayerStates.IDLE:
                        anim.Play("Idle");
                        break;
                    case PlayerStates.WALK:
                        anim.Play("Walk");
                        break;
                    case PlayerStates.ATTACK1:
                        anim.Play("Attack_1");
                        stateLock = true;
                        canFlip = false;
                        break;
                    case PlayerStates.ATTACK2:
                        anim.Play("Attack_2");
                        stateLock = true;
                        canFlip = false;
                        break;
                }
            }

        }
    }

    private PlayerStates currentState;

    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void GeneralEndAttack()
    {
        canFlip = true;
        stateLock = false;

        if (moveInput != Vector2.zero)
        {
            UpdateAnimatorMoveInput();
            CurrentState = PlayerStates.WALK;
        }
        else
        {
            CurrentState = PlayerStates.IDLE;
        }
    }

    void OnMove(InputValue value)
    {
        //if is dialogue mode, return
        if (DialogueManager.GetInstance().isDialoguePlaying)
        {
            return;
        }

        moveInput = value.Get<Vector2>();

        if (moveInput != Vector2.zero && canFlip)
        {
            CurrentState = PlayerStates.WALK;
            UpdateAnimatorMoveInput();
        } else
        {
            CurrentState = PlayerStates.IDLE;
        }

    }

    void OnInteract()
    {
        
    }

    public void UpdateAnimatorMoveInput()
    {
        anim.SetFloat("Horizontal", moveInput.x);
        anim.SetFloat("Vertical", moveInput.y);
    }
}

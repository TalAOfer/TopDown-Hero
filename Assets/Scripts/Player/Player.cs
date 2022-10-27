using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("OnExitCamTrigger")]
    public GameEvent OnExitCamTrigger;

    [Header("OnEnterCamTrigger")]
    public GameEvent OnEnterCamTrigger;

    private bool stateLock = false;

    public enum PlayerStates
    {
        IDLE,
        WALK,
        ATTACK1_RIGHT,
        ATTACK1_LEFT,
        ATTACK1_UP,
        ATTACK1_DOWN,
        ATTACK2_RIGHT,
        ATTACK2_LEFT,
        ATTACK2_UP,
        ATTACK2_DOWN
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
                    case PlayerStates.ATTACK1_RIGHT:
                        anim.Play("Player_Attack_1_Right");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK1_LEFT:
                        anim.Play("Player_Attack_1_Left");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK1_UP:
                        anim.Play("Player_Attack_1_Up");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK1_DOWN:
                        anim.Play("Player_Attack_1_Down");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK2_RIGHT:
                        anim.Play("Player_Attack_2_Right");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK2_LEFT:
                        anim.Play("Player_Attack_2_Left");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK2_UP:
                        anim.Play("Player_Attack_2_Up");
                        stateLock = true;
                        break;
                    case PlayerStates.ATTACK2_DOWN:
                        anim.Play("Player_Attack_2_Down");
                        stateLock = true;
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
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void ExitAttackMode()
    {
        stateLock = false;

        if (moveInput != Vector2.zero)
        {
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

        if (moveInput != Vector2.zero)
        {
            CurrentState = PlayerStates.WALK;
            UpdateAnimatorMoveInput();
        } else
        {
            CurrentState = PlayerStates.IDLE;
        }

    }

    public void UpdateAnimatorMoveInput()
    {
        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slider"))
        {
            OnEnterCamTrigger.Raise(this, collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slider"))
        {
            OnExitCamTrigger.Raise(this, transform.position);
        }
    }
}

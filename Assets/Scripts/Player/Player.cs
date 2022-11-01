using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;

    public GameEvent OnExitCamTrigger,
                     OnEnterCamTrigger,
                     OnEnterPortal,
                     OnChangeForm;

    public bool stateLock = false;
    private bool isInputFreeze = false;
    private string currentShapeshift;
    private PlayerForm_SO currentData;

    [SerializeField] PlayerForm_SO DefaultFormData,
                                   OgreFormData;
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
        ATTACK2_DOWN,
        TRANSFORM
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
                    case PlayerStates.TRANSFORM:
                        anim.Play("Player_Transform");
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
        currentShapeshift = "default";
        currentData = DefaultFormData;
        moveSpeed = currentData.speed;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isInputFreeze) {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
            ResetAnimator();
        }
    }

    public void ExitAttackMode()
    {
        stateLock = false;
        DisableInputFreeze();
        
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
        moveInput = value.Get<Vector2>();
    }

    public void UpdateAnimatorMoveInput()
    {
        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
    }

    private void ResetAnimator()
    {
        if (moveInput != Vector2.zero)
        {
            CurrentState = PlayerStates.WALK;
            UpdateAnimatorMoveInput();
        }
        else
        {
            CurrentState = PlayerStates.IDLE;
        }
    }

    void OnShapeshift(InputValue value)
    {
        if (isInputFreeze)
        {
            return;
        }

        CurrentState = PlayerStates.TRANSFORM;

        switch (currentShapeshift)
        {
            case "default":
                currentShapeshift = "ogre";
                currentData = OgreFormData;
                break;
            case "ogre":
                currentShapeshift = "default";
                currentData = DefaultFormData;
                break;
        }

        moveSpeed = currentData.speed;
        EnableInputFreeze();
    }

    public void Shapeshift()
    {
        OnChangeForm.Raise(this, currentShapeshift);
        DisableInputFreeze();
        stateLock = false;
        //Set Player to idle down
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);
    }

    public void EnableInputFreeze()
    {
        isInputFreeze = true;
    }

    public void DisableInputFreeze()
    {
        isInputFreeze = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slider"))
        {

            OnEnterCamTrigger.Raise(this, collision.name);
            EnableInputFreeze();   

        }

        if (collision.gameObject.CompareTag("Portal"))
        {
            OnEnterPortal.Raise(this, collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slider"))
        {
            OnExitCamTrigger.Raise(this, transform.position);
            ResetAnimator();
            DisableInputFreeze();
        }
    }
}

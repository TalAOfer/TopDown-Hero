using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Setup
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerCombat playerCombat;

    [Header("Events")]
    public GameEvent OnExitCamTrigger;
    public GameEvent OnEnterCamTrigger;
    public GameEvent OnEnterPortal;
    public GameEvent OnChangeForm;
    public GameEvent OnExitDash;
    public GameEvent OnExitKnockback;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private string facingDirection;
    
    ////Dash Vars
    private bool isDashing = false;
    private bool canDash = true;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    ////Knockback Vars
    private Vector2 knockbackForce;
    private bool isKnockedback = false;
    [SerializeField] private float knockbackAmount = 2f;
    [SerializeField] private float knockbackTime = 0.2f;


    [Header("Locks")]
    public bool stateLock = false;
    private bool isInputFreeze = false;


    [Header("Shapeshifting")]
    [SerializeField] PlayerForm_SO DefaultFormData;
    [SerializeField] PlayerForm_SO OgreFormData;

    private PlayerForm_SO currentData;
    private string currentShapeshift;
    [SerializeField] private AnimationCurve dashCurve;
    

    //Player States
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
        DASH_RIGHT,
        DASH_LEFT,
        DASH_UP,
        DASH_DOWN,
        HURT_RIGHT,
        HURT_LEFT,
        HURT_UP,
        HURT_DOWN,
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
                    case PlayerStates.DASH_RIGHT:
                        anim.Play("Player_Dash_Right");
                        stateLock = true;
                        break;
                    case PlayerStates.DASH_LEFT:
                        anim.Play("Player_Dash_Left");
                        stateLock = true;
                        break;
                    case PlayerStates.DASH_UP:
                        anim.Play("Player_Dash_Up");
                        stateLock = true;
                        break;
                    case PlayerStates.DASH_DOWN:
                        anim.Play("Player_Dash_Down");
                        stateLock = true;
                        break;
                    case PlayerStates.HURT_RIGHT:
                        anim.Play("Player_Hurt_Right");
                        stateLock = true;
                        break;
                    case PlayerStates.HURT_LEFT:
                        anim.Play("Player_Hurt_Left");
                        stateLock = true;
                        break;
                    case PlayerStates.HURT_UP:
                        anim.Play("Player_Hurt_Up");
                        stateLock = true;
                        break;
                    case PlayerStates.HURT_DOWN:
                        anim.Play("Player_Hurt_Down");
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

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentShapeshift = "default";
        currentData = DefaultFormData;
        moveSpeed = currentData.speed;
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (moveInput.x > 0.1f && moveInput.y == 0)
        {
            facingDirection = "right";
        }

        else if (moveInput.x < -0.1f && moveInput.y == 0)
        {
            facingDirection = "left";
        }

        else if (moveInput.x == 0 && moveInput.y > 0.1f)
        {
            facingDirection = "up";
        }

        else if (moveInput.x == 0 && moveInput.y < -0.1f)
        {
            facingDirection = "down";
        }
    }
    private void FixedUpdate()
    {

        if (!isInputFreeze && !isDashing && !isKnockedback) {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
            ResetAnimator();
        }   
    }

    private IEnumerator Dash()
    {
        canDash = false;
        HandleDashingAnimation();
        isDashing = true;
        rb.velocity = new Vector2(moveInput.x * dashingPower, moveInput.y * dashingPower);
        yield return new WaitForSeconds(dashingTime);
        OnExitDash.Raise();
        isDashing = false;
        stateLock = false;
        rb.velocity = Vector2.zero;
        ResetAnimator();
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }

    private void HandleDashingAnimation()
    {
        switch (facingDirection)
        {
            case "right":
                CurrentState = PlayerStates.DASH_RIGHT;
                break;
            case "left":
                CurrentState = PlayerStates.DASH_LEFT;
                break;
            case "up":
                CurrentState = PlayerStates.DASH_UP;
                break;
            case "down":
                CurrentState = PlayerStates.DASH_DOWN;
                break;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnShapeshift(InputValue value)
    {
        if (isInputFreeze || stateLock)
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

    void OnDash(InputValue value)
    {
        if (canDash && !stateLock)
        {
            StartCoroutine(Dash());
        }
    }

    public void ExitAttackMode()
    {
        if (currentState == PlayerStates.TRANSFORM)
        {
            return;
        }

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

    public void UpdateAnimatorMoveInput(Vector2 input)
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
    }

    private void ResetAnimator()
    {
        if (!stateLock)
        {
            if (moveInput != Vector2.zero)
            {
                CurrentState = PlayerStates.WALK;
                UpdateAnimatorMoveInput(moveInput);

            }
            else
            {
                CurrentState = PlayerStates.IDLE;
            }
        }
    }

    public void ApplyKnockback(Component sender, object data)
    {
        isKnockedback = true;
        Vector3 enemyPosition = (Vector3) data;
        //HitDirection
        Vector2 direction = (transform.parent.position - enemyPosition).normalized;
        Debug.Log(direction);
        //Apply Knockback
        knockbackForce = (direction * knockbackAmount);
        StartCoroutine(Knockback(knockbackForce));
        //rb.velocity = new Vector2 (knockbackForce.x, knockbackForce.y);
    }

    private void HandleHurtAnimation()
    {
        switch (facingDirection)
        {
            case "right":
                CurrentState = PlayerStates.HURT_RIGHT;
                break;
            case "left":
                CurrentState = PlayerStates.HURT_LEFT;
                break;
            case "up":
                CurrentState = PlayerStates.HURT_UP;
                break;
            case "down":
                CurrentState = PlayerStates.HURT_DOWN;
                break;
        }
        
    }

    private IEnumerator Knockback(Vector2 knockbackForce)
    {
        HandleHurtAnimation();
        isKnockedback = true;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackTime);
        OnExitKnockback.Raise();
        isKnockedback = false;
        stateLock = false;
        rb.velocity = Vector2.zero;
        ResetAnimator();
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

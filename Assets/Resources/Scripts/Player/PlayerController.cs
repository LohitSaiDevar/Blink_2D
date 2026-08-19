using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;


[System.Serializable]
public class CheatSettings
{
    public bool unlockBlink;
    public bool unlockDash;
    public bool unlockDarkVision;
}

public class PlayerController : MonoBehaviour
{
    PlayerPhysics2D Physics { get; set; }
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D playerCollider;
    CapsuleCollider2D capsuleCollider;

    [Header("Movement settings")]
    [SerializeField] float frictionValue;
    float horizontalInput;
    float verticalVelocity;
    [SerializeField] float maxVerticalSpeed;
    int facingDir = 1;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float groundAcceleration;
    [SerializeField] float airAcceleration;

    [Header("Jump settings")]
    [SerializeField] float jumpForce;
    bool canJump;

    [Header("Dash settings")]
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashTimeDuringBarrier;
    [SerializeField] float barrierDashCooldownTime;
    [SerializeField] float groundedDashCooldownTime;
    [SerializeField] float airDashCooldownTime;
    bool canDash;
    bool isDashing;
    bool dashOnCooldown;

    [Header("Death settings")]
    bool isDead;

    [Header("Animation")]
    public Animator animator;
    private string currentAnimationState;

    public const string Player_Idle = "Idle";
    public const string Player_Run = "Run";
    public const string Player_Crouch = "Crouch";
    public const string Player_CrouchWalk = "CrouchWalk";
    public const string Player_Attack = "Attack";
    public const string Player_Throw = "Throw";
    public const string Player_Jump = "Jump";
    public const string Player_Fall = "Fall";
    public const string Player_Land = "Land";
    public const string Player_Dash = "Dash";
    public const string Player_LedgeHang = "LedgeHang";
    public const string Player_Climb = "LedgeClimb";
    public const string Player_Hurt = "Hurt";
    public const string Player_Death = "Death";

    [Header("State Machine")]
    public PlayerFSM stateMachine;
    [SerializeField] PlayerStateID initialState;

    [Header("Crouch Settings")]
    [SerializeField] private Vector2 crouchColliderOffset;
    [SerializeField] private Vector2 crouchColliderSize;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private float ceilingCheckRadius;
    bool isCrouching;
    [SerializeField] float crouchWalkSpeed;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform liftCheck;
    [SerializeField] private Transform barrierCheck;

    [Header("Ground settings")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall settings")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask wallLayer;

    [Header("Barrier settings")]
    RaycastHit2D barrierHit;
    [SerializeField] private float barrierEnableTimer;
    [SerializeField] private float barrierCheckDistance;
    [SerializeField] private LayerMask barrierLayer;

    [Header("Ledge Hang settings")]
    bool isHanging;
    [SerializeField] private float ledgeGrabRayDistance;
    [SerializeField] private LayerMask ledgeLayer;
    [SerializeField] private Vector2 hangOffsetPos;

    [Header("Ledge Climb settings")]
    bool isClimbing;
    [SerializeField] private Vector2 afterClimbOffsetPos;

    [Header("Aim Settings")]
    bool isAiming;
    float currentAimAngle;
    [SerializeField] Transform aimTransform;
    [SerializeField] float leftSideMaxAngle = -135f;
    [SerializeField] float rightSideMaxAngle = 135f;
    [SerializeField] float rotationOffset = -90f;

    [Header("Throw Settings")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float throwForce;
    [SerializeField] float verticalOffset = 0.5f;
    bool readyToThrow;
    Transform ballPos;

    [Header("Blink Mechanic")]
    
    bool isBlinking;
    
    [SerializeField] float teleportOffset = 0.5f;

    [Header("Dark Vision settings")]
    bool isDarkVisionOn;

    [Header("Lift Settings")]
    bool isPlayerOnLift;
    [SerializeField] float liftCheckRadius;
    [SerializeField] LayerMask liftLayer;
    

    [Header("Camera Pan settings")]
    [SerializeField] Transform cameraTarget;
    [SerializeField] float panDistance = 2;
    [SerializeField] float panSpeed = 5;

    [Header("Action events")]
    public static Action OnLiftUsed;
    public static Action<Transform> OnBlinkUsed;
    public static Action OnBallThrown;
    public static Action OnPlayerJump;
    public static Action OnPlayerDash;
    public static Action OnPlayerHurt;
    public static Action OnPlayerDeath;
    public static Action OnMoving;
    public static Action<Collider2D> OnPlayerBarrierDash;

    [Header("Orb of Power Settings")]
    bool blinkOrbSelected;
    bool dashOrbSelected;
    bool darkOrbSelected;

    [Header("Interaction Settings")]
    Tutorial currentTutorial;
    bool nearOrb;
    bool nearTutorial;
    bool nearNPC;
    bool isInteracting;

    OrbOfPower currentOrb;

    [SerializeField] CheatSettings cheats;
    public float HorizontalInput => horizontalInput;
    public float FrictionValue => frictionValue;

    public float VerticalVelocity => verticalVelocity;
    public bool IsClimbing { get => isClimbing; set => isClimbing = value; }
    public bool IsHanging { get => isHanging; set => isHanging = value; }
    public bool CanDash { get => canDash; set => canDash = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool IsAiming { get => isAiming; set => isAiming = value; }
    public bool ReadyToThrow { get => readyToThrow; set => readyToThrow = value; }
    public bool IsBlinking { get => isBlinking; set => isBlinking = value; }
    public bool IsCrouching { get => isCrouching; set => isCrouching = value; }
    public bool IsPlayerOnLift { get => isPlayerOnLift; set => isPlayerOnLift = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool CanJump { get => canJump; set => canJump = value; }
    void Start()
    {
        RegisterAllStates();
        InitializeBoxCollider();
        Physics = PlayerPhysics2D.Instance;
        canDash = true;
        canJump = true;

        if (cheats.unlockBlink)
        {
            UnlockBlink();
        }

        if (cheats.unlockDash)
        {
            UnlockDash();
        }

        if (cheats.unlockDarkVision)
        {
            UnlockDarkVision();
        }
    }

    private void OnEnable()
    {
        //JUMP
        OnPlayerJump += StartJump;

        //DEATH
        OnPlayerDeath += StopMoving;
        OnPlayerDeath += SwitchOffCollider;

        //RESPAWN
        GameManager.OnPlayerRespawn += ResetPlayer;

        //THROW
        OnBallThrown += LaunchBall;

        //BLINK
        OnBlinkUsed += BlinkPlayer;

        //DASH
        OnPlayerDash += PerformDash;
        OnPlayerBarrierDash += PerformDashThroughBarrier;
    }

    private void OnDisable()
    {
        //JUMP
        OnPlayerJump -= StartJump;

        //DEATH
        OnPlayerDeath -= StopMoving;
        OnPlayerDeath -= SwitchOffCollider;

        //RESPAWN
        GameManager.OnPlayerRespawn -= ResetPlayer;

        //THROW
        OnBallThrown -= LaunchBall;

        //BLINK
        OnBlinkUsed -= BlinkPlayer;

        //DASH
        OnPlayerDash -= PerformDash;
        OnPlayerBarrierDash -= PerformDashThroughBarrier;
    }
    // Update is called once per frame
    void Update()
    {
        //if (isDashing) return;
        stateMachine.Update();

        if (Cursor.visible)
        {
            isInteracting = true;
        }

        if(!Cursor.visible)
        {
            isInteracting = false;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        
        verticalVelocity = rb.linearVelocity.y;
        TryLedgeGrab();
        CapVerticalVelocity();
    }
    void RegisterAllStates()
    {
        stateMachine = new PlayerFSM(this);
        stateMachine.RegisterState(new Player_IdleState());
        stateMachine.RegisterState(new Player_MoveState());
        stateMachine.RegisterState(new Player_CrouchState());
        stateMachine.RegisterState(new Player_CrouchWalkState());
        stateMachine.RegisterState(new Player_JumpState());
        stateMachine.RegisterState(new Player_AttackState());
        stateMachine.RegisterState(new Player_FallState());
        stateMachine.RegisterState(new Player_LandState());
        stateMachine.RegisterState(new Player_DashState());
        stateMachine.RegisterState(new Player_LedgeHangState());
        stateMachine.RegisterState(new Player_ClimbState());
        stateMachine.RegisterState(new Player_HurtState());
        stateMachine.RegisterState(new Player_DeathState());
        stateMachine.RegisterState(new Player_AimState());
        stateMachine.RegisterState(new Player_ThrowState());

        stateMachine.ChangeState(initialState);
    }
    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return;
        animator.Play(newAnimationState);
        currentAnimationState = newAnimationState;
        //Debug.Log("Current Animation: " +  newAnimationState);
    }

    public bool HasAnimationEnded(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;
    }
    void Flip(float moveX)
    {
        if (isClimbing || isDashing || isHanging || isDead) return;
        if (moveX == 0) return;
        facingDir = moveX > 0 ? 1 : -1;
        spriteRenderer.flipX = facingDir == -1;
    }
    public void ChangeGravityScale(float value)
    {
        rb.gravityScale = value;
    }
    #region Move
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isClimbing || isDashing || isHanging || isAiming || isDead || isInteracting) return;
        horizontalInput = context.ReadValue<Vector2>().x;
        Flip(horizontalInput);
        //Debug.Log("Working");
    }

    public void ApplyFriction()
    {
        PlayerPhysics2D.Instance.ApplyFriction(rb, frictionValue, GroundDetected());
    }

    public void StartMoving()
    {
        float speed;

        speed = isCrouching ? crouchWalkSpeed : moveSpeed;
        Physics.Move(rb, horizontalInput, speed, GroundDetected(), groundAcceleration, airAcceleration);
    }

    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        horizontalInput = 0;
    }

    public void SwitchOffCollider()
    {
        capsuleCollider.enabled = false;
    }

    public void SwitchOnCollider()
    {
        capsuleCollider.enabled = true;
    }
    #endregion

    #region Crouch

    void InitializeBoxCollider()
    {
        capsuleCollider = playerCollider as CapsuleCollider2D;

        if (capsuleCollider != null)
        {
            originalColliderSize = capsuleCollider.size;
            originalColliderOffset = capsuleCollider.offset;
        }
        else
        {
            Debug.LogError("Player collider is not a CapsuleCollider2D!");
        }
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (isClimbing || isDashing || isHanging || isInteracting) return;
        if (context.performed && GroundDetected())
        {
            if (!isCrouching)
            {
                stateMachine.ChangeState(PlayerStateID.Crouch);
                isCrouching = true;
                //Debug.Log("Working");
            }
            else
            {
                if (CeilingDetected()) return;
                DisableCrouchCollider();
                stateMachine.ChangeState(PlayerStateID.Idle);
                isCrouching = false;
            }
        }
    }

    public void EnableCrouchCollider()
    {
        if (capsuleCollider == null) return;
        capsuleCollider.size = crouchColliderSize;
        capsuleCollider.offset = crouchColliderOffset;
    }

    public void DisableCrouchCollider()
    {
        if (capsuleCollider == null) return;
        capsuleCollider.size = originalColliderSize;
        capsuleCollider.offset = originalColliderOffset;
    }

    public bool CeilingDetected()
    {
        // We can reuse the groundLayer mask since ceilings are usually just the bottom of ground tiles
        return UnityEngine.Physics2D.OverlapCircle(ceilingCheck.position, groundCheckRadius, groundLayer);
    }
    #endregion

    #region Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isCrouching || isDead || !canJump || isHanging || isInteracting) return;
        if (context.performed && GroundDetected())
        {
            stateMachine.ChangeState(PlayerStateID.Jump);
            //Debug.Log("Working");
        }
    }

    public void StartJump()
    {
        Physics.Jump(rb, jumpForce);
    }

    public void ResetCanJump()
    {
        canJump = true;
    }

    void CapVerticalVelocity()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxVerticalSpeed));
    }
    #endregion

    #region Ledge Hang and Climb
    public void ApplyLedgeHang()
    {
        StopMoving();

        ChangeGravityScale(0);
        playerCollider.enabled = false;
    }
    public void TryLedgeGrab()
    {
        if (isHanging || isClimbing || isDead) return;
        if (!GroundDetected() && WallDetected() && !LedgeNotDetected())
        {
            //Debug.Log("Ledge Grabbed");
            stateMachine.ChangeState(PlayerStateID.LedgeHang);
        }
    }

    public void SnapToLedge()
    {
        Vector3 hangOffset = new Vector3(hangOffsetPos.x * facingDir, hangOffsetPos.y, 0);
        transform.position += hangOffset;
    }
    public void OnClimbLedge(InputAction.CallbackContext context)
    {
        if(context.performed && isHanging)
        {
            isClimbing = true;
            stateMachine.ChangeState(PlayerStateID.Climb);
        }
    }

    public void FinishClimb()
    {
        Vector3 climbOffset = new Vector3(afterClimbOffsetPos.x * facingDir, afterClimbOffsetPos.y, 0);
        transform.position += climbOffset;
        horizontalInput = 0;
        StopMoving();
        ChangeGravityScale(3);
        playerCollider.enabled = true;
    }

    public void OnLedgeDrop(InputAction.CallbackContext context)
    {
        if(context.performed && isHanging)
        {
            isHanging = false;
            DropFromLedge();
            stateMachine.ChangeState(PlayerStateID.Fall);
        }
    }

    public void DropFromLedge()
    {
        ChangeGravityScale(3);
        playerCollider.enabled = true;
    }
    #endregion

    #region Dash
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || !canDash || isDashing || isCrouching || dashOnCooldown || isDead || !dashOrbSelected || isInteracting) return;
        if (rb.linearVelocity == Vector2.zero) return;
        canDash = false;
        stateMachine.ChangeState(PlayerStateID.Dash);
    }

    public void PerformDash()
    {
        StartCoroutine(Physics.Dash(
                            rb,
                            trailRenderer,
                            dashSpeed,
                            dashTime,
                            GroundDetected() ? groundedDashCooldownTime : airDashCooldownTime,
                            facingDir,
                            OnDashEnd,
                            OnDashReset)
                        );
    }

    public void PerformDashThroughBarrier(Collider2D barrier)
    {
        StartCoroutine(Physics.Dash(
                            rb,
                            trailRenderer,
                            dashSpeed,
                            dashTimeDuringBarrier,
                            barrierDashCooldownTime,
                            facingDir,
                            OnDashEnd,
                            OnDashReset)
                        );
    }
    void OnDashEnd()
    {
        isDashing = false;
    }

    void OnDashReset()
    {
        canDash = true;
    }

    #endregion

    #region Aim and Throw

    public void OnAiming(InputAction.CallbackContext context)
    {
        if(isBlinking || isHanging || isCrouching || isDead || !blinkOrbSelected || isInteracting) return;
        if (context.performed)
        {
            if (!isAiming)
            {
                StopMoving();
                stateMachine.ChangeState(PlayerStateID.Aim);
            }
            else
            {
                stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }
    }

    public void PerformAim()
    {
        aimTransform.gameObject.SetActive(true);
        //Debug.Log("Aiming...");
        Vector3 mouseWorldPos = PlayerUtils.Instance.GetMouseWorldPos();
        Vector3 aimDirection = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        bool isValidAngle = angle >= rightSideMaxAngle || angle <= leftSideMaxAngle;

        if (isValidAngle)
        {
            currentAimAngle = angle;
        }

        float adjustedAngle = currentAimAngle + rotationOffset;

        aimTransform.eulerAngles =
            new Vector3(0, 0, adjustedAngle);

        // Flip the character based on the angle
        if (aimDirection.x < 0)
        {
            Flip(-1);
        }
        else
        {
            Flip(1);
        }

        aimTransform.eulerAngles = new Vector3(0, 0, adjustedAngle);
        //Debug.Log("Aiming at angle: " + angle);

        Vector3 targetPanPos = aimDirection * panDistance;
        cameraTarget.localPosition = Vector3.Lerp(cameraTarget.localPosition, targetPanPos, Time.deltaTime * panSpeed);
    }

    public void ResetCameraPan()
    {
        if (cameraTarget.localPosition != Vector3.zero)
        {
            if(ballPos != null)
            {
                cameraTarget.SetParent(transform);
            }
            cameraTarget.localPosition = Vector3.Lerp(cameraTarget.localPosition, Vector3.zero, Time.deltaTime * panSpeed);
        }
    }
    public void StopAiming()
    {
        aimTransform.gameObject.SetActive(false);
        isAiming = false;
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if(isBlinking || isHanging || isDead || !blinkOrbSelected) return;
        if (context.performed && readyToThrow)
        {
            stateMachine.ChangeState(PlayerStateID.Throw);
        }
    }

    public void LaunchBall()
    {
        float currentAngle = aimTransform.eulerAngles.z - rotationOffset;
        float radians = currentAngle * Mathf.Deg2Rad;

        Vector3 aimDir = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);

        
        Vector3 spawnPos = aimTransform.position + new Vector3(0, verticalOffset, 0);
        GameObject ball = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        ballRb.AddForce(aimDir * throwForce, ForceMode2D.Impulse);
        ballPos = ball.transform;
        CamPosShiftToBall();
    }

    public void CamPosShiftToBall()
    {
        cameraTarget.SetParent(ballPos);
    }
    public void OnBlink(InputAction.CallbackContext context)
    {
        if (context.performed && ballPos != null)
        {
            BlinkPlayer(ballPos);
            //Debug.Log("Blink used at position: " + ballPos.position);
        }
    }
    public void BlinkPlayer(Transform ballPos)
    {
        //Debug.Log("Blinking to ball position: " + ballPos.position);
        if (ballPos.gameObject.GetComponent<Blink>() == null) return;

        Blink ball = ballPos.gameObject.GetComponent<Blink>();
        Vector2 teleportPos = ballPos.position;
        if (ball.ContactPoint.collider != null) // Adjust the offset as needed
        {
            teleportPos = (Vector2)ballPos.position + ball.BlinkOffset(teleportOffset);
        }
        transform.position = teleportPos;
        isBlinking = false;
        isAiming = false;
        Destroy(ballPos.gameObject);
        ballPos = null;
        ResetCameraPan();
    }
    #endregion

    #region Death

    void PlayerDies()
    {
        stateMachine.ChangeState(PlayerStateID.Death);
    }
    public void ResetPlayer()
    {
        stateMachine.ChangeState(initialState);
        SwitchOnCollider();
        ChangeGravityScale(3);
    }
    #endregion

    #region Dark Vision

    public void OnEnableDarkVision(InputAction.CallbackContext context)
    {
        if (!darkOrbSelected || isInteracting) return;
        if (context.performed && !isDarkVisionOn)
        {
            SetDarkObjects(true);
            Debug.Log("Dark Vision ON");
            isDarkVisionOn = true;
        }
        else if (context.performed && isDarkVisionOn)
        {
            SetDarkObjects(false);
            Debug.Log("Dark Vision OFF");
            isDarkVisionOn = false;
        }
    }

    void SetDarkObjects(bool value)
    {
        DarkObject[] darkObjects = FindObjectsByType<DarkObject>(
                                    FindObjectsInactive.Include,
                                    FindObjectsSortMode.None);

        foreach (DarkObject darkObj in darkObjects)
        {
            darkObj.gameObject.SetActive(value);
        }
    }

    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawRay(ledgeCheck.position, Vector2.right * facingDir * ledgeGrabRayDistance);
        Gizmos.DrawRay(wallCheck.position, Vector2.right * facingDir * wallCheckDistance);
        Gizmos.DrawRay(barrierCheck.position, Vector2.right * facingDir * barrierCheckDistance);
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
    }

    #region Checks
    public bool GroundDetected()
    {
        return UnityEngine.Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool WallDetected()
    {
        return UnityEngine.Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, wallLayer);
    }

    public Collider2D GetDetectedBarrier()
    {
        RaycastHit2D hit = Physics2D.Raycast(barrierCheck.position, Vector2.right * facingDir, barrierCheckDistance, barrierLayer);
        //Debug.Log("Barrier detected: " + hit.collider);
        return hit.collider;
    }

    public bool LedgeNotDetected()
    {
        return UnityEngine.Physics2D.Raycast(ledgeCheck.position, Vector2.right * facingDir, ledgeGrabRayDistance, ledgeLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            PlayerDies();
        }
    }
    #endregion

    #region Orb of Power

    

    public void UnlockBlink()
    {
        blinkOrbSelected = true;
    }

    public void UnlockDash()
    {
        dashOrbSelected = true;
    }

    public void UnlockDarkVision()
    {
        darkOrbSelected = true;
    }


    #endregion

    #region Interact

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (nearOrb && currentOrb != null)
        {
            currentOrb.GrantPower(this);
        }

        if (nearTutorial)
        {
            UIManager.Instance.ToggleTutorialScreen(currentTutorial.GetTutorialGameObj(), true);
        }
    }

    #endregion


    #region Trigger Events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Orb"))
        {
            OrbOfPower orb = collision.GetComponent<OrbOfPower>();

            if (orb != null)
            {
                currentOrb = orb;
                nearOrb = true;
            }
        }

        if (collision.CompareTag("Tutorial"))
        {
            currentTutorial = collision.GetComponent<Tutorial>();
            nearTutorial = true;
            Debug.Log("Current Tutorial: " + currentTutorial.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Orb"))
        {
            OrbOfPower orb = collision.GetComponent<OrbOfPower>();
            nearOrb = false;
            if (orb != null && currentOrb == orb)
            {
                currentOrb = null;
            }
        }

        if (collision.CompareTag("Tutorial"))
        {
            nearTutorial = false;
            currentTutorial = null;
        }
    }
    #endregion
}

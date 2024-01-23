
using System;
using System.Collections;
using CharacterMovement;
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterPlayerController))]
public class MovementController : SceneSingleton<MovementController>
{
    #region Events

    public static event Action<bool> OnPlayerRun;
    public static event Action<bool> OnPlayerIdle;
    public static event Action OnPlayerJump;
    public static event Action OnPlayerJumpFromJumpad;
    public static event Action OnPlayerDoubleJump;
    public static event Action<bool> OnPlayerJumpCancel;
    public static event Action OnPlayerSlide;
    public static event Action<bool> OnPlayerSlideCancel;
    public static event Action OnPlayerClimbLedge;
    public static event Action OnPlayerFall;
    public static event Action<bool> OnPlayerFallCancel;
    public static event Action OnPlayerWallHang;
    public static event Action OnPlayerWallJump;
    public event Action OnPlayerFinish;

    #endregion

    #region Sfx events

    public event Action OnPlayLedgeClimbSfx;
    public event Action OnPlayJumpSfx;

    #endregion

    #region Actions

    private ClimbAction _climbAction;
    private HaltAction _haltAction;
    private FlyAction _flyAction;
    private SwimAction _swimAction;
    private GlideAction _glideAction;
    private RopeHangAction _ropeHangAction;
    private MissileDeflectAction _missileDeflectAction;

    #endregion

    [BoxGroup("Enable Movement Trigger"), SerializeField]
    private bool _enableMovementTrigger;

    [Header("Jumping"), SerializeField] private float _maxJumpHeight = 1f;
    [SerializeField] private float _maxJumpTime = 0.5f;

    [Header("Jump prediction"), SerializeField]
    private float _maxGroundJumpDistance = 1f;

    [SerializeField] private float _jumpPredictionDuration = 0.15f;
    [SerializeField] private float _wallJumpPredictionDuration = 0.3f;
    [SerializeField] private float _ladderJumpPredictionDuration = 0.4f;

    [Header("Tutorial"), SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canDoubleJump = true;

    [Header("Falling"), SerializeField] private float _fallClampSpeed = -20f;
    [SerializeField] private float _freeFallMultiplier = 1.5f;

    [Header("Move Speed"), SerializeField] private float _movementSpeed = 5.0f;

    [Header("Slide Speed"), SerializeField]
    private float _slideSpeedMultiplier = 3.0f;

    [SerializeField] private float _additionalSlideJumpForce = 5f;
    [SerializeField] private float _slideSpeed;
    [SerializeField] private float _minAngleSlide;

    [Header("Rotation Speed"), SerializeField]
    private float rotationFactorPerFrame = 1.0f;

    [Header("Wall jump skip check duration"), SerializeField]
    private float _wallJumpSkipCheckDuration = 0.2f;

    [Header("Wall layer for wall jump"), SerializeField]
    private LayerMask _wallLayer;

    [Header("Grounded layer for fall check"), SerializeField]
    private LayerMask _groundedLayers;

    [Header("Grounded layer for fall check"), SerializeField]
    private Transform _groundCheckOriginTransform;

    [Header("Grounded layer for preventing getting stuck while falling check"), SerializeField]
    private LayerMask _groundedLayersOnly;

    [Header("Gravity multipliers"), SerializeField]
    private float _hangingFallMultiplier = 0.35f;

    [Header("Speed momentum multiplier"), SerializeField]
    private float _moveSpeedMomentumMultiplier;

    [SerializeField] private float _slideSpeedMomentumMultiplier;

    [Header("Freeze position"), SerializeField]
    private bool _freezeXPosition;

    [BoxGroup("Movement Reduce Factors"), Range(1f, 5f), SerializeField]
    private float _jumpadMovementReductionFactor = 2f;

    [BoxGroup("Movement Reduce Factors"), Range(1f, 5f), SerializeField]
    private float _ledgeGrabMovementReductionFactor = 2f;

    [BoxGroup("Respawn"), SerializeField] private float _disableFunctionalityOnRespawnDuration = 1.25f;

    [SerializeField] private bool _freezeYPosition;
    [SerializeField] private bool _freezeZPosition;

    #region Private variable declaration

    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private AlignToSurface _alignToSurface;
    private Tween _speedTween;
    private Tween _ignoreFallTween;
    private Tween _tweenJumpad;
    private Tween _queueJumpTween;
    private Tween _queueWallJumpTween;
    private Tween _grabLedgeTween;
    private Tween _queueLadderJumpTween;

    private Vector2 _currMovementInput;
    private Vector3 _currMovement;
    private Vector3 _currSlideMovement;
    private Vector3 _appliedMovement;
    private Vector3 _startingRotation;
    private RaycastHit _slopeHit;
    private bool _isMovementPressed;
    private bool _isJumpPressed;
    private bool _isDoubleJumpPressed;
    private float _initialJumpVelocity;
    private float _initialLedgeJumpVelocity;
    private bool _isJumping;
    private bool _isDoubleJumping;
    private int _jumpCount;
    private bool _isSlidePressed;
    private bool _isSliding;
    private float _currSpeed;
    private float _currFallClampedSpeed;
    private bool _isGrabbingLedge;
    private bool _isFalling;
    private bool _ignoreFall;
    private bool _isWallHanging;
    private bool _isWallJumping;
    private float _wallJumpSkipCheckTimer;
    private bool _isMoveSpeedSetToRun;
    private bool _isMoveSpeedSetToSlide;
    private float _desiredMoveSpeed;
    private float _lastDesiredMoveSpeed;
    private float _startZPosition;
    private bool _jumpFromClimbing;
    private bool _isJumpingFromJumpad;
    private bool _isRunStarted;
    private bool _queueJump;
    private bool _queueWallJump;
    private bool _queueLadderJump;
    private bool _allowLadderJumping;
    private bool _disableFunctionality;

    #endregion

    #region Constants

    private float _gravity = -9.8f;
    private float _groundedGravity = -0.5f;

    #endregion

    #region Getters and Setters

    public float Direction => _appliedMovement.x;

    public ClimbAction ClimbAction => _climbAction;
    public HaltAction HaltAction => _haltAction;
    public GlideAction GlideAction => _glideAction;
    public MissileDeflectAction MissileDeflectAction => _missileDeflectAction;

    public bool CanJump
    {
        get => _canJump;
        set => _canJump = value;
    }

    public bool CanDoubleJump
    {
        get => _canDoubleJump;
        set => _canDoubleJump = value;
    }

    public bool IsFalling
    {
        get => _isFalling;
        set => _isFalling = value;
    }

    public bool IsGrabbingLedge
    {
        get => _isGrabbingLedge;
        set => _isGrabbingLedge = value;
    }

    public bool IsSliding
    {
        get => _isSliding;
        set => _isSliding = value;
    }

    public bool IgnoreFall
    {
        get => _ignoreFall;
        private set
        {
            if (_ignoreFallTween != null) _ignoreFallTween.Kill();
            _ignoreFall = value;
            if (_ignoreFall)
            {
                _ignoreFallTween = DOVirtual.Float(_maxJumpTime, 0f, _maxJumpTime, f => { })
                    .OnComplete(() => { _ignoreFall = false; });
            }
        }
    }

    public float CurrSpeed => _currSpeed;
    public float MovementSpeed => _movementSpeed;
    public float InitialJumpVelocity => _initialJumpVelocity;
    private float SlideSpeedMinLimit => _movementSpeed + _movementSpeed * 0.5f;

    public bool IsJumping
    {
        get => _isJumping;
        set => _isJumping = value;
    }

    public bool IsDoubleJumping
    {
        get => _isDoubleJumping;
        set => _isDoubleJumping = value;
    }

    public bool IsJumpPressed
    {
        get => _isJumpPressed;
        set => _isJumpPressed = value;
    }

    public bool IsDoubleJumpPressed
    {
        get => _isDoubleJumpPressed;
        set => _isDoubleJumpPressed = value;
    }

    public bool IsRunStarted
    {
        get => _isRunStarted;
        set => _isRunStarted = value;
    }

    public bool IsWallHanging => _isWallHanging;

    public CharacterController CharacterController => _characterController;

    public RopeHangAction RopeHangAction => _ropeHangAction;

    public bool IsSlidingOrSlowingFromSlide =>
        (IsSliding || (_currSpeed > SlideSpeedMinLimit) && !_isJumping && IsGroundedRay && !IsFloating);

    public bool IsMovementPressed => _isMovementPressed;

    public bool DisableFunctionality => _disableFunctionality;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _climbAction = GetComponent<ClimbAction>();
        _haltAction = GetComponent<HaltAction>();
        _flyAction = GetComponent<FlyAction>();
        _swimAction = GetComponent<SwimAction>();
        _glideAction = GetComponent<GlideAction>();
        _ropeHangAction = GetComponent<RopeHangAction>();
        _missileDeflectAction = GetComponent<MissileDeflectAction>();
        _alignToSurface = GetComponentInChildren<AlignToSurface>();
        _startingRotation = transform.localRotation.eulerAngles;
        SetupJumpVariables();
        _currSpeed = _movementSpeed;
        _startZPosition = transform.position.z;
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Jump.started += OnJump;
        //_playerInput.CharacterControls.Jump.canceled += OnJump;

        MobileInputManager.Instance.OnPressRightSideOfScreen += MobileInputManager_OnPressRightSideOfScreen;

        LedgeChecker.OnLedgeEnter += OnClimbLedge;
        JumpRefiller.OnAnyCollectJumpRefiller += JumpRefiller_OnAnyCollectJumpRefiller;
        CharacterPlayerController.OnPlayerDied += ResetMovement;
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        _ropeHangAction.OnGrabRope += RopHangAction_OnGrabRope;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable damageableBoss)
            damageableBoss.OnDie += DamageableBoss_OnDie;
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
        LedgeChecker.OnLedgeEnter -= OnClimbLedge;
        JumpRefiller.OnAnyCollectJumpRefiller -= JumpRefiller_OnAnyCollectJumpRefiller;
        CharacterPlayerController.OnPlayerDied -= ResetMovement;
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        _ropeHangAction.OnGrabRope -= RopHangAction_OnGrabRope;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable damageableBoss)
            damageableBoss.OnDie -= DamageableBoss_OnDie;

        if (MobileInputManager.Instance == null) return;
        MobileInputManager.Instance.OnPressRightSideOfScreen -= MobileInputManager_OnPressRightSideOfScreen;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        if (_disableFunctionality) return;
        if (CustomTransistion.Instance != null && !CustomTransistion.Instance.IsTransitionFinished) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        if (_isWallHanging || _isWallJumping) return;

        _currMovementInput = context.ReadValue<Vector2>();

        ApplyMovementSpeed();
        _isMovementPressed = _currMovementInput.x != 0f || _currMovementInput.y != 0f;

        if (_enableMovementTrigger)
        {
            _isMovementPressed = true;
            _currMovementInput = new Vector2(1f, 0f);
            ApplyMovementSpeed();
        }

        if (_isJumping || IsSliding || _isGrabbingLedge || ClimbAction.IsClimbing) return;
        InvokePlayerRun();
        OnPlayerIdle?.Invoke(_isMovementPressed);
    }

    public void ApplyMovementSpeed()
    {
        _currMovement.x = _freezeXPosition ? 0f : _currMovementInput.x;
        _currMovement.z = _freezeZPosition ? 0f : _currMovementInput.y;
        _currSlideMovement.x = _freezeXPosition ? 0f : _currMovementInput.x * _slideSpeedMultiplier;
        _currSlideMovement.z = _freezeZPosition ? 0f : _currMovementInput.y * _slideSpeedMultiplier;
    }

    private void SetupJumpVariables()
    {
        // parabola formula
        // resource: https://www.youtube.com/watch?v=h2r3_KjChf4 from time: 10:00
        float timeToApex = _maxJumpTime / 2f;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        _initialLedgeJumpVelocity = ((2.2f * _maxJumpHeight) / timeToApex) / 1f;
    }

    private void OnJump(InputAction.CallbackContext context) => TryTriggerJump(context);

    private void MobileInputManager_OnPressRightSideOfScreen()
    {
        if (_disableFunctionality) return;
        if (GlideAction.IsGliding)
        {
            if (_isJumping && _isDoubleJumping) return;
            _isJumping = true;
        }
        else if (HaltAction.IsHalting)
        {
            return;
        }

        TryTriggerJump(default, true);
        if (_isMovementPressed) return;
        TryTriggerMovement();
    }

    public void TryTriggerMovement()
    {
        if (_disableFunctionality) return;
        if (LevelManager.Instance.IsLevelFinished) return;
        _isMovementPressed = true;
        _currMovementInput = new Vector2(1f, 0f);
        ApplyMovementSpeed();
        InvokePlayerRun();
        OnPlayerIdle?.Invoke(_isMovementPressed);
    }

    private void TryTriggerJump(InputAction.CallbackContext context = default, bool isTouchInput = false)
    {
        if (_disableFunctionality) return;
        if (LevelManager.Instance.IsLevelFinished) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        if (CustomTransistion.Instance != null && !CustomTransistion.Instance.IsTransitionFinished) return;
        if (!_canJump) return;

        if ((_isJumping || _isDoubleJumping) && IsGroundNearWhileFalling)
        {
            _queueJump = true;
            if (_queueJumpTween != null) _queueJumpTween.Kill();
            _queueJumpTween = DOVirtual.Float(1f, 0f, _jumpPredictionDuration, value => { })
                .OnComplete(() => { _queueJump = false; });
        }

        if (_isJumping || _isDoubleJumping)
        {
            _queueWallJump = true;
            if (_queueWallJumpTween != null) _queueWallJumpTween.Kill();
            _queueWallJumpTween = DOVirtual.Float(1f, 0f, _wallJumpPredictionDuration, value => { })
                .OnComplete(() => { _queueWallJump = false; });

            _queueLadderJump = true;
            if (_queueLadderJumpTween != null) _queueLadderJumpTween.Kill();
            _queueLadderJumpTween = DOVirtual.Float(1f, 0f, _ladderJumpPredictionDuration, value => { })
                .OnComplete(() => { _queueLadderJump = false; });
        }

        if (GlideAction.IsGliding) GlideAction.CancelGlide();

        if (_isMovementPressed == false && _enableMovementTrigger)
        {
            TryTriggerMovement();
            _isRunStarted = true;
            return;
        }

        if (IsFloating) return;
        if (_climbAction.IsClimbing)
        {
            if (!CanWallJump()) return;

            _allowLadderJumping = true;
            /*_climbAction.StopClimb();
            _isJumpPressed = true;
            _isDoubleJumpPressed = false;
            OnPlayerJumpCancel?.Invoke(false);*/
            return;
        }

        if (_isGrabbingLedge) return;

        if (isTouchInput)
            _isJumpPressed = true;
        else
            _isJumpPressed = context.ReadValueAsButton();

        if (_isWallHanging)
        {
            _isDoubleJumping = true;
            _isDoubleJumpPressed = true;
        }

        if (!_isDoubleJumpPressed && _isJumping && _isJumpPressed && !_isJumpingFromJumpad && _canDoubleJump)
            _isDoubleJumpPressed = true;

        if (IsSliding)
        {
            IsSliding = false;
            OnPlayerSlideCancel?.Invoke(_isMovementPressed);
        }
    }

    private bool CanWallJump()
    {
        if (WallAndPlatformChecker.Instance.transform.rotation.eulerAngles.y > 110f ||
            WallAndPlatformChecker.Instance.transform.rotation.eulerAngles.y < 70f)
            return false;

        if (_climbAction.CurrLadder.IsFinalLadderPart)
        {
            if (!WallAndPlatformChecker.Instance.CanJumpFromFinalLadder()) return false;
        }
        else
        {
            if (!WallAndPlatformChecker.Instance.CanJumpWhileClimbing()) return false;
        }

        return true;
    }

    private void LevelFinish_OnAnyLevelCompleted() => ResetMovementOnFinish();

    private void RopHangAction_OnGrabRope()
    {
        StopAllCoroutines();
        _currSpeed = _movementSpeed;
    }

    private void DamageableBoss_OnDie() => ResetMovementOnFinish();

    private void ResetMovementOnFinish()
    {
        _currMovement.x = 0f;
        _appliedMovement.x = 0f;
        OnPlayerIdle?.Invoke(false);
        OnPlayerFinish?.Invoke();
        _disableFunctionality = true;
    }

    public void UpdateMovement()
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (_wallJumpSkipCheckTimer >= 0f) _wallJumpSkipCheckTimer -= Time.deltaTime;
        if (_isGrabbingLedge) return;

        if (_alignToSurface != null && !_climbAction.IsClimbing && !_ropeHangAction.IsHangingFromRope &&
            !_isWallHanging && _isMovementPressed)
            _alignToSurface.SurfaceAlignment();

        HandleCollision();
        if (!_ropeHangAction.IsHangingFromRope && !LevelManager.Instance.IsLevelFinished) HandleRotation();
        if (!_climbAction.IsClimbing && !_ropeHangAction.IsHangingFromRope)
        {
            HandleMovement();
            HandleSlide();
        }

        //Debug.DrawLine(transform.position, transform.position.y * (Vector3.down * _maxGroundJumpDistance), Color.green);
        if (_queueJump && IsGroundedRay) _isJumpPressed = true;

        HandleGravity();
        HandleJump();
        HandleFloating();
    }

    private void HandleCollision()
    {
        if (_isWallHanging || _isWallJumping) return;
        if (_ropeHangAction.IgnoreHeadCollision) return;
        if (_characterController.collisionFlags == CollisionFlags.CollidedAbove && _currMovement.y > 0f)
            _currMovement.y = 0f;
    }

    private void HandleMovement()
    {
        if (_isWallJumping) return;

        _appliedMovement.y = _currMovement.y;

        if (IsSliding && !_isJumping && !_isWallHanging)
        {
            _appliedMovement.x = _currSlideMovement.x;
            _appliedMovement.z = _currSlideMovement.z;
            _isMoveSpeedSetToRun = false;
            if (!_isMoveSpeedSetToSlide)
            {
                _isMoveSpeedSetToSlide = true;
                _desiredMoveSpeed = _slideSpeed;
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed(true));
            }
        }
        else
        {
            _appliedMovement.x = _currMovement.x;
            _appliedMovement.z = _currMovement.z;
            _isMoveSpeedSetToSlide = false;
            if (!_isMoveSpeedSetToRun)
            {
                _isMoveSpeedSetToRun = true;
                _desiredMoveSpeed = _movementSpeed;
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed(false));
            }
        }

        if ((IsFacingWall || IsFacingGroundAsWall) && _currSpeed > _movementSpeed)
        {
            StopAllCoroutines();
            _currSpeed = _movementSpeed;
        }

        if ((IsFacingWall || (IsGroundedRay && IsFacingGroundAsWall)) && !_isJumping && !_isWallJumping &&
            !_isWallHanging)
            StopMovementHorizontal();

        if (IsFacingWall && IsGroundedRay && _isWallHanging)
        {
            _isWallHanging = false;
            StopMovementHorizontal();
        }

        if (_haltAction.IsHalting && IsGroundedRay && !IsSliding && !_isWallJumping && !_isWallHanging &&
            _currSpeed < SlideSpeedMinLimit)
            StopMovementHorizontal();

        if (IsSliding || (_currSpeed > SlideSpeedMinLimit) && !_isJumping && IsGroundedRay && !IsFloating)
        {
            if (GlideAction.IsGliding) GlideAction.CancelGlide();
            OnPlayerSlide?.Invoke();
        }
        else
            OnPlayerSlideCancel?.Invoke(_isMovementPressed);

        Vector3 movement = new Vector3(_appliedMovement.x * _currSpeed,
            (_appliedMovement.y * _additionalSlideJumpForce),
            _appliedMovement.z * _currSpeed) * Time.deltaTime;
        movement = AdjustVelocityToSlope(movement);

        if (_characterController.enabled)
            _characterController.Move(movement);

        if (_freezeZPosition)
            transform.position = new Vector3(transform.position.x, transform.position.y, _startZPosition);
    }

    private void StopMovementHorizontal()
    {
        _appliedMovement.x = 0f;
        _appliedMovement.z = 0f;
        _currSpeed = _movementSpeed;
        OnPlayerIdle?.Invoke(false);
    }

    private void HandleSlide()
    {
        if (_isJumping || _isGrabbingLedge || _isWallHanging || _isWallJumping || !OnSlope() || !_isMovementPressed ||
            IsFloating)
        {
            IsSliding = false;
            return;
        }

        if (_isJumping) OnPlayerJumpCancel?.Invoke(_isMovementPressed);

        float dot = Vector3.Dot(_slopeHit.normal, transform.forward);
        IsSliding = dot > 0f;
    }

    private IEnumerator SmoothlyLerpMoveSpeed(bool isSliding)
    {
        float multiplier = isSliding ? _slideSpeedMomentumMultiplier : _moveSpeedMomentumMultiplier;
        float time = 0f;
        float difference = Mathf.Abs(_desiredMoveSpeed - _currSpeed);
        float startValue = _currSpeed;

        while (time < difference)
        {
            _currSpeed = Mathf.Lerp(startValue, _desiredMoveSpeed, time / difference);
            time += Time.deltaTime * multiplier;
            yield return null;
        }

        _currSpeed = _desiredMoveSpeed;
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _freezeZPosition ? 0f : _currMovement.z;

        Quaternion currentRotation = transform.rotation;
        if (_climbAction.IsClimbing)
        {
            positionToLookAt.x = 0f;
            positionToLookAt.z = 1f;
        }

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            // this line magically makes the player rotate in the clockwise direction. I can't explain it
            targetRotation.eulerAngles = targetRotation.eulerAngles;
            // magic code out
            transform.rotation =
                Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (_queueLadderJump && !_isJumping && _climbAction.IsClimbing && CanWallJump())
            _allowLadderJumping = true;

        if (_allowLadderJumping)
        {
            _climbAction.StopClimb();
            _isJumpPressed = true;
            _isDoubleJumpPressed = false;
            OnPlayerJumpCancel?.Invoke(false);
            _allowLadderJumping = false;
            return;
        }

        if (_isWallHanging)
        {
            if (_queueWallJump && !_isJumping)
            {
                _isJumping = false;
                _isJumpPressed = true;
                _isDoubleJumpPressed = true;
                _isDoubleJumping = true;
            }

            if (!_isJumping && _isJumpPressed)
            {
                OnPlayerWallJump?.Invoke();
                _isWallJumping = true;
                _isWallHanging = false;
                _wallJumpSkipCheckTimer = _wallJumpSkipCheckDuration;
                SwitchMoveDirection();
                InitPlayerJump(OnPlayerJump);
            }

            return;
        }

        if (!_isJumping && _isJumpPressed)
        {
            IgnoreFall = false;
            _isWallJumping = false;
            _isJumping = true;
            InitPlayerJump(OnPlayerJump);
        }
        else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
            _isDoubleJumping = false;
            _isDoubleJumpPressed = false;
            _isWallJumping = false;
        }

        if (_isJumping && _isDoubleJumpPressed && !_isDoubleJumping && !_isWallJumping)
        {
            IgnoreFall = false;
            _isDoubleJumping = true;
            InitPlayerJump(OnPlayerDoubleJump);
        }
    }

    private void InitPlayerJump(Action callback)
    {
        callback?.Invoke();
        AddJumpVelocity(_initialJumpVelocity);
        OnPlayJumpSfx?.Invoke();
    }

    private void HandleFloating()
    {
        if (_ropeHangAction.IsHangingFromRope) return;
        if (_flyAction.IsFloating) UpdateFloatingSpeed(_flyAction);
        else if (_swimAction.IsFloating) UpdateFloatingSpeed(_swimAction);
    }

    private void UpdateFloatingSpeed(FloatAction floatAction)
    {
        float minYSpeed;
        float maxYSpeed;
        minYSpeed = floatAction.DefaultDownForce;
        maxYSpeed = floatAction.UpForce;
        if (floatAction.CurrFloatActionDirection == FloatActionDirection.Descending) minYSpeed = floatAction.DownForce;
        _currMovement.y = Mathf.Clamp(_currMovement.y, minYSpeed, maxYSpeed);
        _appliedMovement.y = Mathf.Clamp(_currMovement.y, minYSpeed, maxYSpeed);
    }

    public void OnTriggerJumpFromOtherSources(float jumpPadJumpVelocity, float horizontalVelocity = 0f,
        bool slowDownMovement = false, bool playJumpFromJumpadAnimation = true)
    {
        _glideAction.CancelGlide();
        _isJumpingFromJumpad = true;

        if (_isJumpingFromJumpad)
        {
            if (_tweenJumpad != null) _tweenJumpad.Kill();
            _tweenJumpad = DOVirtual.Float(0.1f, 0f, 0.1f, value => { })
                .OnComplete(() => { _isJumpingFromJumpad = false; });
        }

        _flyAction.EndFloating();
        _swimAction.EndFloating();
        AutoJump(jumpPadJumpVelocity, playJumpFromJumpadAnimation);
        if (slowDownMovement)
            _currSpeed = _movementSpeed / _jumpadMovementReductionFactor;
        else if (horizontalVelocity <= 0f)
            return;

        if (_isMovementPressed) _currSpeed += horizontalVelocity;
        _desiredMoveSpeed = _movementSpeed;
        StopAllCoroutines();
        StartCoroutine(SmoothlyLerpMoveSpeed(false));
    }

    public void AutoJump(float jumpVelocity, bool playJumpFromJumpadAnimation = false)
    {
        _isJumpPressed = true;
        _isDoubleJumping = false;
        _isDoubleJumpPressed = false;
        IgnoreFall = false;
        _isWallJumping = false;
        _isJumping = true;
        OnPlayerJumpCancel?.Invoke(_isMovementPressed);
        if (playJumpFromJumpadAnimation)
            OnPlayerJumpFromJumpad?.Invoke();
        else
            OnPlayerJump?.Invoke();

        AddJumpVelocity(jumpVelocity);
    }

    private void AddJumpVelocity(float jumpVelocity)
    {
        _currMovement.y = _freezeYPosition ? 0f : jumpVelocity;
        _appliedMovement.y = _freezeYPosition ? 0f : jumpVelocity;
        _missileDeflectAction.EnableMissileInvincibility();
    }

    public void StopMovingHorizontally()
    {
        _currSpeed = 0f;
        StopAllCoroutines();
        StartCoroutine(SmoothlyLerpMoveSpeed(false));
    }

    private void HandleGravity()
    {
        if (_swimAction.IsFloating && _swimAction.IsGroundedRayUnderwater)
        {
            _currMovement.y = _swimAction.SwimUpForce;
            _appliedMovement.y = _swimAction.SwimUpForce;
            return;
        }

        if (_ropeHangAction.IsHangingFromRope) return;
        if (_glideAction.IsGliding && !IsGroundedRay)
        {
            if (_currMovement.y > 0f || _appliedMovement.y > 0f)
                ResetGlidingSpeed();
            return;
        }

        if (_climbAction.IsClimbing)
        {
            _appliedMovement.x = 0f;
            _appliedMovement.y = _haltAction.IsHalting ? 0f : ClimbAction.ClimbSpeed * Time.deltaTime;
            _appliedMovement.z = 0f;
            _isFalling = false;
            _characterController.Move(_appliedMovement);
            return;
        }

        float fallMultiplier = 1f;
        _currFallClampedSpeed = _fallClampSpeed;
        _isFalling = _currMovement.y <= 0f || !_isJumpPressed;
        if (_isWallHanging)
        {
            fallMultiplier = _hangingFallMultiplier;
        }
        else if (_flyAction.IsFloating)
        {
            fallMultiplier = _flyAction.FallMultiplier;
            _currFallClampedSpeed = _flyAction.FlyingFallClampSpeed;
        }
        else if (_swimAction.IsFloating)
        {
            fallMultiplier = _swimAction.FallMultiplier;
            _currFallClampedSpeed = _swimAction.FlyingFallClampSpeed;
        }
        else if (_glideAction.IsGliding)
        {
            fallMultiplier = _glideAction.GlideFallModifier;
            _currFallClampedSpeed = _glideAction.GlideMaxFallSpeed;
        }
        else
        {
            fallMultiplier = IgnoreFall ? 1f : _freeFallMultiplier;
        }

        // Frame independent jump. Velocity verlet integration for fall calculation after jumping
        // Resource: https://www.youtube.com/watch?v=h2r3_KjChf4 at 13:40
        if (_characterController.isGrounded && !IsFloating)
        {
            _glideAction.CancelGlide();
            _currMovement.y = _groundedGravity;
            OnPlayerJumpCancel?.Invoke(_isMovementPressed);
            if (_isJumping && _isJumpPressed) _isJumpPressed = false;
            if (_isDoubleJumping && _isDoubleJumpPressed) _isDoubleJumpPressed = false;
            _isJumping = false;
            _isDoubleJumping = false;
            OnPlayerFallCancel?.Invoke(_isMovementPressed);
        }
        else if (_isFalling)
        {
            float previousYVelocity = _currMovement.y;
            _currMovement.y = _currMovement.y + (_gravity * Time.deltaTime * fallMultiplier);
            _appliedMovement.y = Mathf.Max((previousYVelocity + _currMovement.y) * 0.5f, _currFallClampedSpeed);
            if (!_isJumping && !IsGroundedRay && !_isWallJumping && !_isWallHanging && !IsFloating)
                OnPlayerFall?.Invoke();
            if (IsSliding && !IsGroundedRay) IsSliding = false;
        }
        else
        {
            float previousYVelocity = _currMovement.y;
            _currMovement.y = _currMovement.y + (_gravity * Time.deltaTime);
            _appliedMovement.y = (previousYVelocity + _currMovement.y) * 0.5f;
        }

        if (IsGroundedRay)
        {
            if (_flyAction.IsFloating) _flyAction.EndFloating();
            if (_swimAction.IsFloating) _swimAction.EndFloating();
        }
    }

    public void ResetGlidingSpeed()
    {
        _currMovement.y = GlideAction.GlideGravityValue;
        _appliedMovement.y = GlideAction.GlideGravityValue;
    }

    private void OnClimbLedge()
    {
        if (IsFloating) return;
        if (IsFacingWall) return;
        if (_isGrabbingLedge) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        _isGrabbingLedge = true;
        _currSpeed = 0f;
        OnPlayerClimbLedge?.Invoke();
        if (_grabLedgeTween != null) _grabLedgeTween.Kill();
        _grabLedgeTween = DOVirtual.Float(.2f, 0f, .2f, value => { }).OnComplete(() =>
        {
            if (_isJumping) OnPlayerJumpCancel?.Invoke(_isMovementPressed);
            _currMovement.y = _initialLedgeJumpVelocity;
            _appliedMovement.y = _initialLedgeJumpVelocity;
            _currSpeed = _movementSpeed / _ledgeGrabMovementReductionFactor;

            _desiredMoveSpeed = _movementSpeed;
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed(false));

            _isDoubleJumpPressed = false;
            _isDoubleJumping = false;
            _isGrabbingLedge = false;
            _isWallHanging = false;
            _isJumping = true;
            OnPlayerJump?.Invoke();
            OnPlayerWallJump?.Invoke();
            IgnoreFall = true;
            OnPlayLedgeClimbSfx?.Invoke();
        });
    }

    private void JumpRefiller_OnAnyCollectJumpRefiller()
    {
        _isDoubleJumping = false;
        _isDoubleJumpPressed = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsFacingWall)
        {
            if (_isJumping && _currMovement.y <= 1f && _wallJumpSkipCheckTimer <= 0f && !_isGrabbingLedge)
            {
                OnPlayerJumpCancel?.Invoke(_isMovementPressed);
                _isJumping = false;
                _isDoubleJumpPressed = false;
                _isDoubleJumping = false;
                _isWallHanging = true;
                _isFalling = false;
                _isJumpPressed = false;
                _currMovement.y = -0.05f;
                _appliedMovement.y = -0.05f;
                OnPlayerWallHang?.Invoke();
            }
        }
        else if (_isWallHanging)
        {
            _isWallHanging = false;
        }
    }

    private bool OnSlope()
    {
        float edgePoint = transform.position.x - _currMovementInput.x * _characterController.radius;
        float center = transform.position.y + _characterController.height / 2f;
        float rayLength = _characterController.height / 2f + 0.2f;
        Vector3 origin = new Vector3(edgePoint, center, transform.position.z);
        //Debug.DrawRay(origin, Vector3.down * rayLength, Color.red);
        if (Physics.Raycast(origin, Vector3.down, out _slopeHit, rayLength, _groundedLayers))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle >= _minAngleSlide;
        }

        return false;
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        RaycastHit slopeHit;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, _characterController.height / 2f + 0.3f))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, slopeHit.normal);
            Vector3 adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0 && IsSliding) return adjustedVelocity;
        }

        return velocity;
    }

    public void SwitchMoveDirection()
    {
        _currMovement.x *= -1f;
        _currSlideMovement.x *= -1f;
    }

    public void SetMoveDirection(MoveDirection moveDirection)
    {
        switch (moveDirection)
        {
            case MoveDirection.Right:
                _currMovement.x = Mathf.Abs(_currMovement.x);
                _currSlideMovement.x = Mathf.Abs(_currSlideMovement.x);
                break;
            case MoveDirection.Left:
                _currMovement.x = -Mathf.Abs(_currMovement.x);
                _currSlideMovement.x = -Mathf.Abs(_currSlideMovement.x);
                break;
        }
    }

    public void ResetMovement()
    {
        StopAllCoroutines();
        _currMovementInput.x = 0f;
        _currMovement.x = 0f;
        _appliedMovement.x = 0f;

        if (_currMovement.y > 0)
        {
            _currMovement.y = -1f;
            _appliedMovement.y = -1f;
        }

        _currSpeed = _movementSpeed;
        _currSlideMovement.x = 0f;
        _currSlideMovement.z = 0f;
        _desiredMoveSpeed = 0f;
        _isMoveSpeedSetToRun = false;
        _isMoveSpeedSetToSlide = false;
        _isMovementPressed = false;
        _isJumping = false;
        _isDoubleJumpPressed = false;
        _isDoubleJumping = false;
        _isWallHanging = true;
        _isFalling = false;
        _isJumpPressed = false;
        _isGrabbingLedge = false;
        _isSliding = false;
        OnPlayerIdle?.Invoke(_isMovementPressed);
        _isRunStarted = false;
        _queueJump = false;
        _queueWallJump = false;
        if (_queueJumpTween != null) _queueJumpTween.Kill();
        if (_queueWallJumpTween != null) _queueWallJumpTween.Kill();
        if (_grabLedgeTween != null) _grabLedgeTween.Kill();
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        _disableFunctionality = true;
        DOVirtual.DelayedCall(_disableFunctionalityOnRespawnDuration, () => _disableFunctionality = false);
    }

    public void ResetRotation() => transform.localRotation = Quaternion.Euler(_startingRotation);

    public void ResetMovementSpeed()
    {
        StopAllCoroutines();
        _currSpeed = _movementSpeed;
        _currSlideMovement.x = 0f;
        _currSlideMovement.z = 0f;
        _isMoveSpeedSetToRun = true;
        _isMoveSpeedSetToSlide = false;
    }

    private Vector3 CenterOfPlayer()
    {
        return new Vector3(transform.position.x,
            transform.position.y + (_characterController.height / 2f),
            transform.position.z);
    }

    public void InvokePlayerRun()
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        OnPlayerRun?.Invoke(_isMovementPressed);
    }

    public bool IsGroundedRay => IsGroundedRayCenter() || IsGroundedRayRight();

    public bool IsGroundedRayCenter() => Physics.Raycast(transform.position,
        Vector3.down,
        _characterController.height * 0.1f,
        _groundedLayers);

    public bool IsGroundedRayRight() => Physics.Raycast(_groundCheckOriginTransform.position,
        Vector3.down,
        _characterController.height * 0.2f,
        _groundedLayers);

    public bool IsFacingWall => Physics.Raycast(CenterOfPlayer(), transform.forward, .7f, _wallLayer);
    public bool IsFacingGroundAsWall => Physics.Raycast(CenterOfPlayer(), transform.forward, .7f, _groundedLayersOnly);

    public bool IsGroundNearWhileFalling =>
        Physics.Raycast(transform.position,
            Vector3.down,
            _characterController.height * _maxGroundJumpDistance,
            _groundedLayers);

    public bool IsFloating => _flyAction.IsFloating || _swimAction.IsFloating;
}


using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class FloatAction : BaseAction
{
    public static event Action<float, float, Vector3> OnAnyUpdateFloatCollider;
    public static event Action OnAnyResetCollider;

    [SerializeField] protected float _upForce = 1.5f;
    [SerializeField] protected float _downForce = -1.5f;
    [SerializeField] protected float _defaultDownForce = -1f;
    [SerializeField] protected float _freeFallForce = 0.7f;

    [SerializeField] protected float _flyingFallClampSpeed = -0.05f;

    [SerializeField] protected float _ascendingGravityModifier = -1f;
    [SerializeField] protected float _descendingGravityModifier = 1f;

    [BoxGroup("Collider Params"), SerializeField]
    protected float _floatingColliderRadius = 0.3f;

    [BoxGroup("Collider Params"), SerializeField]
    protected float _floatingColliderHeight = 0.6f;

    [BoxGroup("Collider Params"), SerializeField]
    protected Vector3 _floatingColliderCenter = new Vector3(0f, .6f, .6f);

    protected PlayerInput _playerInput;
    protected FloatActionDirection? _floatActionDirection;

    protected bool _isHoldingJump;
    protected bool _isHoldingHalt;
    protected bool _isFloating;

    #region Getters and Setters

    public float UpForce => _upForce;
    public float DownForce => _downForce;
    public float DefaultDownForce => _defaultDownForce;

    public float FallMultiplier
    {
        get
        {
            switch (_floatActionDirection)
            {
                case FloatActionDirection.Ascending:
                    return AscendingGravityModifier;
                case FloatActionDirection.Descending:
                    return DescendingGravityModifier;
                default:
                    return _freeFallForce;
            }
        }
    }

    public float FlyingFallClampSpeed => _flyingFallClampSpeed;
    public FloatActionDirection? CurrFloatActionDirection => _floatActionDirection;
    public bool IsFloating => _isFloating;

    public float AscendingGravityModifier => _ascendingGravityModifier;
    public float DescendingGravityModifier => _descendingGravityModifier;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
        _floatActionDirection = FloatActionDirection.Neutral;
    }

    protected virtual void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Jump.started += OnAscend;
        _playerInput.CharacterControls.Jump.canceled += OnStopAscending;

        _playerInput.CharacterControls.Halt.started += OnDescend;
        _playerInput.CharacterControls.Halt.canceled += OnStopDescending;

        MobileInputManager.Instance.OnPressRightSideOfScreen += MobileInputManager_OnPressRightSideOfScreen;
        MobileInputManager.Instance.OnPressLeftSideOfScreen += MobileInputManager_OnPressLeftSideOfScreen;
        MobileInputManager.Instance.OnReleaseRightSideOfScreen += MobileInputManager_OnReleaseRightSideOfScreen;
        MobileInputManager.Instance.OnReleaseLeftSideOfScreen += MobileInputManager_OnReleaseLeftSideOfScreen;
    }

    protected virtual void OnDisable()
    {
        if (_playerInput != null) _playerInput.CharacterControls.Disable();
        if (MobileInputManager.Instance == null) return;
        MobileInputManager.Instance.OnPressRightSideOfScreen -= MobileInputManager_OnPressRightSideOfScreen;
        MobileInputManager.Instance.OnPressLeftSideOfScreen -= MobileInputManager_OnPressLeftSideOfScreen;
        MobileInputManager.Instance.OnReleaseRightSideOfScreen -= MobileInputManager_OnReleaseRightSideOfScreen;
        MobileInputManager.Instance.OnReleaseLeftSideOfScreen -= MobileInputManager_OnReleaseLeftSideOfScreen;
    }

    protected virtual void Update()
    {
        if (_isHoldingJump) Ascend();
        else if (_isHoldingHalt) Descend();
    }

    private void MobileInputManager_OnPressRightSideOfScreen()
    {
        OnStopDescending(default);
        OnAscend(default);
    }

    private void MobileInputManager_OnPressLeftSideOfScreen()
    {
        OnStopAscending(default);
        OnDescend(default);
    }

    private void MobileInputManager_OnReleaseRightSideOfScreen() => OnStopAscending(default);
    private void MobileInputManager_OnReleaseLeftSideOfScreen() => OnStopDescending(default);

    protected virtual void Float()
    {
        _playerMovementController.IsJumpPressed = false;
        _playerMovementController.IsJumping = false;
        _playerMovementController.IsDoubleJumpPressed = false;
        _playerMovementController.IsDoubleJumping = false;
        _isFloating = true;
        OnAnyUpdateFloatCollider?.Invoke(_floatingColliderRadius, _floatingColliderHeight, _floatingColliderCenter);
    }

    public virtual void EndFloating()
    {
        if (!_isFloating) return;
        _isFloating = false;
        _floatActionDirection = FloatActionDirection.Neutral;
        OnAnyResetCollider?.Invoke();
    }

    protected virtual void OnAscend(InputAction.CallbackContext context) => _isHoldingJump = true;

    protected virtual void OnStopAscending(InputAction.CallbackContext context)
    {
        if (!_isHoldingJump) return;
        _isHoldingJump = false;
        _floatActionDirection = FloatActionDirection.Neutral;
    }

    protected virtual void OnDescend(InputAction.CallbackContext context) => _isHoldingHalt = true;

    protected virtual void OnStopDescending(InputAction.CallbackContext context)
    {
        if (!_isHoldingHalt) return;
        _isHoldingHalt = false;
        _floatActionDirection = FloatActionDirection.Neutral;
    }

    protected virtual void Ascend()
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (!IsFloating) return;
        _floatActionDirection = FloatActionDirection.Ascending;
    }

    protected virtual void Descend()
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (!IsFloating) return;
        _floatActionDirection = FloatActionDirection.Descending;
    }
}
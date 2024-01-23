

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlideAction : BaseAction
{
    public static event Action OnStartGliding;
    public static event Action OnStopGliding;

    [SerializeField] private float _glideFallModifier = 0.05f;
    [SerializeField] private float _glideMaxFallSpeed = 5f;
    [SerializeField] private float _resetYMovementValue = 1f;
    [SerializeField] private float _glideGravityValue = -.5f;

    private PlayerInput _playerInput;
    private Tween _tweenCooldown;

    private bool _isGliding;
    private bool _isInCooldown;

    #region Getters and Setters

    public float GlideFallModifier => _glideFallModifier;
    public float GlideMaxFallSpeed => _glideMaxFallSpeed;
    public float ResetYMovementValue => _resetYMovementValue;
    public bool IsGliding => _isGliding;
    public float GlideGravityValue => _glideGravityValue;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Halt.started += CharacterControls_Halt_Started;
        _playerInput.CharacterControls.Halt.canceled += CharacterControls_Halt_Canceled;

        CharacterPlayerController.OnPlayerDied += OnCancelGlide;
        FlyAction.OnPlayerFly += OnCancelGlide;
        SwimAction.OnPlayerSwim += OnCancelGlide;
        ClimbAction.OnPlayerClimb += ClimbAction_OnPlayerClimb;
        ClimbAction.OnPlayerClimbCancel += ClimbAction_OnPlayerClimbCancel;
        MovementController.OnPlayerRun += MovementController_OnPlayerRun;
        MovementController.OnPlayerClimbLedge += OnCancelGlide;
        MovementController.OnPlayerWallHang += OnCancelGlide;
        MovementController.OnPlayerSlide += OnCancelGlide;
        RopeHangAction.OnAnyGrabRope += RopeHangAction_OnAnyGrabRope;

        MobileInputManager.Instance.OnPressLeftSideOfScreen += MobileInputManager_OnPressLeftSideOfScreen;
        MobileInputManager.Instance.OnReleaseLeftSideOfScreen += MobileInputManager_OnReleaseLeftSideOfScreen;
    }

    private void OnDisable()
    {
        if (_playerInput != null) _playerInput.CharacterControls.Disable();

        CharacterPlayerController.OnPlayerDied -= OnCancelGlide;
        FlyAction.OnPlayerFly -= OnCancelGlide;
        SwimAction.OnPlayerSwim -= OnCancelGlide;
        ClimbAction.OnPlayerClimb -= ClimbAction_OnPlayerClimb;
        ClimbAction.OnPlayerClimbCancel -= ClimbAction_OnPlayerClimbCancel;
        MovementController.OnPlayerRun -= MovementController_OnPlayerRun;
        MovementController.OnPlayerClimbLedge -= OnCancelGlide;
        MovementController.OnPlayerWallHang -= OnCancelGlide;
        MovementController.OnPlayerSlide -= OnCancelGlide;
        RopeHangAction.OnAnyGrabRope -= RopeHangAction_OnAnyGrabRope;

        if (MobileInputManager.Instance == null) return;
        MobileInputManager.Instance.OnPressLeftSideOfScreen -= MobileInputManager_OnPressLeftSideOfScreen;
        MobileInputManager.Instance.OnReleaseLeftSideOfScreen -= MobileInputManager_OnReleaseLeftSideOfScreen;
    }

    private void CharacterControls_Halt_Started(InputAction.CallbackContext context) => HandleHalt();

    private void MobileInputManager_OnPressLeftSideOfScreen() => HandleHalt();

    private void HandleHalt()
    {
        if (LevelManager.Instance.IsLevelFinished) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        if ((_playerMovementController.IsFalling || _playerMovementController.IsJumping)
            && !_playerMovementController.IsGroundedRay
            && !_playerMovementController.IsFloating
            && !_playerMovementController.ClimbAction.IsClimbing
            && !_playerMovementController.IsGrabbingLedge
            && !_playerMovementController.IsWallHanging
            && !_isInCooldown)
        {
            _isGliding = true;
            OnStartGliding?.Invoke();
            _playerMovementController.ResetGlidingSpeed();
        }
    }

    private void CharacterControls_Halt_Canceled(InputAction.CallbackContext context) => CancelGlide();
    private void MobileInputManager_OnReleaseLeftSideOfScreen() => CancelGlide();

    private void MovementController_OnPlayerRun(bool isMovementPressed)
    {
        if (isMovementPressed) CancelGlide();
    }

    private void RopeHangAction_OnAnyGrabRope(float direction) => CancelGlide();

    private void ClimbAction_OnPlayerClimb(float climbSpeed)
    {
        EnableCooldown();
        CancelGlide();
    }

    private void ClimbAction_OnPlayerClimbCancel() => EnableCooldown();

    private void EnableCooldown()
    {
        _isInCooldown = true;
        if (_tweenCooldown != null) _tweenCooldown.Kill();
        _tweenCooldown = DOVirtual.Float(1f, 0f, 1f, value => { }).OnComplete(() => { _isInCooldown = false; });
    }

    private void OnCancelGlide() => CancelGlide();

    public void CancelGlide()
    {
        if (!_isGliding) return;
        _isGliding = false;
        OnStopGliding?.Invoke();
    }
}
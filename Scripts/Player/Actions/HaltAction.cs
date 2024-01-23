
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HaltAction : BaseAction
{
    public static event Action OnPlayerHalt;
    public static event Action OnPlayerStopHalt;

    private PlayerInput _playerInput;
    private Coroutine _haltCoroutine;
    private WaitForSeconds _waitHalt;

    private bool _isHalting;

    public bool IsHalting
    {
        get => _isHalting;
        set => _isHalting = value;
    }

    protected override void Awake()
    {
        base.Awake();
        _playerInput = new PlayerInput();
        _waitHalt = new WaitForSeconds(.1f);
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Halt.started += OnHalt;
        _playerInput.CharacterControls.Halt.performed += OnHalt;
        _playerInput.CharacterControls.Halt.canceled += OnStopHalt;

        MovementController.OnPlayerRun += MovementController_OnPlayerRun;
        MovementController.OnPlayerJump += MovementController_OnPlayerJump;
        ClimbAction.OnPlayerClimb += ClimbAction_OnPlayerClimb;

        MobileInputManager.Instance.OnPressLeftSideOfScreen += MobileInputManager_OnPressLeftSideOfScreen;
        MobileInputManager.Instance.OnReleaseLeftSideOfScreen += MobileInputManager_OnReleaseLeftSideOfScreen;
    }

    private void OnDisable()
    {
        if (_playerInput != null) _playerInput.CharacterControls.Disable();
        MovementController.OnPlayerRun -= MovementController_OnPlayerRun;
        MovementController.OnPlayerJump -= MovementController_OnPlayerJump;
        ClimbAction.OnPlayerClimb -= ClimbAction_OnPlayerClimb;

        if (MobileInputManager.Instance == null) return;
        MobileInputManager.Instance.OnPressLeftSideOfScreen -= MobileInputManager_OnPressLeftSideOfScreen;
        MobileInputManager.Instance.OnReleaseLeftSideOfScreen -= MobileInputManager_OnReleaseLeftSideOfScreen;
    }

    private void MobileInputManager_OnPressLeftSideOfScreen() => HandleHalt();
    private void MobileInputManager_OnReleaseLeftSideOfScreen() => HandleStopHalt();

    public void OnHalt(InputAction.CallbackContext context) => HandleHalt();

    private void HandleHalt()
    {
        if (LevelManager.Instance.IsLevelFinished) return;
        //if (_playerMovementController.GlideAction.IsGliding) return;
        if (_playerMovementController.IsWallHanging) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        IsHalting = true;
        OnPlayerHalt?.Invoke();
    }

    public void OnStopHalt(InputAction.CallbackContext context) => HandleStopHalt();

    private void HandleStopHalt()
    {
        if (!_isHalting) return;
        if (_playerMovementController.IsWallHanging)
        {
            if (_isHalting) _isHalting = false;
            return;
        }

        if (!_playerMovementController.IsJumping
            && !_playerMovementController.IsDoubleJumping
            && !_playerMovementController.IsFloating)
            _playerMovementController.InvokePlayerRun();
        IsHalting = false;
        OnPlayerStopHalt?.Invoke();
    }

    private void MovementController_OnPlayerJump()
    {
        IsHalting = false;
        OnPlayerStopHalt?.Invoke();
    }
    
    private void MovementController_OnPlayerRun(bool isMovementPressed) => IsHalting = false;

    private void ClimbAction_OnPlayerClimb(float direction)
    {
        if (!IsHalting) return;
        if (_haltCoroutine != null) StopCoroutine(_haltCoroutine);
        _haltCoroutine = StartCoroutine(DelayHalt());
    }

    private IEnumerator DelayHalt()
    {
        yield return _waitHalt;
        OnPlayerHalt?.Invoke();
    }
}
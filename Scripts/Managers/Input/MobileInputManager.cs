
using System;
using Leon;
using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class MobileInputManager : SceneSingleton<MobileInputManager>
{
    public event Action OnPressRightSideOfScreen;
    public event Action OnPressLeftSideOfScreen;
    public event Action OnReleaseRightSideOfScreen;
    public event Action OnReleaseLeftSideOfScreen;

    private bool _isPressingRightSideOfTheScreen;
    private bool _isPressingLeftSideOfTheScreen;

    private FingerPosition _firstFingerPosition;
    private FingerPosition _secondFingerPosition;

    protected override void Awake()
    {
        base.Awake();
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += Touch_OnFingerDown;
        EnhancedTouch.Touch.onFingerUp += Touch_OnFingerUp;
    }

    private void OnDisable()
    {
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= Touch_OnFingerDown;
        EnhancedTouch.Touch.onFingerUp -= Touch_OnFingerUp;
    }

    private void CharacterPlayerController_OnPlayerDied() => ResetTouch();
    private void PlayerRespawn_OnPlayerRespawn() => ResetTouch();

    private void ResetTouch()
    {
        OnReleaseLeftSideOfScreen?.Invoke();
        OnReleaseRightSideOfScreen?.Invoke();
        _isPressingRightSideOfTheScreen = false;
        _isPressingLeftSideOfTheScreen = false;
        _firstFingerPosition = null;
        _secondFingerPosition = null;
    }

    private void Touch_OnFingerDown(EnhancedTouch.Finger finger)
    {
        if (UIImprovedTutorial.Instance != null && !UIImprovedTutorial.Instance.CanSkipTutorial) return;
        if (LevelManager.Instance.GameState == GameState.Paused) return;
        if (CustomTransistion.Instance != null && !CustomTransistion.Instance.IsTransitionFinished) return;
        if (CharacterPlayerController.Instance.IsDead) return;

        if (_firstFingerPosition == null)
        {
            _firstFingerPosition = new FingerPosition(finger);
            ExecuteInput(ref _firstFingerPosition);
        }
        else if (_secondFingerPosition == null)
        {
            _secondFingerPosition = new FingerPosition(finger);
            ExecuteInput(ref _secondFingerPosition);
        }
    }

    private void Touch_OnFingerUp(EnhancedTouch.Finger finger)
    {
        if (UIImprovedTutorial.Instance != null && !UIImprovedTutorial.Instance.CanSkipTutorial) return;
        if (LevelManager.Instance.GameState == GameState.Paused) return;
        if (CustomTransistion.Instance != null && !CustomTransistion.Instance.IsTransitionFinished) return;
        if (CharacterPlayerController.Instance.IsDead) return;

        if (_firstFingerPosition != null && _firstFingerPosition.IsActiveFinger(finger))
            UpdatePressingAreaVariables(ref _firstFingerPosition);
        else if (_secondFingerPosition != null && _secondFingerPosition.IsActiveFinger(finger))
            UpdatePressingAreaVariables(ref _secondFingerPosition);
    }

    private void ExecuteInput(ref FingerPosition fingerPosition)
    {
        if (fingerPosition.FirstTouchScreenPosition.x > Screen.width / 2f)
        {
            if (_isPressingRightSideOfTheScreen)
            {
                fingerPosition = null;
                return;
            }

            _isPressingRightSideOfTheScreen = true;
            fingerPosition.ScreenTouchArea = TouchArea.Right;
            OnPressRightSideOfScreen?.Invoke();
        }
        else
        {
            if (_isPressingLeftSideOfTheScreen)
            {
                fingerPosition = null;
                return;
            }

            _isPressingLeftSideOfTheScreen = true;
            fingerPosition.ScreenTouchArea = TouchArea.Left;
            OnPressLeftSideOfScreen?.Invoke();
        }
    }

    private void UpdatePressingAreaVariables(ref FingerPosition fingerPosition)
    {
        switch (fingerPosition.ScreenTouchArea)
        {
            case TouchArea.Right:
                _isPressingRightSideOfTheScreen = false;
                OnReleaseRightSideOfScreen?.Invoke();
                break;
            case TouchArea.Left:
                _isPressingLeftSideOfTheScreen = false;
                OnReleaseLeftSideOfScreen?.Invoke();
                break;
        }

        fingerPosition = null;
    }
}
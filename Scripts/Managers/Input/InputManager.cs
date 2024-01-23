using System;
using UnityEngine;
using UnityEngine.Assertions;

public class InputManager : MonoBehaviour
{
    public static event Action OnRunKeyPressed;
    public static event Action OnJumpKeyPressed;
    public static event Action OnSlideKeyPressed;
    public static event Action OnStopKeyPressed;
    public static event Action OnSlideKeyUp;
    public static event Action<bool> OnGameStarted;

    [Header("Input"), SerializeField] private GameControlsData _gameControls;

    private void Start() => Assert.IsNotNull(_gameControls);
    
    private void Update()
    {
        if (CustomTransistion.Instance != null && !CustomTransistion.Instance.IsTransitionFinished) return;
        if (UIImprovedTutorial.Instance != null && !UIImprovedTutorial.Instance.CanSkipTutorial) return;
        if (LevelManager.Instance.GameState == GameState.Paused) return;
        if (CharacterPlayerController.Instance.IsDead) return;

        if (Input.GetKeyDown(_gameControls.KeyRun))
        {
            OnGameStarted?.Invoke(true);
            OnRunKeyPressed?.Invoke();
        }
        if (Input.GetKeyDown(_gameControls.KeyJump)) OnJumpKeyPressed?.Invoke();
        if (Input.GetKey(_gameControls.KeySlide)) OnSlideKeyPressed?.Invoke();
        if (Input.GetKeyDown(_gameControls.KeyStop)) OnStopKeyPressed?.Invoke();
        if (Input.GetKeyUp(_gameControls.KeySlide)) OnSlideKeyUp?.Invoke();
    }
}

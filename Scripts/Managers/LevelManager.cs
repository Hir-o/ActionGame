
using System;
using Leon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SceneSingleton<LevelManager>
{
    public static event Action OnLevelFinished;
    public event Action OnChangingScene;

    private bool _isLevelFinished;

    public bool IsLevelFinished => _isLevelFinished;

    private GameState _gameState = GameState.Playing;

    #region Properties

    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            if (_gameState == GameState.Playing)
                TimeScaleManager.Instance.UpdateTimeScale(1f);
            else if (_gameState == GameState.Paused)
                TimeScaleManager.Instance.UpdateTimeScale(0f);
        }
    }

    public int CurrentLevel => SceneManager.GetActiveScene().buildIndex;

    #endregion

    private void OnEnable()
    {
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable boss)
            boss.OnDie += IBossFinishLevel_OnDie;
    }

    private void OnDisable()
    {
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable boss)
            boss.OnDie -= IBossFinishLevel_OnDie;
    }

    private void LevelFinish_OnAnyLevelCompleted() => _isLevelFinished = true;
    private void IBossFinishLevel_OnDie() => _isLevelFinished = true;

    public void FinishLevel()
    {
        OnLevelFinished?.Invoke();
    }

    public void LoadMenuScene()
    {
        OnChangingScene?.Invoke();
        TimeScaleManager.Instance.UpdateTimeScaleOnChangeScene();
        SceneManager.LoadScene(0);
    }

    public void LoadScene(int index)
    {
        OnChangingScene?.Invoke();
        TimeScaleManager.Instance.UpdateTimeScaleOnChangeScene();
        SceneManager.LoadScene(index);
    }
}
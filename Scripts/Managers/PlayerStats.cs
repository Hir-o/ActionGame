using System;
using System.Collections;
using Leon;
using UnityEngine;

public class PlayerStats : SceneSingleton<PlayerStats>
{
    public static event Action<int> OnIncreaseCoinAmount;
    public static event Action<int> OnIncreaseRareCoinAmount;
    public static event Action<float> OnGameplayTimer;

    [SerializeField] private int _coinsCollected;
    [SerializeField] private int _rareCoinsCollected;

    private bool _checkpointReachedCheck;
    private float _gameplayTimer;
    private bool  _hasTimerCountdownStarted;

    #region Properties

    public int CoinsCollected
    {
        get => _coinsCollected;
        set
        {
            _coinsCollected = value;
            OnIncreaseCoinAmount?.Invoke(_coinsCollected);
        }
    }

    public int RareCoinsCollected
    {
        get => _rareCoinsCollected;
        set
        {
            _rareCoinsCollected = value;
            OnIncreaseRareCoinAmount?.Invoke(_rareCoinsCollected);
        }
    }

    public float GameplayTimer
    {
        get => _gameplayTimer;
        set
        {
            _gameplayTimer = value;
            OnGameplayTimer?.Invoke(_gameplayTimer);
        }
    }

    #endregion

    private void OnEnable()
    {
        Coin.OnCollectCoin += AddCoin;
        RareCoin.OnCollectRareCoin += RareCoin_OnCollectRareCoin;
        MovementController.OnPlayerRun += MovementController_OnPlayerRun;
        Checkpoint.OnNewCheckpointReached += Checkpoint_OnNewCheckpointReached;
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
    }

    private void OnDisable()
    {
        Coin.OnCollectCoin -= AddCoin;
        RareCoin.OnCollectRareCoin -= RareCoin_OnCollectRareCoin;
        MovementController.OnPlayerRun -= MovementController_OnPlayerRun;
        Checkpoint.OnNewCheckpointReached -= Checkpoint_OnNewCheckpointReached;
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
    }

    private void MovementController_OnPlayerRun(bool isRunning)
    {
        if (_hasTimerCountdownStarted) return;
        StartCoroutine(StartTimerCoroutine(true));
    }

    private void AddCoin() => CoinsCollected++;

    public void ReduceCollectedCoinsAmount(int amount) => CoinsCollected -= amount;

    private void RareCoin_OnCollectRareCoin() => RareCoinsCollected++;

    private void Checkpoint_OnNewCheckpointReached(int checkpointIndex, Vector3 characterTransform) =>
        _checkpointReachedCheck = true;

    private void CharacterPlayerController_OnPlayerDied()
    {
        if (!_checkpointReachedCheck)
            GameplayTimer = 0f;
    }

    private void LevelFinish_OnAnyLevelCompleted() => StopAllCoroutines();

    private IEnumerator StartTimerCoroutine(bool gameStarted)
    {
        _hasTimerCountdownStarted = true;
        while (gameStarted)
        {
            yield return new WaitForSecondsRealtime(.1f);
            if (LevelManager.Instance.GameState == GameState.Paused) continue;
            GameplayTimer += .1f;
        }
    }
}
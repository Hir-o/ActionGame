
using Leon;
using UnityEngine;

public class LevelStats : SceneSingleton<LevelStats>
{
    [SerializeField] private int _completionTimerSeconds;
    [SerializeField, Range(0, 100)] private int _coinsThresholdPercentage;
    [SerializeField, Range(0, 100)] private int _specialCoinsThresholdPercentage;

    public bool GetTimerCompletion() => PlayerStats.Instance.GameplayTimer <= _completionTimerSeconds;

    public bool GetCoinsCollectedCompletion()
    {
        if (CoinsManager.Instance.TotalLevelCoinsAmount == 0) return false;
        float collectedCoinsPercentage =
            (float)PlayerStats.Instance.CoinsCollected / CoinsManager.Instance.TotalLevelCoinsAmount;
        collectedCoinsPercentage *= 100;
        return collectedCoinsPercentage >= _coinsThresholdPercentage;
    }

    public bool GetSpecialCoinsCollectedCompletion()
    {
        if (CoinsManager.Instance.TotalSpecialCoinsAmount == 0) return false;
        float collectedSpecialCoinsPercentage =
            (float)PlayerStats.Instance.RareCoinsCollected / CoinsManager.Instance.TotalSpecialCoinsAmount;
        collectedSpecialCoinsPercentage *= 100;
        return collectedSpecialCoinsPercentage >= _specialCoinsThresholdPercentage;
    }
}
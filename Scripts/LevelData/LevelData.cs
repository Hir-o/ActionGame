
using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] private bool _coinCollectedRewardGained;
    [SerializeField] private bool _timerRewardGained;
    [SerializeField] private bool _specialCoinRewardGained;

    public bool CoinCollectedRewardGained
    {
        get => _coinCollectedRewardGained;
        set => _coinCollectedRewardGained = value;
    }

    public bool TimerRewardGained
    {
        get => _timerRewardGained;
        set => _timerRewardGained = value;
    }

    public bool SpecialCoinRewardGained
    {
        get => _specialCoinRewardGained;
        set => _specialCoinRewardGained = value;
    }
}
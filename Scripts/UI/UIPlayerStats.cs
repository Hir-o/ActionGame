using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class UIPlayerStats : MonoBehaviour
{
    [BoxGroup("Coins"), SerializeField] private TextMeshProUGUI _tmpCoinsCollected;

    [BoxGroup("Rare Coins"), SerializeField]
    private TextMeshProUGUI _tmpRareCoinsCollected;

    [BoxGroup("Timer"), SerializeField] private TextMeshProUGUI _tmpGameplayTimer;

    private void OnEnable()
    {
        PlayerStats.OnIncreaseCoinAmount += PlayerStats_OnIncreaseCoinAmount;
        PlayerStats.OnIncreaseRareCoinAmount += PlayerStats_OnIncreaseRareCoinAmount;
        PlayerStats.OnGameplayTimer += PlayerStats_OnGameplayTimer;
    }

    private void OnDisable()
    {
        PlayerStats.OnIncreaseCoinAmount -= PlayerStats_OnIncreaseCoinAmount;
        PlayerStats.OnIncreaseRareCoinAmount -= PlayerStats_OnIncreaseRareCoinAmount;
        PlayerStats.OnGameplayTimer -= PlayerStats_OnGameplayTimer;
    }

    private void Start()
    {
        _tmpCoinsCollected.text = "0";
        _tmpRareCoinsCollected.text = "0";
        Assert.IsNotNull(_tmpCoinsCollected);
    } 

    private void PlayerStats_OnIncreaseCoinAmount(int coinsAmount) => _tmpCoinsCollected.text = $"{coinsAmount}";

    private void PlayerStats_OnIncreaseRareCoinAmount(int rareCoinsAmount) =>
        _tmpRareCoinsCollected.text = $"{rareCoinsAmount}";

    private void PlayerStats_OnGameplayTimer(float gameplayTimer) =>
        _tmpGameplayTimer.text = TimeConvert.ConvertToMMSS(gameplayTimer);
}
using System;
using Leon;
using NaughtyAttributes;
using UnityEngine;

public class LocalAchievementController : SceneSingleton<LocalAchievementController>
{
    public event Action<LocalAchievementData> OnUnlockNewAchievement;

    [SerializeField] private LocalAchievementData _coinCollector;
    [SerializeField] private LocalAchievementData _evilFighter;
    [SerializeField] private LocalAchievementData _grandCanyon;
    [SerializeField] private LocalAchievementData _iDontNeedRoads;
    [SerializeField] private LocalAchievementData _itsHotInHere;
    [SerializeField] private LocalAchievementData _jumpKing;
    [SerializeField] private LocalAchievementData _pirateCove;
    [SerializeField] private LocalAchievementData _skyline;
    [SerializeField] private LocalAchievementData _starGazer;
    [SerializeField] private LocalAchievementData _yoloHero;

    private void OnEnable()
    {
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
    }

    private void OnDisable()
    {
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
    }

    private int _count = 0;
    
    [Button("Test")]
    private void LevelFinish_OnAnyLevelCompleted()
    {
        // for testing purposes only
        //todo implement logic
        //todo delete the line below
        if (_count == 0)
            OnUnlockNewAchievement?.Invoke(_coinCollector);
        else if (_count == 1)
            OnUnlockNewAchievement?.Invoke(_evilFighter);
        else if (_count == 1)
            OnUnlockNewAchievement?.Invoke(_grandCanyon);
        else
            OnUnlockNewAchievement?.Invoke(_yoloHero);

        _count++;
    }
}

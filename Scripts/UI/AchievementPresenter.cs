
using UnityEngine;

public class AchievementPresenter : MonoBehaviour
{
    [SerializeField] private AchievementGainTweener _achievementGainTweener;

    private void OnEnable()
    {
        if (LocalAchievementController.Instance != null)
            LocalAchievementController.Instance.OnUnlockNewAchievement +=
                LocalAchievementController_OnUnlockNewAchievement;
    }

    private void OnDisable()
    {
        if (LocalAchievementController.Instance != null)
            LocalAchievementController.Instance.OnUnlockNewAchievement -=
                LocalAchievementController_OnUnlockNewAchievement;
    }

    private void LocalAchievementController_OnUnlockNewAchievement(LocalAchievementData localAchievementData) =>
        _achievementGainTweener.AddToQueue(localAchievementData);
}
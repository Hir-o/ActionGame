using UnityEngine;

[CreateAssetMenu(fileName = "LocalAchievementData", menuName = "Achievements/LocalAchievement", order = 0)]
public class LocalAchievementData : BaseAchievement
{
    [SerializeField] private Sprite _iconSprite;
    [SerializeField] private Sprite _backgroundSprite;
    [SerializeField] private Sprite _incompleteSprite;
    [SerializeField] private Sprite _completeSprite;

    #region Properties

    public Sprite IconSprite => _iconSprite;
    public Sprite BackgroundSprite => _backgroundSprite;
    public Sprite IncompleteSprite => _incompleteSprite;
    public Sprite CompleteSprite => _completeSprite;

    #endregion
}
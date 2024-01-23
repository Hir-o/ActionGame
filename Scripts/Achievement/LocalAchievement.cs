using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalAchievement : MonoBehaviour
{
    #region Variable Declaration

    [BoxGroup("Local Achievement"), SerializeField]
    private LocalAchievementData _localAchievementData;

    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _nameText;
    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _descriptionText;
    [BoxGroup("UI"), SerializeField] private Image _iconImage;
    [BoxGroup("UI"), SerializeField] private Image _backgroundImage;
    [BoxGroup("UI"), SerializeField] private Image _achievementStateImage;
    [BoxGroup("UI"), SerializeField] private Image _glowImage;

    private bool _isCompleted;

    #endregion

    private void Start()
    {
        LoadData();
        UpdateAchievementUIComponents();
    }

    private void LoadData() =>
        _isCompleted = PlayerPrefs.GetInt(_localAchievementData.name) == 1;

    private void UpdateAchievementUIComponents()
    {
        _nameText.text = _localAchievementData.Name;
        _descriptionText.text = _localAchievementData.Description;
        _iconImage.sprite = _localAchievementData.IconSprite;
        _backgroundImage.sprite = _localAchievementData.BackgroundSprite;
        _achievementStateImage.sprite = _isCompleted
            ? _localAchievementData.CompleteSprite
            : _localAchievementData.IncompleteSprite;
        _glowImage.gameObject.SetActive(_isCompleted);
    }
}
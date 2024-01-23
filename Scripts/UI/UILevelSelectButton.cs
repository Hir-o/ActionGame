
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelectButton : MonoBehaviour
{
    [SerializeField] private int _levelIndex;

    [SerializeField] private TextMeshProUGUI _levelNumberText;

    [BoxGroup("Stars"), SerializeField] private Image _leftStarImage;
    [BoxGroup("Stars"), SerializeField] private Image _centerStarImage;
    [BoxGroup("Stars"), SerializeField] private Image _rightStarImage;

    [BoxGroup("LevelStarRewardData"), SerializeField]
    private LevelStarRewardData _levelStarRewardData;

    private void Awake()
    {
        if (_levelNumberText == null) return;
        _levelNumberText.text = _levelIndex.ToString();
    }

    private void Start() => UpdateStars();

    public void OnClickButton() => UILevelSelection.Instance.HandleLevelSelectionButtonClick(_levelIndex);

    public void UpdateStars()
    {
        if (LevelDataManager.Instance == null) return;
        if (LevelDataManager.Instance.LevelDataList.Count < _levelIndex - 1) return;
        LevelData _levelData = LevelDataManager.Instance.LevelDataList[_levelIndex - 1];
        _leftStarImage.sprite = _levelData.CoinCollectedRewardGained
            ? _levelStarRewardData.StarOnSprite
            : _levelStarRewardData.StarOffSprite;
        _centerStarImage.sprite = _levelData.TimerRewardGained
            ? _levelStarRewardData.StarOnSprite
            : _levelStarRewardData.StarOffSprite;
        _rightStarImage.sprite = _levelData.SpecialCoinRewardGained
            ? _levelStarRewardData.StarOnSprite
            : _levelStarRewardData.StarOffSprite;
    }
}

using System;
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelFinish : SceneSingleton<UILevelFinish>
{
    public event Action OnClickHomeButton;
    public event Action OnClickRestartButton;
    public event Action OnClickNextLevelButton;

    [BoxGroup("Text"), SerializeField] private TextMeshProUGUI _coinsText;
    [BoxGroup("Text"), SerializeField] private TextMeshProUGUI _timeText;
    [BoxGroup("Text"), SerializeField] private TextMeshProUGUI _gemsText;

    [BoxGroup("Buttons"), SerializeField] private Button _homeButton;
    [BoxGroup("Buttons"), SerializeField] private Button _restartButton;
    [BoxGroup("Buttons"), SerializeField] private Button _nextLevelButton;

    [BoxGroup("Starts"), SerializeField] private Image _starsImageLeft;
    [BoxGroup("Starts"), SerializeField] private Image _starsImageCenter;
    [BoxGroup("Starts"), SerializeField] private Image _starsImageRight;

    [BoxGroup("Title"), SerializeField] private TextMeshProUGUI _titleText;

    private LevelFinishPopupTween _levelFinishPopupTween;
    private Canvas _canvas;
    private GraphicRaycaster _graphicRaycaster;
    private CanvasGroup _mainCanvasGroup;

    protected override void Awake()
    {
        base.Awake();
        _levelFinishPopupTween = GetComponentInChildren<LevelFinishPopupTween>();
        _canvas = GetComponent<Canvas>();
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _mainCanvasGroup = GetComponent<CanvasGroup>();

        _canvas.enabled = false;
        _graphicRaycaster.enabled = false;
        _mainCanvasGroup.alpha = 0f;

        _starsImageLeft.DOFade(0f, 0f);
        _starsImageCenter.DOFade(0f, 0f);
        _starsImageRight.DOFade(0f, 0f);

        _titleText.text = $"Level {LevelManager.Instance.CurrentLevel}";
    }

    private void OnEnable()
    {
        if (CelebrationAnimationController.Instance != null)
            CelebrationAnimationController.Instance.OnCelebrationFinished +=
                CelebrationAnimationController_OnCelebrationFinished;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable boss)
            boss.OnDie += IBossFinishLevel_OnAnyLevelCompleted;

        _homeButton.onClick.AddListener(HomeButton);
        _restartButton.onClick.AddListener(RestartButton);
        _nextLevelButton.onClick.AddListener(NextLevelButton);
    }

    private void OnDisable()
    {
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable boss)
            boss.OnDie -= IBossFinishLevel_OnAnyLevelCompleted;

        _homeButton.onClick.RemoveListener(HomeButton);
        _restartButton.onClick.RemoveListener(RestartButton);
        _nextLevelButton.onClick.RemoveListener(NextLevelButton);
    }

    private void CelebrationAnimationController_OnCelebrationFinished()
    {
        OpenLevelFinishPanel();
        if (CelebrationAnimationController.Instance != null)
            CelebrationAnimationController.Instance.OnCelebrationFinished -=
                CelebrationAnimationController_OnCelebrationFinished;
    }

    private void IBossFinishLevel_OnAnyLevelCompleted() => OpenLevelFinishPanel();

    private void OpenLevelFinishPanel()
    {
        if (LevelDataManager.Instance != null)
        {
            LevelDataManager.Instance.SaveData();
        }
        else
        {
            Debug.LogError(
                "LevelDataManager does not contain an instance. Make sure to run the game from the main menu!");
        }

        _levelFinishPopupTween.AnimateOpening();
        UpdateCompletionText();
        _canvas.enabled = true;
        _graphicRaycaster.enabled = true;
        _mainCanvasGroup.alpha = 1f;
    }

    private void UpdateCompletionText()
    {
        _coinsText.text = PlayerStats.Instance.CoinsCollected.ToString();
        _timeText.text = TimeConvert.ConvertToMMSS(PlayerStats.Instance.GameplayTimer);
        _gemsText.text = PlayerStats.Instance.RareCoinsCollected.ToString();
    }

    private void HomeButton()
    {
        OnClickHomeButton?.Invoke();
        _graphicRaycaster.enabled = false;
    }

    private void RestartButton()
    {
        OnClickRestartButton?.Invoke();
        _graphicRaycaster.enabled = false;
    }

    private void NextLevelButton()
    {
        OnClickNextLevelButton?.Invoke();
        _graphicRaycaster.enabled = false;
    }
}
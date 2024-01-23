
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    private Tween _delayTransitionTween;
    [SerializeField] private Button _pauseButton;

    [BoxGroup("PauseMenu"), SerializeField]
    private GameObject _pauseMenu;

    [BoxGroup("Dimmed"), SerializeField] private CanvasGroup _dimmedCanvasGroup;
    [BoxGroup("Dimmed"), SerializeField] private float _dimmedFadeDuration = .5f;
    [BoxGroup("Dimmed"), SerializeField] private Ease _dimmedFadeEase = Ease.OutQuad;

    [BoxGroup("Title"), SerializeField] private CanvasGroup _titleCanvasGroup;
    [BoxGroup("Title"), SerializeField] private float _titleFadeDuration = .5f;
    [BoxGroup("Title"), SerializeField] private Ease _titleFadeEase = Ease.OutQuad;
    [BoxGroup("Title"), SerializeField] private float _titleFadeDelay = 1f;

    [BoxGroup("Current Level"), SerializeField]
    private TextMeshProUGUI _currLevelText;
    
    [BoxGroup("Current Level"), SerializeField]
    private CanvasGroup _currLevelCanvasGroup;

    [BoxGroup("Current Level"), SerializeField]
    private float _currLevelFadeDuration = .5f;

    [BoxGroup("Current Level"), SerializeField]
    private Ease _currLevelFadeEase = Ease.OutQuad;

    [BoxGroup("Current Level"), SerializeField]
    private float _currLevelFadeDelay = 1f;

    [BoxGroup("Buttons Background"), SerializeField]
    private CanvasGroup _buttonBackgroundCanvasGroup;

    [BoxGroup("Buttons Background"), SerializeField]
    private float _buttonBackgroundFadeDuration = .5f;

    [BoxGroup("Buttons Background"), SerializeField]
    private Ease _buttonBackgroundFadeEase = Ease.OutQuad;

    [BoxGroup("Buttons Background"), SerializeField]
    private float _buttonBackgroundFadeDelay = 1f;

    [BoxGroup("Buttons"), SerializeField] private Button _homeUIButton;
    [BoxGroup("Buttons"), SerializeField] private Button _restartUIButton;
    [BoxGroup("Buttons"), SerializeField] private Button _resumeGameUIButton;

    [BoxGroup("Button Tweeners"), SerializeField]
    private FinishPanelButtonTweener _homeButton;

    [BoxGroup("Button Tweeners"), SerializeField]
    private FinishPanelButtonTweener _restartButton;

    [BoxGroup("Button Tweeners"), SerializeField]
    private FinishPanelButtonTweener _resumeGameButton;

    [BoxGroup("Master Volume"), SerializeField]
    private float _masterVolume = 0.3f;
    
    private GraphicRaycaster _graphicRaycaster;

    private void Awake()
    {
        ResetComponents();
        
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _pauseButton.onClick.AddListener(OnClickPauseButton);
        _homeUIButton.onClick.AddListener(OnClickHomeButton);
        _restartUIButton.onClick.AddListener(OnClickRestartButton);
        _resumeGameUIButton.onClick.AddListener(OnClickResumeGameButton);

        _currLevelText.text = $"Level {LevelManager.Instance.CurrentLevel}";
    }
    
    private void OnEnable()
    {
        if (UILevelFinish.Instance != null)
        {
            UILevelFinish.Instance.OnClickHomeButton += UILevelFinish_OnClickHomeButton;
            UILevelFinish.Instance.OnClickRestartButton += UILevelFinish_OnClickRestartButton;
            UILevelFinish.Instance.OnClickNextLevelButton += UILevelFinish_OnClickNextLevelButton;
        }
        
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
    }

    private void OnDisable()
    {
        if (UILevelFinish.Instance != null)
        {
            UILevelFinish.Instance.OnClickHomeButton -= UILevelFinish_OnClickHomeButton;
            UILevelFinish.Instance.OnClickRestartButton -= UILevelFinish_OnClickRestartButton;
            UILevelFinish.Instance.OnClickNextLevelButton -= UILevelFinish_OnClickNextLevelButton;
        }
        
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
    }

    private void UILevelFinish_OnClickHomeButton() => TransitionToMenuScene();
    private void UILevelFinish_OnClickRestartButton() => CustomTransistion.Instance.EnableTransitionIn(RestartLevel);
    private void UILevelFinish_OnClickNextLevelButton() => StartTransitionAnimation();
    private void LevelFinish_OnAnyLevelCompleted() => _pauseButton.interactable = false;

    private void ResetComponents()
    {
        _dimmedCanvasGroup.alpha = 0f;
        _titleCanvasGroup.alpha = 0f;
        _currLevelCanvasGroup.alpha = 0f;
        _buttonBackgroundCanvasGroup.alpha = 0f;
        
        _homeButton.Initialize();
        _restartButton.Initialize();
        _resumeGameButton.Initialize();

        _pauseMenu.SetActive(false);
    }
    
    private void OnClickPauseButton()
    {
        if (CustomTransistion.Instance.IsTransitioning) return;
        if (!CustomTransistion.Instance.IsTransitionFinished) return;
        if (SoundManager.Instance != null) SoundManager.Instance.UpdateMasterVolume(_masterVolume);
        LevelManager.Instance.GameState = GameState.Paused;
        _pauseMenu.SetActive(true);
        _dimmedCanvasGroup.DOFade(1f, _dimmedFadeDuration).SetEase(_dimmedFadeEase).SetUpdate(true);
        _titleCanvasGroup.DOFade(1f, _currLevelFadeDuration).SetEase(_titleFadeEase).SetDelay(_titleFadeDelay)
            .SetUpdate(true)
            .OnComplete(AnimateItems);
        _buttonBackgroundCanvasGroup.DOFade(1f, _buttonBackgroundFadeDuration).SetEase(_buttonBackgroundFadeEase)
            .SetDelay(_buttonBackgroundFadeDelay).SetUpdate(true);
        _currLevelCanvasGroup.DOFade(1f, _titleFadeDuration).SetEase(_currLevelFadeEase).SetDelay(_currLevelFadeDelay)
            .SetUpdate(true);
    }

    private void OnClickHomeButton()
    {
        TransitionToMenuScene();
        LevelManager.Instance.GameState = GameState.Paused;
        _graphicRaycaster.enabled = false;
    }

    private void OnClickRestartButton()
    {
        CustomTransistion.Instance.EnableTransitionIn(RestartLevel);
        LevelManager.Instance.GameState = GameState.Paused;
        _graphicRaycaster.enabled = false;
    }

    private void OnClickResumeGameButton()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.UpdateMasterVolume(1f);
        _homeButton.ButtonCanvasGroup.blocksRaycasts = false;
        _restartButton.ButtonCanvasGroup.blocksRaycasts = false;
        _resumeGameButton.ButtonCanvasGroup.blocksRaycasts = false;
        _dimmedCanvasGroup.DOFade(0f, _dimmedFadeDuration).SetEase(_dimmedFadeEase).SetUpdate(true)
            .OnComplete(() =>
            {
                LevelManager.Instance.GameState = GameState.Playing;
                ResetComponents();
            });
    }

    private void AnimateItems()
    {
        _homeButton.Animate();
        _restartButton.Animate();
        _resumeGameButton.Animate();
    }

    public void OnClickMenu()
    {
        MovementController.Instance.ResetMovement();
        TransitionToMenuScene();
    }


    private void TransitionToMenuScene()
    {
        TimeScaleManager.Instance.UpdateTimeScale(1f);
        CustomTransistion.Instance.EnableTransitionIn(LoadMenuScene);
    }

    private void StartTransitionAnimation(float delay = 0f)
    {
        if (_delayTransitionTween != null) _delayTransitionTween.Kill();
        _delayTransitionTween = DOVirtual.Float(1f, 0f, delay, value => { }).OnComplete(() =>
        {
            TimeScaleManager.Instance.UpdateTimeScale(1f);
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int nextLevelIndex = currentLevel + 1;
            if (nextLevelIndex > SceneManager.sceneCountInBuildSettings - 1)
                CustomTransistion.Instance.EnableTransitionIn(LoadMenuScene);
            else
                CustomTransistion.Instance.EnableTransitionIn(() => LoadNextLevel(nextLevelIndex));
        });
    }

    private void LoadMenuScene() => LevelManager.Instance.LoadMenuScene();
    private void LoadNextLevel(int nextLevelIndex) => LevelManager.Instance.LoadScene(nextLevelIndex);
    private void RestartLevel() => LevelManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
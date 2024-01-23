
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSelect : SceneSingleton<UIStageSelect>
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [BoxGroup("Stages Buttons"), SerializeField]
    private UIStageButton[] _stageButtonsArray;

    [BoxGroup("Tweening"), SerializeField] private float _fadeSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fadeEase = Ease.OutQuad;

    [SerializeField] private Button _backButton;

    [SerializeField] private int _unlockedStages;

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _backButton.onClick.AddListener(OnClickBackButton);
        LoadData();
        UpdateStageButtons();
    }

    public void OnClickBackButton() => FadeOut();

    public void HandleStageSelectionButtonClick(int index) => FadeOut(false, index);

    private void UpdateStageButtons()
    {
        for (int i = 0; i < _stageButtonsArray.Length; i++)
            _stageButtonsArray[i].UpdateActiveState(_stageButtonsArray[i].Index <= _unlockedStages);
    }

    public void FadeIn()
    {
        for (int i = 0; i < _stageButtonsArray.Length; i++)
        {
            _stageButtonsArray[i].StageSelectUITweener.Reset();
            _stageButtonsArray[i].StageSelectUITweener.PlayAnimation();
        }
        
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }

    public void FadeOut(bool isBackButtonClicked = true, int stageIndex = 0)
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase)
            .OnComplete(() =>
            {
                if (isBackButtonClicked)
                    UIMenu.Instance.FadeIn();
                else
                    UILevelSelection.Instance.FadeIn(stageIndex);

                for (int i = 0; i < _stageButtonsArray.Length; i++)
                    _stageButtonsArray[i].StageSelectUITweener.Reset();
            });
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.UnlockedStages))
            _unlockedStages = PlayerPrefs.GetInt(PlayerPrefsConstants.UnlockedStages);
    }
}
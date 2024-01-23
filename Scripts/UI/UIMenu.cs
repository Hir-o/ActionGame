
using System;
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : SceneSingleton<UIMenu>
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;

    [BoxGroup("Tweening"), SerializeField] private float _fadeSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fadeEase = Ease.OutQuad;

    [BoxGroup("Buttons"), SerializeField] private Button _achievementsButton;
    [BoxGroup("Buttons"), SerializeField] private Button _collectablesButton;

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _playButton.onClick.AddListener(OnClickPlayButton);
        _settingsButton.onClick.AddListener(OnClickSettingsButton);
        _achievementsButton.onClick.AddListener(OnClickAchievementsButton);
        _collectablesButton.onClick.AddListener(OnClickCollectablesButton);
    }

    public void OnClickPlayButton() => FadeOut(UIPanelType.Stages);
    private void OnClickSettingsButton() => FadeOut(UIPanelType.Settings);
    private void OnClickAchievementsButton() => FadeOut(UIPanelType.Achievements);
    private void OnClickCollectablesButton() => FadeOut(UIPanelType.Collectables);

    public void FadeIn()
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }

    public void FadeOut(UIPanelType panelType)
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase)
            .OnStart(() =>
            {
                if (panelType == UIPanelType.Settings)
                    UISettings.Instance.FadeIn();
            })
            .OnComplete(() =>
            {
                switch (panelType)
                {
                    case UIPanelType.Stages:
                        UIStageSelect.Instance.FadeIn();
                        break;
                    case UIPanelType.Achievements:
                        UIAchievements.Instance.FadeIn();
                        break;
                    case UIPanelType.Collectables:
                        UICollectables.Instance.FadeIn();
                        break;
                }
            });
    }
}
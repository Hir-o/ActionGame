
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class LevelFinishPopupTween : MonoBehaviour
{
    [BoxGroup("Size Delta"), SerializeField]
    private Vector2 _initSizeDelta;

    [BoxGroup("Size Delta"), SerializeField]
    private Vector2 _finalSizeDelta;

    [BoxGroup("Dimmed"), SerializeField] private CanvasGroup _dimmedCanvasGroup;
    [BoxGroup("Dimmed"), SerializeField] private float _dimmedFadeDuration = .5f;
    [BoxGroup("Dimmed"), SerializeField] private Ease _dimmedFadeEase = Ease.OutQuad;

    [BoxGroup("Popup"), SerializeField] private RectTransform _popupRectTransform;
    [BoxGroup("Popup"), SerializeField] private CanvasGroup _popupCanvasGroup;
    [BoxGroup("Popup"), SerializeField] private float _popupFadeDuration = .5f;
    [BoxGroup("Popup"), SerializeField] private float _popupSizeDuration = 1f;
    [BoxGroup("Popup"), SerializeField] private Ease _popupFadeEase = Ease.OutQuad;
    [BoxGroup("Popup"), SerializeField] private float _popupDelay = .4f;

    [BoxGroup("Title"), SerializeField] private CanvasGroup _titleCanvasGroup;
    [BoxGroup("Title"), SerializeField] private float _titleFadeDuration = .5f;
    [BoxGroup("Title"), SerializeField] private Ease _titleFadeEase = Ease.OutQuad;
    [BoxGroup("Title"), SerializeField] private float _titleFadeDelay = 1.25f;

    [BoxGroup("StarTweeners"), SerializeField]
    private StarTweener _leftStartweener;

    [BoxGroup("StarTweeners"), SerializeField]
    private StarTweener _centerStarTweener;

    [BoxGroup("StarTweeners"), SerializeField]
    private StarTweener _rightStarTweener;

    [BoxGroup("Buttons"), SerializeField] private FinishPanelButtonTweener _homeButton;
    [BoxGroup("Buttons"), SerializeField] private FinishPanelButtonTweener _restartButton;
    [BoxGroup("Buttons"), SerializeField] private FinishPanelButtonTweener _nextLevelButton;

    private void Awake()
    {
        _dimmedCanvasGroup.alpha = 0f;
        _popupCanvasGroup.alpha = 0f;
        _titleCanvasGroup.alpha = 0f;
        _popupRectTransform.sizeDelta = _initSizeDelta;

        _leftStartweener.Initialize();
        _centerStarTweener.Initialize();
        _rightStarTweener.Initialize();
        _homeButton.Initialize();
        _restartButton.Initialize();
        _nextLevelButton.Initialize();
    }

    public void AnimateOpening()
    {
        _dimmedCanvasGroup.DOFade(1f, _dimmedFadeDuration).SetEase(_dimmedFadeEase);
        _popupCanvasGroup.DOFade(1f, _popupFadeDuration).SetEase(_popupFadeEase).SetDelay(_popupDelay);
        _popupRectTransform.DOSizeDelta(_finalSizeDelta, _popupSizeDuration)
            .SetEase(EasePresets.Instance.BounceOnceEaseLight)
            .SetDelay(_popupDelay);
        _titleCanvasGroup.DOFade(1f, _titleFadeDuration).SetEase(_titleFadeEase).SetDelay(_titleFadeDelay)
            .OnComplete(AnimateItems);
    }

    private void AnimateItems()
    {
        bool _coinsStarCollected = LevelStats.Instance.GetCoinsCollectedCompletion();
        bool _timerStarCollected = LevelStats.Instance.GetTimerCompletion();
        bool _specialCoinsStarCollected = LevelStats.Instance.GetSpecialCoinsCollectedCompletion();
        _leftStartweener.Animate(_coinsStarCollected);
        _centerStarTweener.Animate(_timerStarCollected);
        _rightStarTweener.Animate(_specialCoinsStarCollected);
        _homeButton.Animate();
        _restartButton.Animate();
        _nextLevelButton.Animate();
    }
}
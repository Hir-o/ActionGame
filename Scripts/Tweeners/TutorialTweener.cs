using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using System;

public class TutorialTweener : MonoBehaviour
{
    #region Variable Declaration

    [BoxGroup("Fade"), SerializeField] private CanvasGroup _tutorialCanvasGroup;
    [BoxGroup("Fade"), SerializeField] private float _fadeDuration = 0.5f;
    [BoxGroup("Fade"), SerializeField] private Ease _fadeEase = Ease.OutQuad;

    [BoxGroup("Popup"), SerializeField] private CanvasGroup _popupCanvasGroup;
    [BoxGroup("Popup"), SerializeField] private float _popupFadeDuration = 0.4f;
    [BoxGroup("Popup"), SerializeField] private float _popupDelayDuration = 0.5f;
    [BoxGroup("Fade"), SerializeField] private Ease _popupFadeEase = Ease.OutQuad;

    [BoxGroup("Bars"), SerializeField] private RectTransform _topBarRectTransform;
    [BoxGroup("Bars"), SerializeField] private RectTransform _bottomBarRectTransform;
    [BoxGroup("Bars"), SerializeField] private float _barSizeDelta = 140f;
    [BoxGroup("Bars"), SerializeField] private float _barDuration = .5f;
    [BoxGroup("Bars"), SerializeField] private float _barDelay = .7f;
    [BoxGroup("Bars"), SerializeField] private Ease _barEase = Ease.Linear;

    private bool _canTweenFade;
    private bool _canTweenBars;
    private bool _canTweenPopup;

    private UIImprovedTutorial _uiImprovedTutorial;
    private Tween _fadeTween;
    private Tween _topBarTween;
    private Tween _bottomBarTween;
    private Tween _popupFadeTween;

    #endregion

    private void Awake()
    {
        _uiImprovedTutorial = GetComponent<UIImprovedTutorial>();

        _canTweenFade = _tutorialCanvasGroup != null;
        _canTweenPopup = _popupCanvasGroup != null;
        _canTweenBars = _topBarRectTransform != null && _bottomBarRectTransform != null;

        if (_canTweenFade) _tutorialCanvasGroup.alpha = 0f;
        if (_canTweenPopup) _popupCanvasGroup.alpha = 0f;
        if (_canTweenBars)
        {
            _topBarRectTransform.sizeDelta = Vector2.zero;
            _bottomBarRectTransform.sizeDelta = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        _uiImprovedTutorial.OnTutorialShow += UIImprovedTutorial_OnTutorialShow;
        _uiImprovedTutorial.OnTutorialFinished += UIImprovedTutorial_OnTutorialFinished;
    }

    private void UIImprovedTutorial_OnTutorialShow(Action callback) => ShowTutorial(callback);
    private void UIImprovedTutorial_OnTutorialFinished() => HideTutorial();

    private void ShowTutorial(Action callback)
    {
        if (_canTweenFade) TweenFading();
        if (_canTweenBars) TweenBars();
        if (_canTweenPopup) TweenPopupFading();

        float timeToShowTutorial = _popupDelayDuration + _popupFadeDuration;
        DOVirtual.DelayedCall(timeToShowTutorial, () => callback?.Invoke());
    }

    private void HideTutorial()
    {
        if (_canTweenFade) TweenFading(true);
        if (_canTweenBars) TweenBars(true);
        if (_canTweenPopup) TweenPopupFading(true);
    }

    private void TweenFading(bool playBackwards = false)
    {
        if (_fadeTween != null) _fadeTween.Kill();
        float endValue = playBackwards ? 0f : 1f;
        _fadeTween = _tutorialCanvasGroup.DOFade(endValue, _fadeDuration)
            .SetEase(_fadeEase)
            .SetUpdate(true);
    }

    private void TweenBars(bool playBackwards = false)
    {
        if (_topBarTween != null) _topBarTween.Kill();
        if (_bottomBarTween != null) _bottomBarTween.Kill();

        Vector2 endValue = playBackwards ? Vector2.zero : new Vector2(0f, _barSizeDelta);
        _topBarTween = _topBarRectTransform.DOSizeDelta(endValue, _barDuration)
            .SetDelay(_barDelay)
            .SetEase(_barEase)
            .SetUpdate(true);

        _bottomBarTween = _bottomBarRectTransform.DOSizeDelta(endValue, _barDuration)
            .SetDelay(_barDelay)
            .SetEase(_barEase)
            .SetUpdate(true);
    }

    private void TweenPopupFading(bool playBackwards = false)
    {
        if (_popupFadeTween != null) _popupFadeTween.Kill();
        float endValue = playBackwards ? 0f : 1f;
        _popupFadeTween = _popupCanvasGroup.DOFade(endValue, _popupFadeDuration)
            .SetEase(_popupFadeEase)
            .SetDelay(_popupDelayDuration)
            .SetUpdate(true);
    }
}
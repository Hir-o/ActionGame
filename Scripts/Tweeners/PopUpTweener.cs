
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PopUpTweener : MonoBehaviour
{
    [BoxGroup("SizeDelta"), SerializeField]
    private Vector2 _initSizeDelta;

    [BoxGroup("SizeDelta"), SerializeField]
    private Vector2 _finalSizeDelta;

    [BoxGroup("Dimmed"), SerializeField] private CanvasGroup _dimmedCanvasGroup;
    [BoxGroup("Dimmed"), SerializeField] private float _dimmedFadeDuration = .3f;
    [BoxGroup("Dimmed"), SerializeField] private Ease _dimmedFadeEase = Ease.OutQuad;

    [BoxGroup("Popup"), SerializeField] private RectTransform _popupRectTransform;
    [BoxGroup("Popup"), SerializeField] private CanvasGroup _popupCanvasGroup;
    [BoxGroup("Popup"), SerializeField] private float _popupFadeDuration = .5f;
    [BoxGroup("Popup"), SerializeField] private float _popupSizeDuration = .65f;
    [BoxGroup("Popup"), SerializeField] private Ease _popupFadeEase = Ease.OutQuad;
    [BoxGroup("Popup"), SerializeField] private float _popupDelay = .25f;

    [BoxGroup("Content"), SerializeField] private CanvasGroup _contentCanvasGroup;
    [BoxGroup("Content"), SerializeField] private float _contentFadeDuration = 0.5f;
    [BoxGroup("Content"), SerializeField] private Ease _contentFadeEase = Ease.OutQuad;

    [BoxGroup("Button Pop In Tweeners"), SerializeField]
    private ButtonPopInTweener _quitPopInTweener;

    [BoxGroup("Button Pop In Tweeners"), SerializeField]
    private ButtonPopInTweener _closePopInTweener;

    public void Reset()
    {
        _dimmedCanvasGroup.alpha = 0f;
        _popupCanvasGroup.alpha = 0f;
        _contentCanvasGroup.alpha = 0f;
        _popupRectTransform.sizeDelta = _initSizeDelta;
    }

    public void AnimateOpening()
    {
        KillTweens();
        _contentCanvasGroup.blocksRaycasts = true;
        _quitPopInTweener.Reset();
        _closePopInTweener.Reset();
        _dimmedCanvasGroup.DOFade(1f, _dimmedFadeDuration).SetEase(_dimmedFadeEase);
        _popupCanvasGroup.DOFade(1f, _popupFadeDuration).SetEase(_popupFadeEase).SetDelay(_popupDelay);
        _popupRectTransform.DOSizeDelta(_finalSizeDelta, _popupSizeDuration)
            .SetEase(EasePresets.Instance.BounceOnceEaseLight)
            .SetDelay(_popupDelay)
            .OnComplete(() =>
            {
                _contentCanvasGroup.DOFade(1f, _contentFadeDuration).SetEase(_contentFadeEase).OnComplete(() =>
                {
                    _quitPopInTweener.Animate();
                    _closePopInTweener.Animate();
                });
            });
    }

    public void AnimateClosing(Action callback)
    {
        KillTweens();
        _contentCanvasGroup.blocksRaycasts = false;
        _contentCanvasGroup.DOFade(0f, _contentFadeDuration).SetEase(_contentFadeEase).OnComplete(() =>
        {
            _dimmedCanvasGroup.DOFade(0f, _dimmedFadeDuration).SetEase(_dimmedFadeEase)
                .OnComplete(() => callback?.Invoke());
            _popupCanvasGroup.DOFade(0f, _dimmedFadeDuration).SetEase(_dimmedFadeEase);
        });
    }

    private void KillTweens()
    {
        if (DOTween.IsTweening(_dimmedCanvasGroup)) DOTween.Kill(_dimmedCanvasGroup);
        if (DOTween.IsTweening(_popupCanvasGroup)) DOTween.Kill(_popupCanvasGroup);
        if (DOTween.IsTweening(_contentCanvasGroup)) DOTween.Kill(_contentCanvasGroup);
        if (DOTween.IsTweening(_popupRectTransform)) DOTween.Kill(_popupRectTransform);
    }
}
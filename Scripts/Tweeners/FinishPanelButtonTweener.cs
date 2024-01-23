
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FinishPanelButtonTweener
{
    [SerializeField] private Vector2 _initSizeDelta = new Vector2(0f, 0f);
    [SerializeField] private Vector2 _finalSizeDelta = new Vector2(152f, 185f);
    [SerializeField] private float _sizeDuration = .75f;
    [SerializeField] private float _sizeDelay;
    [SerializeField] private RectTransform _buttonRectTransform;
    [SerializeField] private CanvasGroup _buttonCanvasGroup;
    [SerializeField] private Image _iconImage;
    [SerializeField] private float _fadeDuration = .75f;
    [SerializeField] private Ease _fadeEase = Ease.OutCubic;
    [SerializeField] private float _fadeDelay;

    #region Properties

    public CanvasGroup ButtonCanvasGroup => _buttonCanvasGroup;

    #endregion

    public void Initialize()
    {
        _buttonCanvasGroup.blocksRaycasts = false;
        _iconImage.DOFade(0f, 0f).SetUpdate(true);
        _buttonRectTransform.sizeDelta = _initSizeDelta;
    }

    public void Animate()
    {
        _buttonRectTransform.DOSizeDelta(_finalSizeDelta, _sizeDuration).SetEase(EasePresets.Instance.BounceOnceEase)
            .SetDelay(_sizeDelay).SetUpdate(true);
        _iconImage.DOFade(1f, _fadeDuration).SetEase(_fadeEase).SetDelay(_fadeDelay).SetUpdate(true)
            .OnComplete(() => _buttonCanvasGroup.blocksRaycasts = true);
    }

    public void Reset()
    {
        _buttonCanvasGroup.blocksRaycasts = false;
        _iconImage.DOFade(0f, 0f).SetUpdate(true);
        _buttonRectTransform.sizeDelta = _initSizeDelta;
    }
}

using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StarTweener
{
    [BoxGroup("Star"), SerializeField] private Vector2 _initSizeDelta = new Vector2(300f, 300f);
    [BoxGroup("Star"), SerializeField] private Vector2 _finalSizeDelta = new Vector2(250f, 250f);
    [BoxGroup("Star"), SerializeField] private float _sizeDuration = .5f;
    [BoxGroup("Star"), SerializeField] private Ease _sizeEase = Ease.OutQuad;
    [BoxGroup("Star"), SerializeField] private float _sizeDelay = 1f;
    [BoxGroup("Star"), SerializeField] private RectTransform _rectTransformFg;
    [BoxGroup("Star"), SerializeField] private Image _starImage;
    [BoxGroup("Star"), SerializeField] private float _starFadeDuration = .5f;
    [BoxGroup("Star"), SerializeField] private Ease _starFadeEase = Ease.OutCubic;
    [BoxGroup("Star"), SerializeField] private float _starFadeDelay;

    [BoxGroup("Fade"), SerializeField] private Image _image;
    [BoxGroup("Fade"), SerializeField] private float _fadeDuration = .5f;
    [BoxGroup("Fade"), SerializeField] private Ease _fadeEase = Ease.OutQuad;
    [BoxGroup("Fade"), SerializeField] private float _fadeDelay;

    [BoxGroup("Text"), SerializeField] private TextMeshProUGUI _text;
    [BoxGroup("Text"), SerializeField] private float _textFadeDuration = .5f;
    [BoxGroup("Text"), SerializeField] private Ease _textFadeEase = Ease.OutQuad;
    [BoxGroup("Text"), SerializeField] private float _textFadeDelay;

    [BoxGroup("Value Text"), SerializeField] private TextMeshProUGUI _valueText;

    public void Initialize()
    {
        _image.DOFade(0f, 0f);
        _rectTransformFg.sizeDelta = _initSizeDelta;
        _text.DOFade(0f, 0f);
        _starImage.DOFade(0f, 0f);
        _valueText.DOFade(0f, 0f);
    }

    public void Animate(bool showStar)
    {
        _image.DOFade(1f, _fadeDuration).SetEase(_fadeEase).SetDelay(_fadeDelay);
        _text.DOFade(1f, _textFadeDuration).SetEase(_textFadeEase).SetDelay(_textFadeDelay);
        if (showStar)_rectTransformFg.DOSizeDelta(_finalSizeDelta, _sizeDuration).SetEase(_sizeEase).SetDelay(_sizeDelay);
        if (showStar) _starImage.DOFade(1f, _starFadeDuration).SetEase(_starFadeEase).SetDelay(_starFadeDelay);
        _valueText.DOFade(1f, _starFadeDuration).SetEase(_starFadeEase).SetDelay(_starFadeDelay);
    }
}
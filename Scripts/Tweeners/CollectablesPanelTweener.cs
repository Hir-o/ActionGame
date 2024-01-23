
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[AddComponentMenu("Code/Scripts/SlideUIElementTweener")]
public class CollectablesPanelTweener : MonoBehaviour
{
    [BoxGroup("SlideDirection"), SerializeField]
    private UISlideAxis _slideDirection;

    [BoxGroup("Tweening"), SerializeField] private float _duration = 0.5f;

    [BoxGroup("Tweening"), ShowIf("_isXAxis"), SerializeField]
    private float _initXPos = 120;

    [BoxGroup("Tweening"), ShowIf("_isXAxis"), SerializeField]
    private float _finalXPos = 120f;

    [BoxGroup("Tweening"), ShowIf("_isYAxis"), SerializeField]
    private float _initYPos = 120f;

    [BoxGroup("Tweening"), ShowIf("_isYAxis"), SerializeField]
    private float _finalYPos = 120f;

    [BoxGroup("Tweening"), SerializeField] private float _delay = 1f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease;

    private RectTransform _rectTransform;
    private Tween _moveTween;
    private bool _isXAxis;
    private bool _isYAxis;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = _slideDirection switch
        {
            UISlideAxis.X => new Vector2(_initXPos, _rectTransform.anchoredPosition.y),
            UISlideAxis.Y => new Vector2(_rectTransform.anchoredPosition.x, _initYPos),
            _ => Vector2.zero
        };
    }

    private void OnValidate()
    {
        _isXAxis = _slideDirection == UISlideAxis.X;
        _isYAxis = _slideDirection == UISlideAxis.Y;
    }

    private void Start()
    {
        if (_moveTween != null && _moveTween.IsPlaying()) _moveTween.Kill();
        _moveTween = _slideDirection switch
        {
            UISlideAxis.X => _rectTransform.DOAnchorPosX(_finalXPos, _duration).SetEase(_ease).SetDelay(_delay),
            UISlideAxis.Y => _rectTransform.DOAnchorPosY(_finalYPos, _duration).SetEase(_ease).SetDelay(_delay),
            _ => null
        };
    }
}
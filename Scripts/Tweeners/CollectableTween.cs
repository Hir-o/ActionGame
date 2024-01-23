
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CollectableTween : MonoBehaviour
{
    [SerializeField] private Transform _graphicTransform;
    [SerializeField] private MeshRenderer _meshRenderer;

    [BoxGroup("Shrink"), SerializeField] private float _shrinkDuration = 0.25f;
    [BoxGroup("Shrink"), SerializeField] private float _shrinkDelay = 0.05f;
    [BoxGroup("Shrink"), SerializeField] private Ease _shrinkEase = Ease.OutCubic;

    private Tween _shrinkTween;
    private Vector3 _initScale;

    private void Awake() => _initScale = _graphicTransform.localScale;

    public void StartTweening(Action callback)
    {
        if (_shrinkTween != null) _shrinkTween.Kill();
        _shrinkTween = _graphicTransform.DOScale(Vector3.zero, _shrinkDuration)
            .SetEase(_shrinkEase)
            .SetDelay(_shrinkDelay)
            .OnComplete(() => callback?.Invoke());
    }

    public void Reset()
    {
        if (_shrinkTween != null) _shrinkTween.Kill();
        _graphicTransform.localScale = _initScale;
    }
}
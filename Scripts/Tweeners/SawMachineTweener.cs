
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SawMachineTweener : MonoBehaviour
{
    [SerializeField] private Transform _sawTransform;

    [BoxGroup("Tweening"), SerializeField] private float _rotationSpeed;

    private Tween _rotationTween;

    private void Start() => TweenRotation();

    private void OnDisable()
    {
        if (_rotationTween != null && _rotationTween.IsPlaying()) _rotationTween.Kill();
    }

    private void TweenRotation()
    {
        if (_rotationTween != null && _rotationTween.IsPlaying()) _rotationTween.Kill();
        _rotationTween = _sawTransform.DOLocalRotate(new Vector3(0f, 0f, -180f), _rotationSpeed).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetSpeedBased();
    }
}
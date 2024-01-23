
using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class FloatTweener : MonoBehaviour
{
    [SerializeField] private float _yOffset;
    [SerializeField] private float _floatDuration = 1f;
    [SerializeField] private Ease _ease = Ease.InOutQuad;
    [SerializeField] private float _maxDelay = 1f;

    private Tween _floatTween;

    private void Start() => Float();

    private void OnDisable()
    {
        if (_floatTween != null && _floatTween.IsPlaying()) _floatTween.Kill();
    }

    private void Float()
    {
        if (_floatTween != null && _floatTween.IsPlaying()) _floatTween.Kill();
        float currYPos = transform.localPosition.y;
        float delay = Random.Range(0f, _maxDelay);
        _floatTween = transform.DOLocalMoveY(_yOffset + currYPos, _floatDuration)
            .SetEase(_ease)
            .SetDelay(delay)
            .SetUpdate(UpdateType.Fixed)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
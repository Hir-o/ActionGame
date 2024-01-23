
using System;
using DG.Tweening;
using UnityEngine;

public class ThrusterTweener : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _strength = 0.1f;
    [SerializeField] private int _vibrato = 20;
    [SerializeField] private float _randomness = 60f;
    [SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

    private Tween _tween;

    private Vector3 _originalScale;

    #region Properties

    public Vector3 OriginalScale => _originalScale;

    #endregion

    private void Awake() => _originalScale = transform.localScale;

    private void Start() => StartTweening(_originalScale);

    public void StartTweening(Vector3 scale)
    {
        ResetTween();
        transform.localScale = scale;
        _tween = transform
            .DOShakeScale(_duration, _strength, _vibrato, _randomness, false, _shakeRandomnessMode)
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Restart);
    }

    public void ResetTween()
    {
        if (_tween != null && _tween.IsPlaying())
            _tween.Kill();
    }
}
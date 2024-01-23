
using System;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;

public class SpringBoardTweener : MonoBehaviour
{
    [SerializeField] private Transform _springboardTransform;

    [BoxGroup("Tweening"), SerializeField] private float _stretchDuration = .8f;
    [BoxGroup("Tweening"), SerializeField] private float _squashDuration = .3f;
    [BoxGroup("Tweening"), SerializeField] private float _squashYScale = 0.5f;
    [BoxGroup("Tweening"), SerializeField] private Ease _stretchEase = Ease.OutElastic;
    [BoxGroup("Tweening"), SerializeField] private Ease _squashEase = Ease.OutQuad;


    private JumpPad _jumpPad;
    private Tween _squashTween;

    private float _initYScale;

    private void Awake()
    {
        _jumpPad = GetComponent<JumpPad>();
        if (_springboardTransform == null) return;
        _initYScale = _springboardTransform.localScale.y;
    }

    private void OnEnable() => _jumpPad.OnTriggerJumpPad += Jumpad_OnTriggerJumpPad;

    private void OnDisable()
    {
        _jumpPad.OnTriggerJumpPad -= Jumpad_OnTriggerJumpPad;
        KillTween();   
    }

    private void Jumpad_OnTriggerJumpPad() => TweenScale();
    
    private void TweenScale()
    {
        if (_springboardTransform == null) return;
        KillTween();
        _squashTween = _springboardTransform.DOScaleY(_squashYScale, _squashDuration)
            .SetEase(_squashEase)
            .OnComplete(RevertScale);
    }

    private void RevertScale()
    {
        KillTween();
        _squashTween = _springboardTransform.DOScaleY(_initYScale, _stretchDuration)
            .SetEase(_stretchEase)
            .OnComplete(RevertScale);
    }

    private void KillTween()
    {
        if (_squashTween != null && _squashTween.IsPlaying()) _squashTween.Kill();
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneTweener : MonoBehaviour
{
    [SerializeField] private Transform _swingArm;
    [SerializeField] private float _swingArmRotationAmount;
    [SerializeField] private float _swingArmDuration;
    [SerializeField] private float _swingArmDelay;
    [SerializeField] private Ease _swingArmEase;

    private Vector3 _swingArmRotateVector;

    private Tween _swingArmTween;
    
    private void Start()
    {
        _swingArmRotateVector.y = _swingArmRotationAmount;
        SwingArm();
    }

    private void SwingArm()
    {
        if(_swingArmTween != null) _swingArmTween.Kill();
        _swingArmTween = _swingArm.DOLocalRotate(_swingArmRotateVector, _swingArmDuration, RotateMode.FastBeyond360)
            .SetEase(_swingArmEase)
            .SetDelay(_swingArmDelay)
            .SetLoops(-1, LoopType.Yoyo);
    }
}

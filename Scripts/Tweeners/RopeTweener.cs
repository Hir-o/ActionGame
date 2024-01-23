
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RopeTweener : BaseRotateTweener
{
    [SerializeField] private bool _disableRotation;
    public  override  void TweenRotation()
    {
        if (_disableRotation) return;
        _rotationAngle *= _rotationDirection;
        _origin.DOLocalRotate(new Vector3(0f, 0f, _rotationAngle), _speed)
            .SetSpeedBased()
            .SetEase(_ease)
            .OnComplete(() =>
            {
                _rotationDirection *= -1;
                TweenRotation();
            });
    }
}
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PropellerTweener : BaseRotateTweener
{
    public override void TweenRotation()
    {
        _rotationAngle *= _rotationDirection;
        _origin.DOLocalRotate(new Vector3(0f, _rotationAngle, 0f), _speed)
            .SetSpeedBased()
            .SetEase(_ease)
            .OnComplete(() =>
            {
                _rotationDirection *= -1;
                TweenRotation();
            })
            .SetLoops(-1, LoopType.Incremental);
    }
}

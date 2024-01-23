
using DG.Tweening;
using Leon;
using UnityEngine;

public class EasePresets : SceneSingleton<EasePresets>
{
    [SerializeField] private AnimationCurve _bounceOnceEase;
    [SerializeField] private AnimationCurve _bounceOnceEaseLight;
    [SerializeField] private AnimationCurve _bounceOnceEaseVeryLight;

    public AnimationCurve BounceOnceEase => _bounceOnceEase;
    public AnimationCurve BounceOnceEaseLight => _bounceOnceEaseLight;
    public AnimationCurve BounceOnceEaseVeryLight => _bounceOnceEaseVeryLight;
}

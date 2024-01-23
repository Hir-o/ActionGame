using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class ChargeParticle
{
    [SerializeField] private ParticleSystem _chargeVfx;
    [SerializeField] private Vector2 _startSizeOverLifetime = new Vector2(0f, 0.372f);
    [SerializeField] private float _maxValueTime = 1f;

    private AnimationCurve _sizeOverLifetimeCurve;
    private Tween _chargeTween;

    #region Properties

    public ParticleSystem ChargeVfx => _chargeVfx;
    public Tween ChargeTween => _chargeTween;

    #endregion

    public void TweenChargeVfx(float chargeDuration)
    {
        if (_chargeTween != null) _chargeTween.Kill();

        ParticleSystem.SizeOverLifetimeModule module = _chargeVfx.sizeOverLifetime;
        _sizeOverLifetimeCurve = new AnimationCurve();
        _sizeOverLifetimeCurve.AddKey(0f, _startSizeOverLifetime.x);
        _sizeOverLifetimeCurve.AddKey(1f, _startSizeOverLifetime.x);
        module.size = new ParticleSystem.MinMaxCurve(1f, _sizeOverLifetimeCurve);

        _chargeTween = DOVirtual.Float(_startSizeOverLifetime.x, 1f, chargeDuration, value =>
            {
                Keyframe[] modifiedKeys = _sizeOverLifetimeCurve.keys;
                modifiedKeys[modifiedKeys.Length - 1].value = value;
                if (value >= _maxValueTime) modifiedKeys[modifiedKeys.Length - 1].time = _maxValueTime;
                _sizeOverLifetimeCurve.keys = modifiedKeys;
                module.size = new ParticleSystem.MinMaxCurve(1f, _sizeOverLifetimeCurve);
            })
            .OnComplete(() => _sizeOverLifetimeCurve = null)
            .OnKill(() => _sizeOverLifetimeCurve = null);
    }

    public void StopChargeVfx()
    {
        if (_chargeTween != null) _chargeTween.Kill();
        _sizeOverLifetimeCurve = null;
    }
}
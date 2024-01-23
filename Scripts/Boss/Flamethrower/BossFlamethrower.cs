using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class BossFlamethrower : MonoBehaviour
{
    [BoxGroup("Particles"), SerializeField]
    private ParticleSystem[] _particleSystemArray;

    [BoxGroup("Charge Particles"), SerializeField]
    private ParticleSystem[] _chargeParticleSystemArray;

    [BoxGroup("Charge Particles"), SerializeField]
    private ChargeParticle _flamesCharge;

    [BoxGroup("Charge Particles"), SerializeField]
    private ChargeParticle _glowCharge;

    [BoxGroup("Charge Duration"), SerializeField]
    private float _chargeDuration = 1f;

    [BoxGroup("Collider"), SerializeField] private float _colliderActivationDelay = 0.5f;
    [BoxGroup("Collider"), SerializeField] private Collider _flamethrowerCollider;

    private Tween _colliderTween;
    private Tween _disableChargeVfxTween;

    [Button("Activate")]
    public void ActivateChargeParticles(Action callback)
    {
        for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Play();
        _flamesCharge.TweenChargeVfx(_chargeDuration);
        _glowCharge.TweenChargeVfx(_chargeDuration);

        if (_disableChargeVfxTween != null) _disableChargeVfxTween.Kill();
        _disableChargeVfxTween = DOVirtual.Float(0f, 1f, _chargeDuration, value => { }).OnComplete(() =>
        {
            for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Stop();
        }).OnComplete(() =>
        {
            callback?.Invoke();
            Activate();
        });
    }

    public void Activate()
    {
        for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Stop();
        if (_colliderTween != null) _colliderTween.Kill();
        _colliderTween =
            DOVirtual.Float(1f, 0f, _colliderActivationDelay, value => { })
                .OnComplete(() => _flamethrowerCollider.enabled = true);
        for (int i = 0; i < _particleSystemArray.Length; i++) _particleSystemArray[i].Play();
    }

    public void Deactivate()
    {
        if (_colliderTween != null) _colliderTween.Kill();
        if (_disableChargeVfxTween != null) _disableChargeVfxTween.Kill();
        _flamesCharge.StopChargeVfx();
        _glowCharge.StopChargeVfx();
        _flamethrowerCollider.enabled = false;
        for (int i = 0; i < _particleSystemArray.Length; i++) _particleSystemArray[i].Stop();
        for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Stop();
    }
}
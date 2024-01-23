using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class LadderFlamethrowerTrap : MonoBehaviour

{
    [BoxGroup("Activate Duration"), SerializeField]
    private float _activateDuration = 1f;

    [BoxGroup("Particles"), SerializeField]
    private ParticleSystem[] _particleSystemArray;

    [BoxGroup("Particles"), SerializeField]
    private float _flameDuration = 2f;

    [BoxGroup("Particles"), SerializeField]
    private float _deactivatedDuration = 1f;

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
    private Tween _disableFlameVfxTween;
    private Tween _enableFlameVfxTween;
    private Tween _activateFlameVfxTween;

    private void Start()
    {
        if (_activateFlameVfxTween != null) _activateFlameVfxTween.Kill();
        _activateFlameVfxTween = DOVirtual.Float(0f, 1f, _activateDuration, value => { }).OnComplete(Deactivate);
    }

    public void ActivateChargeParticles()
    {
        for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Play();
        _flamesCharge.TweenChargeVfx(_chargeDuration);
        _glowCharge.TweenChargeVfx(_chargeDuration);

        if (_disableChargeVfxTween != null) _disableChargeVfxTween.Kill();
        _disableChargeVfxTween = DOVirtual.Float(0f, 1f, _chargeDuration, value => { }).OnComplete(() =>
        {
            for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Stop();
        }).OnComplete(Activate);
    }

    public void Activate()
    {
        for (int i = 0; i < _chargeParticleSystemArray.Length; i++) _chargeParticleSystemArray[i].Stop();

        if (_colliderTween != null) _colliderTween.Kill();
        _colliderTween =
            DOVirtual.Float(1f, 0f, _colliderActivationDelay, value => { })
                .OnComplete(() => _flamethrowerCollider.enabled = true);
        for (int i = 0; i < _particleSystemArray.Length; i++) _particleSystemArray[i].Play();

        if (_disableFlameVfxTween != null) _disableFlameVfxTween.Kill();
        _disableFlameVfxTween = DOVirtual.Float(0f, 1f, _flameDuration, value => { }).OnComplete(Deactivate);
    }

    public void Deactivate()
    {
        if (_colliderTween != null) _colliderTween.Kill();
        _flamethrowerCollider.enabled = false;
        for (int i = 0; i < _particleSystemArray.Length; i++) _particleSystemArray[i].Stop();

        if (_enableFlameVfxTween != null) _enableFlameVfxTween.Kill();
        _enableFlameVfxTween = DOVirtual.Float(0f, 1f, _flameDuration, value => { }).OnComplete(ActivateChargeParticles);
    }

    public void OnTriggerFireThrowerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterPlayerController player))
        {
            if (player.IsDead) return;
            player.Die();
        }
    }
}
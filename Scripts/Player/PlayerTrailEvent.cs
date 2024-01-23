using System;
using UnityEngine;
using DG.Tweening;
using EmissionModule = UnityEngine.ParticleSystem.EmissionModule;

public class PlayerTrailEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _trailVfx;
    private Tween _vfxDelay;
    [SerializeField] private float _waitDelay = 1.5f;
    private EmissionModule _emissionModule;

    private void OnEnable() => MovementController.OnPlayerJumpFromJumpad += OnPlayerJumpFromJumpad;
    private void OnDisable() => MovementController.OnPlayerJumpFromJumpad -= OnPlayerJumpFromJumpad;

    private void Awake()
    {
        foreach (var trails in _trailVfx)
        {
            DisableEmission(trails);
        }
    }

    private void OnPlayerJumpFromJumpad()
    {
        foreach (var trails in _trailVfx) EnableEmission(trails);

        if (_vfxDelay != null) _vfxDelay.Kill();
        _vfxDelay = DOVirtual.Float(0f, 1f, _waitDelay, value => { })
            .OnComplete(() =>
            {
                foreach (var trails in _trailVfx)
                    DisableEmission(trails);
            });
    }

    private void EnableEmission(ParticleSystem particleSystem)
    {
        _emissionModule = particleSystem.emission;
        _emissionModule.enabled = true;
    }

    private void DisableEmission(ParticleSystem particleSystem)
    {
        _emissionModule = particleSystem.emission;
        _emissionModule.enabled = false;
    }
}
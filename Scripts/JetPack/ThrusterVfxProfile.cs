
using System;
using UnityEngine;

[Serializable]
public struct ThrusterVfxProfile
{
    [SerializeField] private float _fireParticleRateOverDistance;
    [SerializeField] private float _smokeParticleRateOverDistance;
    [SerializeField] private float _fireGlowRateOverDistance;
    [SerializeField] private FloatActionDirection _flyActionDirection;

    public FloatActionDirection? FlyActionDirection => _flyActionDirection;

    public void UpdateVfx(ParticleSystem vfxFire, ParticleSystem vfxSmoke, ParticleSystem vfxGlow)
    {
        ParticleSystem.EmissionModule _vfxFireEmissionModule = vfxFire.emission;
        _vfxFireEmissionModule.rateOverDistance = _fireParticleRateOverDistance;
        
        ParticleSystem.EmissionModule _vfxSmokeEmissionModule = vfxSmoke.emission;
        _vfxSmokeEmissionModule.rateOverDistance = _smokeParticleRateOverDistance;
        
        ParticleSystem.EmissionModule _vfxGlowEmissionModule = vfxGlow.emission;
        _vfxGlowEmissionModule.rateOverDistance = _fireGlowRateOverDistance;
    }
}
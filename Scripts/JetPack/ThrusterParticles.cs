
using System;
using UnityEngine;

[Serializable]
public struct ThrusterParticles
{
    [SerializeField] private Transform _mainParticleTransform;
    
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private ParticleSystem _smokeParticles;
    [SerializeField] private ParticleSystem _glowParticles;

    [SerializeField] private Transform _spawnTransform;

    public Transform MainParticleTransform => _mainParticleTransform;
    public ParticleSystem FireParticles => _fireParticles;
    public ParticleSystem SmokeParticles => _smokeParticles;
    public ParticleSystem GlowParticles => _glowParticles;
    public Vector3 SpawnPosition => _spawnTransform.localPosition;
}
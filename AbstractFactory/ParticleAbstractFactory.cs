using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleAbstractFactory
{
    public abstract string Name { get; }
    public abstract void SpawnParticle(Transform playerPosition, Transform parent);
}

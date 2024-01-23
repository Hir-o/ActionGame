using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingParticle : ParticleAbstractFactory
{
    public override string Name => "sliding";

    public override void SpawnParticle(Transform player, Transform parent)
    {
        PlayerParticleSpawner.Instance.TrailInstantiator(3, player, parent);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopingParticle : ParticleAbstractFactory
{
    public override string Name => "idle";

    public override void SpawnParticle(Transform player, Transform parent)
    {
        PlayerParticleSpawner.Instance.ParticleInstantiator(2, player, parent);
    }
}

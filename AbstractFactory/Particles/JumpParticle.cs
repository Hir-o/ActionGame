using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticle : ParticleAbstractFactory
{
    public override string Name => "jump";

    public override void SpawnParticle(Transform player, Transform parent)
    {
        PlayerParticleSpawner.Instance.ParticleInstantiator(0, player, parent);
    }
}

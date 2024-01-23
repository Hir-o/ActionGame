using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpParticle : ParticleAbstractFactory
{
    public override string Name => "doubleJump";

    public override void SpawnParticle(Transform player, Transform parent)
    {
        PlayerParticleSpawner.Instance.ParticleInstantiator(1, player, parent);
    }
}

using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;

public class FinishParticleFactory : GenericFactory<FinishParticleFactory>
{
    [SerializeField] private GameObject _confettiVfx;


    public GameObject GetNewFinishVfxInstance(Vector3 spawnPosition)
    {
          return GetNewInstance(_confettiVfx, spawnPosition);
    }

    public override GameObject GetNewInstance(GameObject gameObject, Vector3 spawnPosition) =>
     LeanPool.Spawn(gameObject, spawnPosition, gameObject.transform.rotation);

}

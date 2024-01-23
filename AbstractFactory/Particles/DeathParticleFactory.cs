using System;
using Lean.Pool;
using UnityEngine;

namespace Factory
{
    public class DeathParticleFactory : GenericFactory<DeathParticleFactory>
    {
        [SerializeField] private GameObject _genericDeathVfx;
        [SerializeField] private GameObject _hazardDeathVfx;

        public GameObject GetNewDeathVfxInstance(Vector3 spawnPosition) =>
            GetNewInstance(_genericDeathVfx, spawnPosition);

        public GameObject GetNewHazardDeathVfxInstance(Vector3 spawnPosition) =>
            GetNewInstance(_hazardDeathVfx, spawnPosition);

        public override GameObject GetNewInstance(GameObject gameObject, Vector3 spawnPosition) =>
            LeanPool.Spawn(gameObject, spawnPosition, gameObject.transform.rotation);
    }
}
using Lean.Pool;
using UnityEngine;

namespace Factory
{
    public class CollectableParticleFactory : GenericFactory<CollectableParticleFactory>
    {
        [SerializeField] private GameObject _collectCoinVfx;
        [SerializeField] private GameObject _collectGemVfx;

        public GameObject GetNewCoinVfxInstance(Vector3 spawnPosition) =>
            GetNewInstance(_collectCoinVfx, spawnPosition);

        public GameObject GetNewGemVfxInstance(Vector3 spawnPosition) =>
            GetNewInstance(_collectGemVfx, spawnPosition);

        public override GameObject GetNewInstance(GameObject gameobject, Vector3 spawnPosition) =>
            LeanPool.Spawn(gameobject, spawnPosition, gameobject.transform.rotation);
    }
}
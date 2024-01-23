
using Lean.Pool;
using UnityEngine;



namespace Factory {
    public class TrapParticleFactory : GenericFactory<TrapParticleFactory>
    {
        [SerializeField] private GameObject _dustTrapVfx;

        [SerializeField] private GameObject _hammerTrapVfx;

      
        public GameObject GetNewHammerTrapVfxInstance(Vector3 spawnPosition) =>
            GetNewInstance(_hammerTrapVfx, spawnPosition);


        public GameObject GetNewHammerDustVfxInstance(Vector3 spawnPosition) =>
         GetNewInstance(_dustTrapVfx, spawnPosition);

        public override GameObject GetNewInstance(GameObject gameobject, Vector3 spawnPosition) =>
            LeanPool.Spawn(gameobject, spawnPosition, gameobject.transform.rotation);
    }

}

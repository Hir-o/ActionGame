using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace  Factory
{
    public class WallJumpParticleFactory : GenericFactory<WallJumpParticleFactory>
    {
        [SerializeField] private GameObject _wallJumpVfx;
        
        public GameObject GetNewWallJumpVfxInstance(Vector3 spawnPosition)
        {
            return GetNewInstance(_wallJumpVfx, spawnPosition);
        }
        public override GameObject GetNewInstance(GameObject gameObject, Vector3 spawnPosition) =>
            LeanPool.Spawn(gameObject, spawnPosition, gameObject.transform.rotation);


    }
    
  
}

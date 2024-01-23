

using System;
using Lean.Pool;
using UnityEngine;

namespace Factory
{
    public class PlayerParticleFactory : GenericFactory<PlayerParticleFactory>
    {
        [SerializeField] private GameObject _jumpVfx;
        [SerializeField] private GameObject _doubleJumpVfx;
        [SerializeField] private GameObject _footstepVfx;
        [SerializeField] private GameObject _wallJumpVfx;

        private void OnEnable()
        {
            MovementController.OnPlayerJump += MovementController_OnPlayerJump;
            MovementController.OnPlayerDoubleJump += MovementController_OnPlayerDoubleJump;
            MovementController.OnPlayerWallJump += MovementControllerOnOnPlayerWallHang;
        }

        private void OnDisable()
        {
            MovementController.OnPlayerJump -= MovementController_OnPlayerJump;
            MovementController.OnPlayerDoubleJump -= MovementController_OnPlayerDoubleJump;
            MovementController.OnPlayerWallJump -= MovementControllerOnOnPlayerWallHang;
        }

        private void MovementController_OnPlayerJump()
        {
            if (MovementController.Instance.IsGrabbingLedge) return;
            if (MovementController.Instance.IsFacingWall) return;
            GetNewInstance(_jumpVfx, MovementController.Instance.transform.position);
        }

        private void MovementController_OnPlayerDoubleJump()
        {
            if (MovementController.Instance.IsFacingWall) return;
            GetNewInstance(_doubleJumpVfx, MovementController.Instance.transform.position);
        }

        private void MovementControllerOnOnPlayerWallHang()
        {
            GetNewInstance(_wallJumpVfx, MovementController.Instance.transform.position);
        }

        public GameObject GetNewFootstepVfxInstance(Vector3 spawnPosition) =>
            GetNewInstance(_footstepVfx, spawnPosition);

        public override GameObject GetNewInstance(GameObject gameobject, Vector3 spawnPosition) =>
            LeanPool.Spawn(gameobject, spawnPosition, gameobject.transform.rotation);
    }
}
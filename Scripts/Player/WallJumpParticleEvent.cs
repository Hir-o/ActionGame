using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Factory;


public class WallJumpParticleEvent : MonoBehaviour
{
    private void OnEnable()
    {
        MovementController.OnPlayerWallJump += MovementControllerOnOnPlayerWallHang;
    }
    private void OnDisable()
    {
        MovementController.OnPlayerWallJump -= MovementControllerOnOnPlayerWallHang;
    }

    private void MovementControllerOnOnPlayerWallHang()
    {
       
        if (WallJumpParticleFactory.Instance != null)
        {
            WallJumpParticleFactory.Instance.GetNewWallJumpVfxInstance(transform.position);
        }

    }


}

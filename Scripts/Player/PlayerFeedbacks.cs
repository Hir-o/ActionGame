using System;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    [BoxGroup("Feedbacks"), SerializeField]
    private MMFeedbacks _jumpFeedback;
    
    private bool _isSliding;

    private void OnEnable()
    {
        if (_jumpFeedback != null)
        {
            MovementController.OnPlayerJump += MovementController_OnPlayerJump;
            MovementController.OnPlayerDoubleJump += MovementController_OnPlayerDoubleJump;
            MovementController.OnPlayerJumpFromJumpad += MovementController_OnPlayerJumpFromJumpad;
            MovementController.OnPlayerWallJump += MovementController_OnPlayerWallJump;
        }
    }

    private void OnDisable()
    {
        if (_jumpFeedback != null)
        {
            MovementController.OnPlayerJump -= MovementController_OnPlayerJump;
            MovementController.OnPlayerDoubleJump -= MovementController_OnPlayerDoubleJump;
            MovementController.OnPlayerJumpFromJumpad -= MovementController_OnPlayerJumpFromJumpad;
            MovementController.OnPlayerWallJump -= MovementController_OnPlayerWallJump;
        }
    }

    private void MovementController_OnPlayerJump() => ExecuteJumpFeedback();
    private void MovementController_OnPlayerDoubleJump() => ExecuteJumpFeedback();
    private void MovementController_OnPlayerJumpFromJumpad() => ExecuteJumpFeedback();
    private void MovementController_OnPlayerWallJump() => ExecuteJumpFeedback();

    private void ExecuteJumpFeedback() => _jumpFeedback.PlayFeedbacks();
}
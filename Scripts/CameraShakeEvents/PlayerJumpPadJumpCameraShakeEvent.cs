using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpPadJumpCameraShakeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        MovementController.OnPlayerJumpFromJumpad += MovementController_OnPlayerJumpFromJumpad;
    }
    private void OnDisable()
    {
        MovementController.OnPlayerJumpFromJumpad -= MovementController_OnPlayerJumpFromJumpad;
    }
    private void MovementController_OnPlayerJumpFromJumpad()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.BouncePad);
    }
}

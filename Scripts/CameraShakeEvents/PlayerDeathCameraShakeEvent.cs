using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCameraShakeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
    }
    private void OnDisable()
    {
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
    }
    private void CharacterPlayerController_OnPlayerDied()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.Death);
    }
}

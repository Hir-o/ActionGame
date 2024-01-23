using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyCameraShakeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        EnemyJumpPad.OnPlayerBounce += EnemyJumpPad_OnPlayerBounce;
    }

    private void OnDisable()
    {
        EnemyJumpPad.OnPlayerBounce -= EnemyJumpPad_OnPlayerBounce;
    }
    private void EnemyJumpPad_OnPlayerBounce()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.EnemyDestruction);
    }
}

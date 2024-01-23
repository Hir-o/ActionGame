using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDestroyCameraShakeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        BossRocketMissleJumpad.OnPlayerRocketBounce += BossRocketMissleJumpad_OnPlayerRocketBounce;
    }
    private void OnDisable()
    {
        BossRocketMissleJumpad.OnPlayerRocketBounce -= BossRocketMissleJumpad_OnPlayerRocketBounce;
    }
    private void BossRocketMissleJumpad_OnPlayerRocketBounce()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.RocketDestruction);
    }
}

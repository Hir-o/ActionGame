using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplodeCameraShakeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        BossRocketMissile.OnRocketOnBossExplode += BossRocketMissile_OnRocketOnBossExplode;
    }
    private void OnDisable()
    {
        BossRocketMissile.OnRocketOnBossExplode += BossRocketMissile_OnRocketOnBossExplode;
    }
    private void BossRocketMissile_OnRocketOnBossExplode()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.RocketExplosion);
    }
}

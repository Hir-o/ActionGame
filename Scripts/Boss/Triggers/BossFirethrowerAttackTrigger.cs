using System;
using NaughtyAttributes;
using UnityEngine;

public class BossFirethrowerAttackTrigger : Invisibler, IUngrabbable
{
    [SerializeField] private BossFlamethrowerAttackTeleportArea _teleportArea;

    [ShowIf("_teleportArea", BossFlamethrowerAttackTeleportArea.Top), SerializeField]
    private float _topPosition;

    [ShowIf("_teleportArea", BossFlamethrowerAttackTeleportArea.Middle), SerializeField]
    private float _middlePosition;

    [ShowIf("_teleportArea", BossFlamethrowerAttackTeleportArea.Bottom), SerializeField]
    private float _bottomPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out MovementController player)) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        if (BaseBoss.Instance is IBossFlamethrowerAttack boss)
        {
            Vector3 teleportDestination = BaseBoss.Instance.transform.position;
            switch (_teleportArea)
            {
                case BossFlamethrowerAttackTeleportArea.Top:
                    teleportDestination.y = _topPosition;
                    break;
                case BossFlamethrowerAttackTeleportArea.Middle:
                    teleportDestination.y = _middlePosition;
                    break;
                case BossFlamethrowerAttackTeleportArea.Bottom:
                    teleportDestination.y = _bottomPosition;
                    break;
            }
            
            boss.TeleportAtAttackPosition(teleportDestination);
            boss.ShootFlamethrower();
        }
    }
}
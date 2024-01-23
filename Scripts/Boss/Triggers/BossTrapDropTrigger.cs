using UnityEngine;

public class BossTrapDropTrigger : Invisibler, IUngrabbable
{
    private void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out MovementController player)) return;
        if (BaseBoss.Instance is IBossDropObstacle boss)
            boss.DropObstacle();
    }
}
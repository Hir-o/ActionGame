using UnityEngine;

public class BossWarningMissilesTrigger : Invisibler, IUngrabbable
{
    private bool _isTriggered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered) return;
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out MovementController player)) return;
        if (BaseBoss.Instance is IBossWarningMissiles boss)
            boss.TriggerFiringMissiles();

        _isTriggered = true;
    }
}
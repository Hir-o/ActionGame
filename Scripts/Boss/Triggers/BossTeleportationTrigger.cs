using UnityEngine;

public class BossTeleportationTrigger : Invisibler, IUngrabbable
{
    [SerializeField] private bool _canTeleport = true;
    [SerializeField] private bool _hideBoss;

    private void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out CharacterPlayerController player)) return;
        BaseBoss.Instance.IsHiding = _hideBoss;
        BaseBoss.Instance.SetTeleportToTarget(_canTeleport);
    }
}
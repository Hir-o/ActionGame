using NaughtyAttributes;
using UnityEngine;

public class BossHorizontalRocketTrigger : Invisibler, IUngrabbable
{
    [BoxGroup("Missiles"), SerializeField] private BossRocketMissile[] _missileArray;

    protected override void Awake()
    {
        base.Awake();
        _missileArray = GetComponentsInChildren<BossRocketMissile>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out MovementController player)) return;
        for (int i = 0; i < _missileArray.Length; i++)
            _missileArray[i].Activate();
    }

    public void Reset()
    {
        for (int i = 0; i < _missileArray.Length; i++)
            _missileArray[i].Reset();
    }
}
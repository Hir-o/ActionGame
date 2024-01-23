using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class BossDashAttackTrigger : Invisibler, IUngrabbable
{
    [BoxGroup("Warning Particle"), SerializeField]
    private GameObject _warningIndicator;

    [BoxGroup("Waypoints"), SerializeField]
    private Transform[] _waypointArray;
    
    [BoxGroup("Waypoints"), SerializeField]
    private Transform[] _indicatorWaypointArray;

    [SerializeField] private float _dashDelay = 1f;

    private WaitForSeconds _waitDelay;

    protected override void Awake()
    {
        base.Awake();
        _waitDelay = new WaitForSeconds(_dashDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out CharacterPlayerController player)) return;
        if (BaseBoss.Instance is IBossDashAttack boss)
        {
            if (_waypointArray.Length == 0) return;
            TriggerDashAttack(boss);
        }
    }

    private void TriggerDashAttack(IBossDashAttack boss)
    {
        GameObject warningGameObject = LeanPool.Spawn(
            _warningIndicator,
            _indicatorWaypointArray[0].position,
            _warningIndicator.transform.rotation);
            
        if (warningGameObject.TryGetComponent(out WarningIndicator warningIndicator))
            warningIndicator.MovePath(_indicatorWaypointArray);

        StartCoroutine(boss.DashAttack(_waypointArray));
    }
}

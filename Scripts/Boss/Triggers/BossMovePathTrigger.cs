using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class BossMovePathTrigger : Invisibler, IUngrabbable
{
    [BoxGroup("Waypoints"), SerializeField]
    private Transform[] _waypointArray;

    private void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance.BossState == BossState.Dead) return;
        if (!other.TryGetComponent(out CharacterPlayerController player)) return;
        if (BaseBoss.Instance is IBossPathMovement bossPath)
        {
            if (_waypointArray.Length == 0) return;
            StartCoroutine(bossPath.MovePath(_waypointArray));
        }
    }
}
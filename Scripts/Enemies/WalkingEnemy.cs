using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WalkingEnemy : BaseEnemy, IPatrol
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if (CanPatrol)
            Coroutine = StartCoroutine(TryToTriggerTrap());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Coroutine != null) StopCoroutine(Coroutine);
        ResetPatrol();
    }

    protected override IEnumerator TryToTriggerTrap()
    {
        float distanceToPlayer;
        while (true)
        {
            yield return Wait;
            if (!MovementController.Instance.IsRunStarted) continue;
            distanceToPlayer =
                Vector3.Distance(EnemyTransform.position, MovementController.Instance.transform.position);
            if (distanceToPlayer <= BaseEnemyData.TriggerDistance)
            {
                Patrol();
                if (Coroutine != null)
                    StopCoroutine(Coroutine);
                break;
            }
        }
    }

    public void Patrol()
    {
        if (WaypointPositionArray == null) return;
        ResetPatrol();
        if (BaseEnemyData is IMovingEnemy movingEnemy)
            MoveTween = EnemyTransform.DOPath(WaypointPositionArray,
                                              movingEnemy.MovementSpeed,
                                              PathType.Linear,
                                              PathMode.Full3D,
                                              10,
                                              Color.red)
                .SetSpeedBased()
                .SetEase(movingEnemy.MovementEase)
                .SetUpdate(UpdateType.Fixed)
                .SetLoops(-1, LoopType.Yoyo);
    }

    public void ResetPatrol()
    {
        if (MoveTween != null) MoveTween.Kill();
        EnemyTransform.position = WaypointPositionArray[0];
    }

    protected override void Reset()
    {
        ResetPatrol();
        if (CanPatrol)
            Coroutine = StartCoroutine(TryToTriggerTrap());
    }

    public void OnEnemyTriggerEnter(Collider other, EnemyJumpPad enemyJumpPad)
    {
        if (other.TryGetComponent(out CharacterPlayerController player))
        {
            if (MovementController.Instance.IsSlidingOrSlowingFromSlide && enemyJumpPad != null &&
                enemyJumpPad.IsDestructable)
            {
                enemyJumpPad.DestroyEnemy();
                return;
            }

            player.Die();
        }
    }
}
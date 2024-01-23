using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DashingEnemy : BaseEnemy, IDashingBotInterface
{
    public event Action DashingBotOnIdle;
    public event Action DashingBotOnAttack;

    protected override void OnEnable()
    {
        base.OnEnable();
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
            distanceToPlayer =
                Vector3.Distance(EnemyTransform.position, MovementController.Instance.transform.position);
            if (distanceToPlayer <= BaseEnemyData.TriggerDistance)
            {
                if (BaseEnemyData is IDashingEnemy dashingEnemy)
                {
                    if (distanceToPlayer <= dashingEnemy.DashingTriggerDistance)
                    {
                        Patrol();

                        if (Coroutine != null)
                            StopCoroutine(Coroutine);
                        break;
                    }
                }
            }
        }
    }

    public void Patrol()
    {
        DashingBotOnAttack?.Invoke();
        if (MoveTween != null) MoveTween.Kill();
        if (BaseEnemyData is IDashingEnemy dashingEnemy)
            MoveTween = EnemyTransform.DOMove(MovementController.Instance.transform.position, dashingEnemy.DashingSpeed)
                .SetSpeedBased()
                .SetUpdate(UpdateType.Fixed)
                .SetEase(dashingEnemy.DashingEase)
                .SetLoops(2, LoopType.Incremental);
    }

    public void ResetPatrol()
    {
        DashingBotOnIdle?.Invoke();
        if (MoveTween != null) MoveTween.Kill();
        EnemyTransform.position = WaypointPositionArray[0];
    }

    protected override void Reset()
    {
        ResetPatrol();
        Coroutine = StartCoroutine(TryToTriggerTrap());
    }
}
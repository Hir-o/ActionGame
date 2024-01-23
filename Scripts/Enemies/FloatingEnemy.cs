using System.Collections;
using DG.Tweening;
using System;
using UnityEngine;

public class FloatingEnemy : BaseEnemy, IBirdMechPatrol
{
    public event Action OnAscend;
    public event Action OnDescend;

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
            distanceToPlayer = GetXDistanceToPlayer();

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
        {
            bool isDescending = true;
            OnDescend?.Invoke();
            MoveTween = EnemyTransform.DOPath(WaypointPositionArray, movingEnemy.MovementSpeed, PathType.Linear, PathMode.Full3D, 10, Color.red)
                .SetSpeedBased()
                .SetEase(movingEnemy.MovementEase)
                .SetUpdate(UpdateType.Fixed)
                .SetLoops(-1, LoopType.Yoyo)
                .OnStepComplete(() =>
                {
                    isDescending = !isDescending;
                    if (isDescending)
                        OnDescend?.Invoke();
                    else
                        OnAscend?.Invoke();
                });
        }
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
        EnemyTransform.gameObject.SetActive(true);
    }
}
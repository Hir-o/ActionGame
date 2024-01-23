using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RotatingEnemy : BaseEnemy, IPatrol
{
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
            MoveTween = EnemyTransform.DOPath(WaypointPositionArray, movingEnemy.MovementSpeed, PathType.CatmullRom,
                    PathMode.Full3D, 10, Color.red)
                .SetSpeedBased()
                .SetEase(movingEnemy.MovementEase)
                .SetLoops(-1)
                .SetUpdate(UpdateType.Fixed);

    }

    public void ResetPatrol()
    {
        if (MoveTween != null) MoveTween.Kill();
        EnemyTransform.position = WaypointPositionArray[0];
    }

    protected override void Reset()
    {
        ResetPatrol();
        Coroutine = StartCoroutine(TryToTriggerTrap());
    }
}
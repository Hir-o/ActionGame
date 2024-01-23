using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LadderBugEnemy : BaseEnemy, IPatrol
{
    private Tween RotateTween;

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

        if (BaseEnemyData is ILadderEnemy ladderEnemy)
        {
            MoveTween = EnemyTransform.DOPath(WaypointPositionArray, ladderEnemy.MovementSpeed, PathType.CatmullRom,
                    PathMode.Full3D, 10, Color.red)
                .SetSpeedBased()
                .SetUpdate(UpdateType.Fixed)
                .SetEase(ladderEnemy.MovementEase);

            RotateTween = EnemyTransform.DOLocalRotate(Vector3.zero, ladderEnemy.WaitDuration)
                .SetEase(ladderEnemy.RotateEase);
        }
    }

    public void ResetPatrol()
    {
        if (MoveTween != null) MoveTween.Kill();
        if (RotateTween != null) RotateTween.Kill();
        EnemyTransform.position = WaypointPositionArray[0];
        EnemyTransform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    protected override void Reset()
    {
        ResetPatrol();
        if (gameObject.activeInHierarchy) StartCoroutine(TryToTriggerTrap());
    }
}
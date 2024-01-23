
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FireballEnemy : BaseEnemy, IPatrol
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
            MoveTween = EnemyTransform.DOPath(WaypointPositionArray, movingEnemy.MovementSpeed, PathType.Linear,
                    PathMode.Full3D, 10, Color.red)
                .SetSpeedBased()
                .SetEase(movingEnemy.MovementEase)
                .OnStepComplete(() =>
                {
                    if (EnemyTransform.GetComponent<ParticleSystem>().main.gravityModifierMultiplier == 0f)
                        EnemyTransform.GetComponent<ParticleSystem>().main.gravityModifierMultiplier.Equals(-1.5f);
                    else
                        EnemyTransform.GetComponent<ParticleSystem>().main.gravityModifierMultiplier.Equals(0f);
                })
                .SetUpdate(UpdateType.Fixed)
                .SetLoops(-1, LoopType.Yoyo);
    }

    public void ResetPatrol()
    {
        if (MoveTween != null) MoveTween.Kill();
        EnemyTransform.position = WaypointPositionArray[0];
        EnemyTransform.GetComponent<ParticleSystem>().main.gravityModifierMultiplier.Equals(0f);
    }

    protected override void Reset()
    {
        ResetPatrol();
        if (CanPatrol)
            Coroutine = StartCoroutine(TryToTriggerTrap());
    }
}
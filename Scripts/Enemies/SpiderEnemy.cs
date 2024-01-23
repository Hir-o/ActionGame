using System.Collections;
using System.Linq;
using DG.Tweening;
using System;

public class SpiderEnemy : BaseEnemy, ISpiderInterface
{
    public event Action SpiderOnIdle;
    public event Action SpiderOnAttack;

    private Tween _moveBackwardTween;
    private Tween _delayTween;

    protected override void OnEnable()
    {
        base.OnEnable();
        Coroutine = StartCoroutine(TryToTriggerTrap());

        OnIsEnemyDestroyed += BaseEnemy_OnIsEnemyDestroyed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Coroutine != null) StopCoroutine(Coroutine);
        ResetPatrol();
        
        OnIsEnemyDestroyed -= BaseEnemy_OnIsEnemyDestroyed;
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
        MoveForward();
    }

    private void MoveForward()
    {
        if (BaseEnemyData is IMovingEnemy movingEnemy)
        {
            if (_delayTween != null) _delayTween.Kill();
            _delayTween = DOVirtual.DelayedCall(movingEnemy.WaitDuration, () =>
            {
                SpiderOnAttack.Invoke();
                MoveTween = EnemyTransform.DOMove(WaypointPositionArray.Last(), movingEnemy.MovementSpeed)
                    .SetSpeedBased()
                    .SetEase(movingEnemy.MovementEase)
                    .SetUpdate(UpdateType.Fixed)
                    .OnComplete(() => { MoveBackward(); });
            });
        }
    }

    private void MoveBackward()
    {
        if (BaseEnemyData is IMovingEnemy movingEnemy)
        {
            if (_delayTween != null) _delayTween.Kill();
            _delayTween = DOVirtual.DelayedCall(movingEnemy.WaitDuration, () =>
            {
                SpiderOnIdle?.Invoke();
                _moveBackwardTween = EnemyTransform.DOMove(WaypointPositionArray.First(), movingEnemy.MovementSpeed)
                    .SetSpeedBased()
                    .SetEase(movingEnemy.MovementEase)
                    .SetUpdate(UpdateType.Fixed)
                    .OnComplete(() => { MoveForward(); });
            });
        }
    }

    public void ResetPatrol()
    {
        KillTweens();
        EnemyTransform.position = WaypointPositionArray[0];
    }

    protected override void Reset()
    {
        ResetPatrol();
        Coroutine = StartCoroutine(TryToTriggerTrap());
        EnemyTransform.gameObject.SetActive(true);
    }

    private void BaseEnemy_OnIsEnemyDestroyed() => KillTweens();

    private void KillTweens()
    {
        if (_delayTween != null) _delayTween.Kill();
        if (MoveTween != null) MoveTween.Kill();
        if (_moveBackwardTween != null) _moveBackwardTween.Kill();
    }
}
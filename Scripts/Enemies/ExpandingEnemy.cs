using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ExpandingEnemy : BaseEnemy
{
    private Tween _scaleTween;
    protected override void Awake()
    {
        ResetExpand();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Coroutine = StartCoroutine(TryToTriggerTrap());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Coroutine != null) StopCoroutine(Coroutine);
        ResetExpand();
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
                Expand();
                if (Coroutine != null)
                    StopCoroutine(Coroutine);
                break;
            }
        }
    }

    public void Expand()
    {
        ResetExpand();
        if (BaseEnemyData is IExpandingEnemy expandingEnemyData)
            _scaleTween = EnemyTransform
                .DOScale(expandingEnemyData.EndScale, expandingEnemyData.ScaleSpeed)
                .SetSpeedBased()
                .SetEase(expandingEnemyData.ScaleEase);
    }

    public void ResetExpand()
    {
        if (_scaleTween != null) _scaleTween.Kill();
        if (BaseEnemyData is IExpandingEnemy expandingEnemyData)
            EnemyTransform.localScale = expandingEnemyData.StartScale * Vector3.one;
    }

    protected override void Reset()
    {
        ResetExpand();
        Coroutine = StartCoroutine(TryToTriggerTrap());
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Lean.Pool;

public class FireballTrap : BaseTrap
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private Transform _explodionPrefab;
    [SerializeField] private float _projectileWait = 2f;
    [SerializeField] private Transform _projectileEndDistance;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 4f;
    [BoxGroup("Tweening"), SerializeField] private Ease _projectileEase = Ease.OutBounce;
    [BoxGroup("Delay"), SerializeField] private float _delay = .25f;

    private Coroutine _startTrapCoroutine;

    private Tween _tweenMove;

    protected override  void Awake()
    {
        base.Awake();
        _wait = new WaitForSeconds(_checkTriggerInterval);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _coroutine = StartCoroutine(TryToTriggerTrap());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_coroutine != null) StopCoroutine(_coroutine);
        if (_tweenMove != null) _tweenMove.Kill();
    }

    protected override IEnumerator TryToTriggerTrap()
    {
        float distanceToPlayer;
        while (true)
        {
            yield return _wait;
            distanceToPlayer = Vector3.Distance(transform.position, MovementController.Instance.transform.position);
            if (distanceToPlayer <= _triggerDistance)
            {
                FireballTrapStart();

                break;
            }
        }
    }

    private void FireballTrapStart()
    {
        GameObject projectileClone = LeanPool.Spawn(_fireballPrefab, transform);

        if (_tweenMove != null) _tweenMove.Kill();
        _tweenMove = projectileClone.transform.DOLocalMoveY(_projectileEndDistance.localPosition.y, _speed)
            .SetSpeedBased()
            .SetDelay(_delay)
            .SetEase(_projectileEase)
            .OnComplete(() => {
                LeanPool.Despawn(projectileClone);
                LeanPool.Spawn(_explodionPrefab, _projectileEndDistance);
                FireballTrapStart();
            });
    }

    public void OnTriggerProjectileEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterPlayerController player))
        {
            if (player.IsDead) return;
            player.Die();
        }
    }

    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        if (_tweenMove != null) _tweenMove.Kill();
        if (_startTrapCoroutine != null) StopCoroutine(_startTrapCoroutine);
        LeanPool.DespawnAll();
    }
}

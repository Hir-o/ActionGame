using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Lean.Pool;

public class ProjectileThrowingTrap : BaseTrap
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileWait = 2f;

    [BoxGroup("Waypoints"), SerializeField] private Transform[] _waypointTranformArray;

    private Vector3[] _waypointPositionArray;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 4f;
    [BoxGroup("Tweening"), SerializeField] private Ease _projectileEase = Ease.OutBounce;
    [BoxGroup("Delay"), SerializeField] private float _delay = .25f;

    private Coroutine _startTrapCoroutine;

    private Tween _tweenMove;

    protected virtual void Awake()
    {
        base.Awake();
        if (_waypointTranformArray.Length == 0) return;
        _waypointPositionArray = new Vector3[_waypointTranformArray.Length];
        for (int i = 0; i < _waypointTranformArray.Length; i++)
            _waypointPositionArray[i] = _waypointTranformArray[i].position;

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
                if (_startTrapCoroutine == null)
                    _startTrapCoroutine = StartCoroutine(StartTrapCoroutine());

                break;
            }
        }
    }

    private IEnumerator StartTrapCoroutine()
    {
        while (true)
        {
            GameObject projectileClone = LeanPool.Spawn(_projectilePrefab, transform);

            if (_tweenMove != null) _tweenMove.Kill();
            _tweenMove = projectileClone.transform.DOPath(_waypointPositionArray, _speed, PathType.CatmullRom)
                .SetSpeedBased()
                .SetDelay(_delay)
                .SetEase(_projectileEase)
                .OnComplete(() => LeanPool.Despawn(projectileClone));

            yield return new WaitForSeconds(_projectileWait);
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

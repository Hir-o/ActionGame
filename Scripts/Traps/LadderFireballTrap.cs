using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using Lean.Pool;

public class LadderFireballTrap : BaseTrap
{
    [SerializeField] private GameObject _LadderFireballPrefab;
    [SerializeField] private Transform _explodionPrefab;
    [SerializeField] private Transform _projectileEndDistance;

    [SerializeField, Range(3, 6)] private int _projectileSequence = 3;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 4f;
    [BoxGroup("Tweening"), SerializeField] private Ease _projectileEase = Ease.OutBounce;
    [BoxGroup("Delay"), SerializeField] private float _delay = .25f;

    private Coroutine _startTrapCoroutine;

    private Tween[] _tweenMove;


    protected virtual void Awake()
    {
        base.Awake();
        _wait = new WaitForSeconds(_checkTriggerInterval);
        _tweenMove = new Tween[_projectileSequence];
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
        for (int i = 0; i < _tweenMove.Length; i++)
        {
            if (_tweenMove[i] != null) _tweenMove[i].Kill();
        }
    }

    protected override IEnumerator TryToTriggerTrap()
    {
        float distanceToPlayer;
        while (true)
        {
            yield return _wait;
            distanceToPlayer = GetXDistanceToPlayer();
            if (distanceToPlayer <= _triggerDistance)
            {
                LadderFireballTrapStart();

                break;
            }
        }
    }

    private void LadderFireballTrapStart()
    {
        for (int i = 0; i < _tweenMove.Length; i++)
        {
            GameObject projectileClone = LeanPool.Spawn(_LadderFireballPrefab, transform);

            if (_tweenMove[i] != null) _tweenMove[i].Kill();
            _tweenMove[i] = projectileClone.transform.DOLocalMoveY(_projectileEndDistance.localPosition.y, _speed)
                .SetSpeedBased()
                .SetDelay(_delay * i)
                .SetEase(_projectileEase)
                .SetLoops(-1, LoopType.Restart)
                .OnComplete(() =>
                {
                    LeanPool.Despawn(projectileClone);
                    LeanPool.Spawn(_explodionPrefab, _projectileEndDistance);
                });
        }
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
        if (_startTrapCoroutine != null) StopCoroutine(_startTrapCoroutine);

        for (int i = 0; i < _tweenMove.Length; i++)
        {
            if (_tweenMove[i] != null) _tweenMove[i].Kill();
        }

        LeanPool.DespawnAll();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class ClosingGateTrap : BaseTrap
{
    [SerializeField] private Transform _spikesTransform;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 5f;
    [BoxGroup("Tweening"), SerializeField] private Ease _popEase = Ease.OutBounce;
    [BoxGroup("Delay"), SerializeField] private float _delay = .5f;

    [SerializeField] private Vector3 _startingSpikePosition = Vector3.zero;
    [SerializeField] private Vector3 _endSpikePosition = new Vector3(0f, 1f, 0f);

    private Tween _tweenMove;

    protected override void Awake()
    {
        base.Awake();
        _spikesTransform.localPosition = _startingSpikePosition;
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
                StartTrap(_endSpikePosition);
                break;
            }
        }
    }

    private void StartTrap(Vector3 position)
    {
        if (_tweenMove != null) _tweenMove.Kill();
        _tweenMove = _spikesTransform.DOLocalMoveY(position.y, _speed)
            .SetSpeedBased()
            .SetEase(_popEase)
            .SetDelay(_delay);
    }


    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        if (_tweenMove != null) _tweenMove.Kill();
        _spikesTransform.localPosition = _startingSpikePosition;
    }
}

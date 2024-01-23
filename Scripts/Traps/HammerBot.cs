using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System;


public class HammerBot : BaseTrap
{
    public static event Action OnHammerSmash;
    public event Action OnHammerHitGround;

    [SerializeField] private Transform _armTransform;
    [SerializeField] private float _playVfx;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _smashEase = Ease.OutBounce;
    [BoxGroup("Tweening"), SerializeField] private Ease _retractEase = Ease.OutSine;

    [BoxGroup("Delay"), SerializeField] private float _delay = .5f;

    private Vector3 _startingArmRotation = new Vector3(0f, 0f, 0f);
    private Vector3 _endArmRotation = new Vector3(0f, 0f, 90f);

    private Tween _tweenRotation;

    protected virtual void Awake()
    {
        base.Awake();
        _armTransform.localEulerAngles = _startingArmRotation;
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
        if (_tweenRotation != null) _tweenRotation.Kill();
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
                StartTrap(_startingArmRotation);
                break;
            }
        }
    }

    private void StartTrap(Vector3 rotation)
    {
        if (_tweenRotation != null) _tweenRotation.Kill();
        _tweenRotation = _armTransform.DOLocalRotate(rotation, _speed)
            .SetSpeedBased()
            .SetEase(_retractEase)
            .SetDelay(_delay)
            .OnComplete(() =>
            {
                RevertTrap(_endArmRotation);
                OnHammerSmash?.Invoke();
            });
    }

    private void RevertTrap(Vector3 rotation)
    {
        if (_tweenRotation != null) _tweenRotation.Kill();
        _tweenRotation = _armTransform.DOLocalRotate(rotation, _speed)
            .OnUpdate(() =>
            {
                if (_armTransform.localEulerAngles.z >= _playVfx)
                {
                    OnHammerHitGround?.Invoke();
                    _tweenRotation.OnUpdate(null);
                }
            })
            .SetSpeedBased()
            .SetDelay(_delay)
            .OnComplete(() => { StartTrap(_startingArmRotation); })
            .SetEase(_smashEase);
    }

    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        if (_tweenRotation != null) _tweenRotation.Kill();
        _armTransform.localEulerAngles = _startingArmRotation;
    }
}
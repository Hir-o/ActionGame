using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class DroppingStoneTrap : BaseTrap
{
    [SerializeField] private Transform _armTransform;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 100f;
    [BoxGroup("Tweening"), SerializeField] private Ease _smashEase = Ease.OutBounce;
    [BoxGroup("Delay"), SerializeField] private float _delay = .5f;

    private Vector3 _startingArmRotation = new Vector3(90f, 0f, 0f);
    private Vector3 _endArmRotation = Vector3.zero;

    private Tween _tweenRotation;

    protected override  void Awake()
    {
        base.Awake();
        _armTransform.eulerAngles = _startingArmRotation;
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
                StartTrap(_endArmRotation);
                break;
            }
        }
    }

    private void StartTrap(Vector3 rotation)
    {
        if (_tweenRotation != null) _tweenRotation.Kill();
        _tweenRotation = _armTransform.DOLocalRotate(rotation, _speed)
            .SetSpeedBased()
            .SetEase(_smashEase)
            .SetDelay(_delay);
    }

    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        if (_tweenRotation != null) _tweenRotation.Kill();
        _armTransform.eulerAngles = _startingArmRotation;
    }
}
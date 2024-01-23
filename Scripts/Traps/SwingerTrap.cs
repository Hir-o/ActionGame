
using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SwingerTrap : BaseTrap
{
    public event Action OnSwing; 

    [SerializeField] private Transform _swingerTransform;
    
    [BoxGroup("Tweening"), SerializeField] private float _speed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease = Ease.OutQuad;

    [SerializeField] private float _swingAngleShrink = 20f;
    [SerializeField] private float _swingAngleExtend = 40f;
    [SerializeField] private bool _reverseSwing;

    private Vector3 _startingSwingerRotation;
    private Vector3 _endSwingerRotation;
    
    private Tween _tweenRotation;
    
    protected override void Awake()
    {
        base.Awake();
        _startingSwingerRotation = new Vector3(0f, 0f, -_swingAngleShrink);
        _endSwingerRotation = new Vector3(0f, 0f, _swingAngleExtend);
        
        if (_reverseSwing)
        {
            (_startingSwingerRotation, _endSwingerRotation) = (_endSwingerRotation, _startingSwingerRotation);
        }
        
        _swingerTransform.localEulerAngles = _startingSwingerRotation;
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
        if (_coroutine != null)
            StopCoroutine(_coroutine);
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
                StartTrap(_startingSwingerRotation);
                break;
            }
        }
    }
    
    private void StartTrap(Vector3 rotation)
    {
        OnSwing?.Invoke();
        if (_tweenRotation != null) _tweenRotation.Kill();
        _tweenRotation = _swingerTransform.DOLocalRotate(rotation, _speed)
            .SetSpeedBased()
            .SetEase(_ease)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => RevertTrap(_endSwingerRotation));
    }

    private void RevertTrap(Vector3 rotation)
    {
        OnSwing?.Invoke();
        if (_tweenRotation != null) _tweenRotation.Kill();
        _tweenRotation = _swingerTransform.DOLocalRotate(rotation, _speed)
            .SetSpeedBased()
            .SetEase(_ease)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => StartTrap(_startingSwingerRotation));
    }

    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        if (_tweenRotation != null) _tweenRotation.Kill();
        _swingerTransform.localEulerAngles = _startingSwingerRotation;
    }
}

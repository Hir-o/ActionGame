using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRockTrap : BaseTrap
{
    private Rigidbody _rigidbody;

    [SerializeField] private Transform _startPosition;

    protected virtual void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
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
                StartTrap();
                break;
            }
        }
    }

    private void StartTrap()
    {
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
    }

    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        transform.position = _startPosition.position;
    }
}

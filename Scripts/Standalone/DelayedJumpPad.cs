
using System;
using System.Collections;
using Lean.Pool;
using UnityEngine;

public class DelayedJumpPad : MonoBehaviour
{
    [Range(0, 10), SerializeField] private float _jumpVelocity;
    
    [SerializeField] private GameObject _jumpChargeVfxRed;
    [SerializeField] private GameObject _jumpChargeVfxOrange;
    [SerializeField] private GameObject _jumpBlastVfx;

    [SerializeField] private float _particleSpawnDelay = 0.5f;

    private MovementController _movementController;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private void Awake() => _wait = new WaitForSeconds(_particleSpawnDelay);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            _movementController = movementController;
            _movementController.transform.parent = transform;
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(TriggerParticles(CompleteRoutine));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            _movementController = null;
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
    }

    private IEnumerator TriggerParticles(Action completeRoutine)
    {
        LeanPool.Spawn(_jumpChargeVfxRed, transform.position, _jumpChargeVfxRed.transform.rotation);
        yield return _wait;
        LeanPool.Spawn(_jumpChargeVfxOrange, transform.position, _jumpChargeVfxOrange.transform.rotation);
        yield return _wait;
        LeanPool.Spawn(_jumpBlastVfx, transform.position, _jumpBlastVfx.transform.rotation);
        completeRoutine();
    }

    private void CompleteRoutine()
    {
        _movementController.transform.parent = null;
        _movementController.OnTriggerJumpFromOtherSources(_jumpVelocity);
        _movementController = null;
    }
}
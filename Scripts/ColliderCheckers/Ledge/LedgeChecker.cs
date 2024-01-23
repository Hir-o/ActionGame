using System;
using Leon;
using NaughtyAttributes;
using UnityEngine;

public class LedgeChecker : SceneSingleton<LedgeChecker>
{
    public static Action OnLedgeEnter;

    [BoxGroup("Head Position Transform"), SerializeField]
    private Transform _headPositionTransform;

    [BoxGroup("Head Position Transform"), SerializeField]
    private float _topRayDistance;

    [BoxGroup("Head Position Transform"), SerializeField]
    private LayerMask _groundedLayers;

    [SerializeField] private LedgeCollider _ledgeColliderTop;
    [SerializeField] private LedgeCollider _ledgeColliderBottom;

    [Header("Ledge check"), SerializeField]
    private float _ledgeCheckSkipDuration = 0.2f;

    private float _ledgeCheckSkipTimer;

    private void OnEnable() => MovementController.OnPlayerJump += ResetCheckSkipTimer;
    private void OnDisable() => MovementController.OnPlayerJump -= ResetCheckSkipTimer;

    public void CheckForLedges(float timer)
    {
        // Draws a ray in the up direction
        /*Vector3 up = transform.TransformDirection(Vector3.up) * _topRayDistance;
        Debug.DrawRay(_headPositionTransform.position, up , Color.red);*/
        
        _ledgeCheckSkipTimer -= timer;
        if (_ledgeCheckSkipTimer > 0f) return;
        if (MovementController.Instance.IsGroundedRay) return;
        if (HasABlockingColliderOnTop)
        {
            if (_ledgeColliderTop.CollidedObjects.Count > 0) _ledgeColliderTop.CollidedObjects.Clear();
            if (_ledgeColliderBottom.CollidedObjects.Count > 0) _ledgeColliderBottom.CollidedObjects.Clear();
            return;
        }

        //if (!MovementController.Instance.IsFalling) return;
        foreach (GameObject gameObject in _ledgeColliderBottom.CollidedObjects)
        {
            if (!_ledgeColliderTop.CollidedObjects.Contains(gameObject))
            {
                OnLedgeEnter?.Invoke();
                ResetCheckSkipTimer();
                break;
            }
        }
    }

    private void ResetCheckSkipTimer() => _ledgeCheckSkipTimer = _ledgeCheckSkipDuration;

    public bool HasABlockingColliderOnTop => Physics.Raycast(_headPositionTransform.position,
        Vector3.up,
        _topRayDistance,
        _groundedLayers);
}
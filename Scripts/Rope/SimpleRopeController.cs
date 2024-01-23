using System;
using DG.Tweening;
using UnityEngine;

public class SimpleRopeController : MonoBehaviour
{
    public static event Action<Transform, RopeTweener, int, Transform[]> OnAnyRopeHang;
    [SerializeField] private Transform[] _jointArray;

    private bool _isPlayerAttached;

    private RopeTweener _ropeTweener;
    private Tween _ignorePlayerCollisionTween;

    public bool IsPlayerAttached => _isPlayerAttached;

    private void Awake() => _ropeTweener = GetComponent<RopeTweener>();
    
    private void OnEnable() => MovementController.OnPlayerJump += MovementController_OnPlayerJump;
    private void OnDisable() => MovementController.OnPlayerJump -= MovementController_OnPlayerJump;

    public void HandleRopeTriggerCollision(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (_isPlayerAttached) return;
        _isPlayerAttached = true;
        int nearestJointTransformIndex = 0;
        float nearestJointDistance = float.PositiveInfinity;
        for (int i = 0; i < _jointArray.Length; i++)
        {
            float distance = Vector3.Distance(_jointArray[i].position, other.transform.position);
            if (distance < nearestJointDistance)
            {
                nearestJointDistance = distance;
                nearestJointTransformIndex = i;
            }
        }

        OnAnyRopeHang?.Invoke(transform, _ropeTweener, nearestJointTransformIndex, _jointArray);
    }

    private void MovementController_OnPlayerJump()
    {
        if (!_isPlayerAttached) return;
        MovementController.Instance.CharacterController.enabled = true;
        if (_ignorePlayerCollisionTween != null) _ignorePlayerCollisionTween.Kill();
        _ignorePlayerCollisionTween =
            DOVirtual.Float(1f, 0f, 2f, value => { })
                .OnComplete(() => _isPlayerAttached = false);
    }
}
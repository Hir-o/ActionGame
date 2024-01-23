

using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class RopeHangAction : BaseAction
{
    public static event Action<float> OnAnyGrabRope;
    public event Action OnGrabRope;
    public static event Action OnAnyReleaseRope;

    [SerializeField] private Vector3 _hangOffset;
    [SerializeField] private float _ropeDescendSpeed = 5f;
    [SerializeField] private float _degreesPerSecond = 180f;
    [SerializeField] private float _distanceThreshhold = 0.001f;
    [SerializeField] private Ease _ease = Ease.OutQuad;
    [SerializeField] private float _slidingDownMultiplier = 4f;
    [SerializeField] private float _slidingDownMultiplierDuration = 0.5f;

    [BoxGroup("Blob Shadow"), SerializeField]
    private GameObject _blobShadow;

    private float _initSpeedMultiplier = 1f;
    private float _speedMultiplier;

    private Transform _currWaypointTransform;
    private Transform _lastWaypointTransform;
    private RopeTweener _ropeTweener;
    private CharacterController _characterController;
    private Tween _collisionTween;
    private Tween _speedMultiplierTween;
    private Tween _rotationSpeedMultiplierTween;
    private Vector3 _originalCharacterScale;

    private List<Transform> _lowerWaypointsList = new List<Transform>();

    private int _totalNumberOfJoints;
    private bool _isHangingFromRope;
    private bool _ignoreHeadCollision;

    [BoxGroup("Rotation speed multiplier when the player jumps on the lower end of the swinging rope"), SerializeField]
    private bool _enableBoostRotationSpeed;

    [BoxGroup("Rotation speed multiplier when the player jumps on the lower end of the swinging rope"), SerializeField]
    private float _boostRotationSpeedDuration = .25f;

    [BoxGroup("Rotation speed multiplier when the player jumps on the lower end of the swinging rope"), SerializeField]
    private float _boostRotationSpeed = 160f;

    #region Getters and Setters

    public Transform CurrentHangedRope => _ropeTweener.transform;
    public bool IsHangingFromRope => _isHangingFromRope;

    public bool IgnoreHeadCollision
    {
        get => _ignoreHeadCollision;
        set => _ignoreHeadCollision = value;
    }

    #endregion

    private void OnEnable()
    {
        MovementController.OnPlayerJump += MovementController_OnPlayerJump;
        SimpleRopeController.OnAnyRopeHang += SimpleRopeController_OnAnyRopeHang;
        _characterController = GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        MovementController.OnPlayerJump -= MovementController_OnPlayerJump;
        SimpleRopeController.OnAnyRopeHang -= SimpleRopeController_OnAnyRopeHang;
    }

    protected override void Awake()
    {
        base.Awake();
        _originalCharacterScale = transform.localScale;
        _speedMultiplier = _initSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        if (_blobShadow != null)
        {
            if (_isHangingFromRope && _blobShadow.activeSelf)
                _blobShadow.SetActive(false);
            else if (!_isHangingFromRope && !_blobShadow.activeSelf &&
                     (transform.localRotation.x > -.1f && transform.localRotation.x < .1f))
                _blobShadow.SetActive(true);
        }

        HandleRotation();
        if (!_isHangingFromRope) return;
        UpdateAnimation();
        if (_currWaypointTransform == null && !TryGetNextWaypoint()) return;
        HandleRopeDescend();
    }

    private void UpdateAnimation() => OnAnyGrabRope?.Invoke(_ropeTweener.RotationAngle);

    private void SimpleRopeController_OnAnyRopeHang(
        Transform ropeTransform,
        RopeTweener ropeTweener,
        int nearestJointTransformIndex,
        Transform[] waypointsArray)
    {
        _characterController.enabled = false;
        _isHangingFromRope = true;
        _lowerWaypointsList = new List<Transform>();
        for (int i = 0; i < waypointsArray.Length; i++)
            if (i >= nearestJointTransformIndex)
                _lowerWaypointsList.Add(waypointsArray[i]);

        _playerMovementController.IsJumping = false;
        _playerMovementController.IsJumpPressed = false;
        _playerMovementController.IsDoubleJumping = false;
        _playerMovementController.IsDoubleJumpPressed = false;
        _playerMovementController.IsSliding = false;
        _playerMovementController.IsFalling = false;
        _ignoreHeadCollision = true;
        _ropeTweener = ropeTweener;
        transform.parent = ropeTransform;
        OnGrabRope?.Invoke();
        if (_speedMultiplierTween != null) _speedMultiplierTween.Kill();
        _speedMultiplierTween =
            DOVirtual.Float(0f, _slidingDownMultiplier, _slidingDownMultiplierDuration,
                value => { _speedMultiplier = _initSpeedMultiplier + value; }).SetEase(_ease);

        if (nearestJointTransformIndex >= 3)
        {
            _enableBoostRotationSpeed = true;
            if (_rotationSpeedMultiplierTween != null) _rotationSpeedMultiplierTween.Kill();
            _rotationSpeedMultiplierTween =
                DOVirtual.DelayedCall(_boostRotationSpeedDuration, () => _enableBoostRotationSpeed = false);
        }
    }

    private void HandleRopeDescend()
    {
        transform.parent = _currWaypointTransform;
        Vector3 waypointWithOffset = Vector3.zero + _hangOffset;
        Vector3 direction = (waypointWithOffset - transform.localPosition).normalized;
        float distance = Vector3.Distance(waypointWithOffset, transform.localPosition);
        if (transform.localPosition.y <= waypointWithOffset.y) distance = _distanceThreshhold;
        float _step = _ropeDescendSpeed * Time.fixedDeltaTime * _speedMultiplier;
        transform.localPosition += direction * _step;
        distance = Vector3.Distance(waypointWithOffset, transform.localPosition);
        if (transform.localPosition.y <= waypointWithOffset.y) distance = _distanceThreshhold;

        if (distance <= _distanceThreshhold)
        {
            if (_lowerWaypointsList.Count > 0) _currWaypointTransform = null;
            else if (_lowerWaypointsList.Count == 0)
            {
                transform.localPosition = waypointWithOffset;
                transform.parent = _currWaypointTransform.transform;
                _currWaypointTransform = null;
            }
        }
    }

    private void HandleRotation()
    {
        if (_lastWaypointTransform != null)
        {
            float rotationSpeed = _enableBoostRotationSpeed
                ? _boostRotationSpeed * Time.fixedDeltaTime
                : _degreesPerSecond * Time.fixedDeltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(_lastWaypointTransform.right);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        }
    }

    private bool TryGetNextWaypoint()
    {
        if (_lowerWaypointsList.Count > 0)
        {
            _currWaypointTransform = _lowerWaypointsList[0];
            _lowerWaypointsList.Remove(_currWaypointTransform);
            _lastWaypointTransform = _currWaypointTransform;
            return true;
        }

        return false;
    }

    private void MovementController_OnPlayerJump()
    {
        if (!_isHangingFromRope) return;
        OnAnyReleaseRope?.Invoke();
        transform.parent = null;
        if (transform.localScale != _originalCharacterScale) transform.localScale = _originalCharacterScale;
        _isHangingFromRope = false;
        _currWaypointTransform = null;
        _lastWaypointTransform = null;
        _lowerWaypointsList.Clear();
        if (_collisionTween != null) _collisionTween.Kill();
        _collisionTween = DOVirtual.Float(1f, 0f, .25f, value => { }).OnComplete(() => _ignoreHeadCollision = false);
    }
}
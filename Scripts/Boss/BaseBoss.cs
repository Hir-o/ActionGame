using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Lean.Pool;
using Leon;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseBoss : SceneSingleton<BaseBoss>, IBossPathMovement, IBossDashAttack, IBossHide
{
    public event Action OnHide;
    public event Action OnTeleport;
    public event Action OnDash;
    public event Action OnStopDash;

    [BoxGroup("Health"), SerializeField] private int _maxHealth = 100;

    [SerializeField] private float _moveSpeed = 18f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;

    [BoxGroup("Teleport Particles"), SerializeField]
    private GameObject _teleportParticle;

    [BoxGroup("Particle Wait Delay"), SerializeField]
    private float _moveDelay = 0.25f;

    [BoxGroup("Player Follow"), SerializeField]
    private float _followSpeed = 10f;

    [BoxGroup("Player Follow"), SerializeField]
    private Vector3 _followOffset;

    [BoxGroup("Player Look At"), SerializeField]
    private Transform _graphicTransform;

    [BoxGroup("Player Look At"), SerializeField]
    private Vector3 _lookAtTargetOffset = new Vector3(0f, 3f, 0f);

    [BoxGroup("Player Look At"), SerializeField]
    private float _lookAtSpeed = 2f;

    [BoxGroup("Player Look At"), SerializeField]
    private Vector3 _dashRotation = new Vector3(15f, -90f, 0f);

    [BoxGroup("Idle Movement Tweening"), SerializeField]
    private float _idleMoveSpeed = 5f;

    [BoxGroup("Idle Movement Tweening"), SerializeField]
    private Transform _waypoint1;

    [BoxGroup("Idle Movement Tweening"), SerializeField]
    private Transform _waypoint2;

    [BoxGroup("Idle Movement Tweening"), SerializeField]
    private Ease _idleEase = Ease.OutQuad;

    [BoxGroup("Idle Shake Tweening"), SerializeField]
    private Transform _bodyTransform;

    [BoxGroup("Idle Shake Tweening"), SerializeField]
    private float _idleShakeStrength = 0.05f;

    [BoxGroup("Idle Shake Tweening"), SerializeField]
    private float _idleShakeDuration = 1f;

    [BoxGroup("Idle Shake Tweening"), SerializeField]
    private int _idleShakeVibrato = 10;

    [BoxGroup("Idle Shake Tweening"), SerializeField]
    private float _idleShakeRandomness = 50f;

    [BoxGroup("Idle Shake Tweening"), SerializeField]
    private ShakeRandomnessMode _idleShakeRandomnessMode = ShakeRandomnessMode.Harmonic;

    [BoxGroup("Shake Tweening"), SerializeField]
    private float _shakeStrength = 3f;

    [BoxGroup("Shake Tweening"), SerializeField]
    private float _shakeDuration = 10f;

    [BoxGroup("Shake Tweening"), SerializeField]
    private int _shakeVibrato = 10;

    [BoxGroup("Shake Tweening"), SerializeField]
    private float _shakeRandomness = 60f;

    [BoxGroup("Shake Tweening"), SerializeField]
    private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Full;

    [BoxGroup("Dash Rotate Tween"), SerializeField]
    private float _dashRotateDuration = 1f;

    [BoxGroup("Dash Rotate Tween"), SerializeField]
    private float _dashRotateAmount = 360f;

    [BoxGroup("Dash Rotate Tween"), SerializeField]
    private AnimationCurve _dashRotateAnimationCurve;

    [BoxGroup("Scale Tween"), SerializeField]
    private Vector3 _newScale = new Vector3(1.2f, 1.2f, 1.2f);

    [BoxGroup("Scale Tween"), SerializeField]
    private float _scaleSpeed = 2f;

    [BoxGroup("Scale Tween"), SerializeField]
    private Ease _scaleEase = Ease.InOutElastic;

    private Tween _moveTween;
    private Tween _movePathTween;
    private Tween _shakeTween;
    private Tween _idleMovementTween;
    private Tween _dashRotateTween;
    private Tween _scaleTween;
    private Tween _idleShakeTween;
    private WaitForSeconds _waitDelay;
    private Transform _waypoint;
    private MeshRenderer _meshRenderer;
    private Material _material;

    private BossState _bossState;
    private Vector3 _startPosition;
    private Vector3 _startBodyPosition;
    private Vector3 _startGraphicScale;
    private Vector3 _originalFollowOffset;
    private Vector3 _originalLookAtTargetOffset;
    private Quaternion _startGraphicRotation;

    private Collider[] _colliderArray;

    private readonly string _emission = "_EMISSION";

    private bool _followPlayer = true;
    private bool _lookAtPlayer = true;
    private bool _ignoreVerticalFollowDirection = false;
    private bool _allowTeleportationToTarget = true;
    private bool _isDashing;
    private bool _isHiding;

    #region Properties

    protected int MaxHealth => _maxHealth;

    public Transform GraphicTransform
    {
        get => _graphicTransform;
        set => _graphicTransform = value;
    }

    public bool IsHiding
    {
        get => _isHiding;
        set
        {
            _isHiding = value;
            if (_isHiding) Hide();
        }
    }

    public Transform BodyTransform => _bodyTransform;

    public BossState BossState
    {
        get => _bossState;
        set => _bossState = value;
    }

    protected Vector3 FollowOffset
    {
        get => _followOffset;
        set => _followOffset = value;
    }

    protected Vector3 LookAtTargetOffset
    {
        get => _lookAtTargetOffset;
        set => _lookAtTargetOffset = value;
    }

    public float FollowSpeed
    {
        get => _followSpeed;
        set => _followSpeed = value;
    }

    public bool IsDashing => _isDashing;

    public bool LookAtPlayer
    {
        get => _lookAtPlayer;
        set => _lookAtPlayer = value;
    }

    public bool IgnoreVerticalFollowDirection
    {
        get => _ignoreVerticalFollowDirection;
        set => _ignoreVerticalFollowDirection = true;
    }

    protected bool AllowTeleportationToTarget
    {
        set => _allowTeleportationToTarget = value;
    }

    protected bool FollowPlayer
    {
        set => _followPlayer = value;
    }

    #endregion

    protected virtual void Awake()
    {
        _startPosition = transform.position;
        _startBodyPosition = _bodyTransform.localPosition;
        _startGraphicRotation = _graphicTransform.localRotation;
        _startGraphicScale = _graphicTransform.localScale;
        _originalFollowOffset = _followOffset;
        _originalLookAtTargetOffset = _lookAtTargetOffset;
        _waitDelay = new WaitForSeconds(_moveDelay);
        _colliderArray = GetComponentsInChildren<Collider>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (_meshRenderer != null) _material = _meshRenderer.material;
        _waypoint = _waypoint1;
        _bossState = BossState.Alive;
    }

    protected virtual void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        LevelManager.Instance.OnChangingScene += LevelManager_OnChangingScene;
    }

    protected virtual void OnDisable()
    {
        UpdateMaterialEmission(false);
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
    }

    private void LevelManager_OnChangingScene() => DOTween.KillAll();

    protected virtual void Start()
    {
        UpdateColliders(false);
        HandleIdleMovement();
        TeleportAtFollowPosition();
        UpdateMaterialEmission(false);
    }

    private void FixedUpdate()
    {
        if (!_followPlayer) return;
        HandlePlayerFollow();
        if (!_lookAtPlayer) return;
        HandleLookAtPlayer();
    }

    private void HandlePlayerFollow()
    {
        Vector3 targetPosition = CalculateFollowPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, FollowSpeed * Time.fixedDeltaTime);
    }

    protected Vector3 CalculateFollowPosition()
    {
        Vector3 targetPosition = CharacterPlayerController.Instance.transform.position + _followOffset;
        if (_ignoreVerticalFollowDirection) targetPosition.y = transform.position.y;
        if (transform.position.x > targetPosition.x) targetPosition.x = transform.position.x;
        return targetPosition;
    }

    private void HandleLookAtPlayer()
    {
        Vector3 targetPosition = CharacterPlayerController.Instance.transform.position + _lookAtTargetOffset;
        Vector3 direction = (_graphicTransform.transform.position - targetPosition).normalized;
        float rotationSpeed = _lookAtSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(_graphicTransform.forward, direction, rotationSpeed, 0f);
        _graphicTransform.rotation = Quaternion.LookRotation(newDirection);
    }

    protected virtual void HandleIdleMovement()
    {
        if (_idleMovementTween != null) _idleMovementTween.Kill();
        _waypoint = _waypoint == _waypoint1 ? _waypoint2 : _waypoint1;
        _idleMovementTween = _graphicTransform
            .DOLocalMove(_waypoint.transform.localPosition, _idleMoveSpeed)
            .SetSpeedBased()
            .SetEase(_idleEase)
            .OnComplete(HandleIdleMovement);
    }

    protected virtual void TeleportAtFollowPosition()
    {
        if (!_allowTeleportationToTarget) return;
        if (_moveTween != null) _moveTween.Kill();
        KillMoveTweens();
        if (_shakeTween != null) _shakeTween.Kill();
        if (_dashRotateTween != null) _dashRotateTween.Kill();
        if (_scaleTween != null) _scaleTween.Kill();
        if (_idleShakeTween != null) _idleShakeTween.Kill();
        _graphicTransform.localPosition = Vector3.zero;
        _graphicTransform.localRotation = _startGraphicRotation;
        _graphicTransform.localScale = _newScale;
        _bodyTransform.localPosition = Vector3.zero;
        _followOffset = _originalFollowOffset;
        _lookAtTargetOffset = _originalLookAtTargetOffset;
        Vector3 targetPosition = CharacterPlayerController.Instance.transform.position + _followOffset;
        if (_isHiding)
        {
            targetPosition = _startPosition;
            OnTeleport?.Invoke();
        }

        transform.position = targetPosition;
        _followPlayer = true;
        _lookAtPlayer = true;
        if (_isDashing)
        {
            OnStopDash?.Invoke();
            _isDashing = false;
        }

        _ignoreVerticalFollowDirection = false;
        SpawnTeleportParticles(transform);
        HandleIdleMovement();
        TweenScale(_startGraphicScale);
        UpdateMaterialEmission(false);
        IdleShake();
    }

    protected virtual void PlayerRespawn_OnPlayerRespawn()
    {
        _isHiding = false;
        _allowTeleportationToTarget = true;
        TeleportAtFollowPosition();
    }

    public IEnumerator MovePath(Transform[] waypointArray)
    {
        _followPlayer = false;
        Vector3[] waypointPoisitionArray = waypointArray.Select(x => x.transform.position).ToArray();
        if (_movePathTween != null) _movePathTween.Kill();
        SpawnTeleportParticles(waypointArray[0]);
        yield return _waitDelay;
        transform.position = waypointArray[0].position;
        _movePathTween = transform.DOPath(waypointPoisitionArray, _moveSpeed, PathType.CatmullRom)
            .SetSpeedBased()
            .SetUpdate(UpdateType.Fixed)
            .SetEase(_moveEase);
    }

    public IEnumerator DashAttack(Transform[] waypointArray)
    {
        _followPlayer = false;
        Vector3[] waypointPoisitionArray = waypointArray.Select(x => x.transform.position).ToArray();
        KillMoveTweens();
        SpawnTeleportParticles(transform);
        SpawnTeleportParticles(waypointArray[0]);
        yield return _waitDelay;
        transform.position = waypointArray[0].position;
        UpdateMaterialEmission(true);
        if (_idleShakeTween != null) _idleShakeTween.Kill();
        _graphicTransform.localPosition = Vector3.zero;
        _graphicTransform.localRotation = Quaternion.Euler(_dashRotation);
        _graphicTransform.localScale = _startGraphicScale;
        //DashRotate();
        Shake();
        UpdateColliders(true);
        _movePathTween = transform.DOPath(waypointPoisitionArray, _moveSpeed, PathType.CatmullRom)
            .SetSpeedBased()
            .SetEase(_moveEase)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() =>
            {
                TeleportAtFollowPosition();
                UpdateColliders(false);
                OnStopDash?.Invoke();
                _isDashing = false;
            });
        OnDash?.Invoke();
        _isDashing = true;
    }

    private void UpdateMaterialEmission(bool enableEmission)
    {
        if (_material == null) return;
        if (enableEmission)
            _material.EnableKeyword(_emission);
        else
            _material.DisableKeyword(_emission);
    }

    private void DashRotate()
    {
        if (_dashRotateTween != null) _dashRotateTween.Kill();
        Vector3 rotationVector = _graphicTransform.localRotation.eulerAngles;
        rotationVector.z = _dashRotateAmount;

        DOVirtual.Float(0, _dashRotateAmount, _dashRotateDuration, value =>
            {
                _graphicTransform.localRotation =
                    Quaternion.Euler(_startGraphicRotation.eulerAngles.x, _startGraphicRotation.eulerAngles.y, value);
            })
            .SetEase(_dashRotateAnimationCurve);
    }

    private void IdleShake()
    {
        if (_idleShakeTween != null) _idleShakeTween.Kill();
        _idleShakeTween = _bodyTransform
            .DOShakePosition(
                _idleShakeDuration,
                new Vector3(0f, _idleShakeStrength, 0f),
                _idleShakeVibrato,
                _idleShakeRandomness,
                false,
                false,
                _idleShakeRandomnessMode)
            //.OnStepComplete(() => _bodyTransform.localPosition = _startBodyPosition)
            .SetLoops(-1, LoopType.Yoyo);
    }

    protected void Shake()
    {
        if (_shakeTween != null) _shakeTween.Kill();
        _shakeTween = _graphicTransform
            .DOShakePosition(
                _shakeDuration,
                _shakeStrength,
                _shakeVibrato,
                _shakeRandomness,
                false,
                false,
                _shakeRandomnessMode)
            .OnComplete(() => { _graphicTransform.localPosition = Vector3.zero; });
    }

    protected void ShakeSmall()
    {
        if (_shakeTween != null) _shakeTween.Kill();
        _shakeTween = _graphicTransform
            .DOShakePosition(
                _shakeDuration,
                _shakeStrength / 2,
                _shakeVibrato,
                _shakeRandomness,
                false,
                false,
                _shakeRandomnessMode)
            .SetEase(Ease.OutSine)
            .OnComplete(() => { _graphicTransform.localPosition = Vector3.zero; });
    }

    private void TweenScale(Vector3 newScale)
    {
        if (_scaleTween != null) _scaleTween.Kill();
        _scaleTween = _graphicTransform
            .DOScale(newScale, _scaleSpeed)
            .SetSpeedBased().SetEase(_scaleEase);
    }

    public void SpawnTeleportParticles(Transform waypoint)
    {
        if (_teleportParticle == null) return;
        LeanPool.Spawn(_teleportParticle, waypoint.position, _teleportParticle.transform.rotation);
    }

    public void SpawnTeleportParticles(Vector3 position)
    {
        if (_teleportParticle == null) return;
        LeanPool.Spawn(_teleportParticle, position, _teleportParticle.transform.rotation);
    }

    private void UpdateColliders(bool isColliderEnabled)
    {
        for (var i = 0; i < _colliderArray.Length; i++)
            _colliderArray[i].enabled = isColliderEnabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterPlayerController player))
            player.Die();
    }

    public void SetTeleportToTarget(bool allowTeleportation)
    {
        _allowTeleportationToTarget = allowTeleportation;
        if (_allowTeleportationToTarget) TeleportAtFollowPosition();
    }

    public virtual void Hide()
    {
        _isHiding = true;
        _followPlayer = false;
        if (_movePathTween != null && DOTween.IsTweening(_movePathTween)) return;
        SpawnTeleportParticles(transform);
        transform.position = _startPosition;
        OnHide?.Invoke();
    }

    protected virtual void KillMoveTweens()
    {
        if (_movePathTween != null) _movePathTween.Kill();
        if (_idleMovementTween != null) _idleMovementTween.Kill();
    }

    protected void KillShakeTweens()
    {
        if (_idleShakeTween != null) _idleShakeTween.Kill();
        if (_shakeTween != null) _shakeTween.Kill();
    }
}
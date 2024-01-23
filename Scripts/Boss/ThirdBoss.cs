using System;
using DG.Tweening;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class ThirdBoss : BaseBoss, IBossDropObstacle, IDamageable, IBossWarningMissiles
{
    public event Action OnDie;
    public event Action<int, int> OnHealthChanged;

    [BoxGroup("Vertical Idle Movement Tweening"), SerializeField]
    private Transform _bodyHolderTransform;

    [BoxGroup("Vertical Idle Movement Tweening"), SerializeField]
    private float _verticalIdleMoveSpeed = 1.5f;

    [BoxGroup("Vertical Idle Movement Tweening"), SerializeField]
    private Transform _verticalWaypoint1;

    [BoxGroup("Vertical Idle Movement Tweening"), SerializeField]
    private Transform _verticalWaypoint2;

    [BoxGroup("Vertical Idle Movement Tweening"), SerializeField]
    private Ease _verticalIdleEase = Ease.InOutQuad;

    [BoxGroup("Obstacle Item"), SerializeField]
    private GameObject _obstacleDrop;

    [BoxGroup("Obstacle Item"), SerializeField]
    private Transform _obstacleSpawnPosition;

    [BoxGroup("Obstacle Indicator"), SerializeField]
    private GameObject _obstacleIndicator;

    [BoxGroup("Obstacle Indicator"), SerializeField]
    private Vector3 _obstacleIndicatorSpawnOffset;

    [BoxGroup("Obstacle Indicator"), SerializeField]
    private LayerMask _groundLayer;

    [BoxGroup("Obstacle Spawn Particle"), SerializeField]
    private Transform _trapSpawnTransform;

    [BoxGroup("Obstacle Spawn Particle"), SerializeField]
    private GameObject _trapSpawnVfx;

    [BoxGroup("Rocket Incoming Position"), SerializeField]
    private Transform _rocketIncomingPosition;

    [BoxGroup("Retreating"), SerializeField]
    private float _retreatDelay = 1f;

    [BoxGroup("Retreating"), SerializeField]
    private Vector3 _retreatingFollowOffset;

    [BoxGroup("Retreating"), SerializeField]
    private float _retreatingSpeed = 1f;

    private Tween _verticalIdleMovementTween;
    private Tween _changeFollowSpeedTween;
    private Tween _levelFinishTween;
    private Transform _verticalWaypoint;
    private WarningMissileStrike _warningMissileStrike;
    private Vector3 _newFollowOffset;

    private int _currHealth;
    private float _originalFollowSpeed;

    #region Properties

    public Vector3 RocketIncomingPosition => _rocketIncomingPosition.position;

    public Vector3 BodyPosition => _bodyHolderTransform.position;

    public WarningMissileStrike WarningMissileStrike => _warningMissileStrike;

    public int CurrHealth
    {
        get => _currHealth;
        set
        {
            _currHealth = value;
            OnHealthChanged?.Invoke(_currHealth, MaxHealth);
        }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _newFollowOffset = FollowOffset;
        _originalFollowSpeed = FollowSpeed;
        _currHealth = MaxHealth;
        _warningMissileStrike = GetComponent<WarningMissileStrike>();
    }

    protected override void Start()
    {
        base.Start();
        HandleIdleVerticalMovement();
    }

    protected override void TeleportAtFollowPosition()
    {
        base.TeleportAtFollowPosition();
        HandleIdleVerticalMovement();
    }

    protected override void PlayerRespawn_OnPlayerRespawn()
    {
        if (_changeFollowSpeedTween != null) _changeFollowSpeedTween.Kill();
        if (_levelFinishTween != null) _levelFinishTween.Kill();
        FollowOffset = _newFollowOffset;
        FollowSpeed = _originalFollowSpeed;
        base.PlayerRespawn_OnPlayerRespawn();
        BossState = BossState.Alive;
        CurrHealth = MaxHealth;
    }

    private void HandleIdleVerticalMovement()
    {
        if (_verticalIdleMovementTween != null) _verticalIdleMovementTween.Kill();
        _verticalWaypoint = _verticalWaypoint == _verticalWaypoint1 ? _verticalWaypoint2 : _verticalWaypoint1;
        _verticalIdleMovementTween = _bodyHolderTransform
            .DOLocalMove(_verticalWaypoint.transform.localPosition, _verticalIdleMoveSpeed)
            .SetSpeedBased()
            .SetEase(_verticalIdleEase)
            .OnComplete(HandleIdleVerticalMovement);
    }

    protected override void KillMoveTweens()
    {
        base.KillMoveTweens();
        if (_verticalIdleMovementTween != null) _verticalIdleMovementTween.Kill();
    }

    public void DropObstacle()
    {
        Vector3 bossTrapDropPosition = GetObstacleDropPosition();
        //if (_obstacleIndicator != null)
            //LeanPool.Spawn(_obstacleIndicator, bossTrapDropPosition, _obstacleIndicator.transform.rotation);
        Vector3 spawnPos = _obstacleSpawnPosition == null ? transform.position : _obstacleSpawnPosition.position;
        LeanPool.Spawn(_trapSpawnVfx, spawnPos, _trapSpawnVfx.transform.rotation);
        GameObject staticTrap =
            LeanPool.Spawn(_obstacleDrop, _trapSpawnTransform.position, _obstacleDrop.transform.rotation);
        StaticBossTrap staticBossTrap = staticTrap.GetComponent<StaticBossTrap>();
        staticBossTrap.StartMoving(bossTrapDropPosition);
    }

    private Vector3 GetObstacleDropPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, _groundLayer))
        {
            Vector3 hitPos = hit.point;
            spawnPosition = hitPos + _obstacleIndicatorSpawnOffset;
            return spawnPosition;
        }

        return spawnPosition;
    }

    public void TakeDamage(int damageAmount)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (_currHealth <= 0) return;
        CurrHealth -= damageAmount;
        if (_currHealth <= 0) Die();
    }

    public void Die()
    {
        BossState = BossState.Dead;
        OnDie?.Invoke();

        if (_changeFollowSpeedTween != null) _changeFollowSpeedTween.Kill();
        _changeFollowSpeedTween = DOVirtual.Float(1, 0f, _retreatDelay, value => { })
            .OnComplete(() =>
            {
                FollowOffset = _retreatingFollowOffset;
                FollowSpeed = _retreatingSpeed;
            });
    }

    public void TriggerFiringMissiles() => _warningMissileStrike.OnFireMissiles();
}
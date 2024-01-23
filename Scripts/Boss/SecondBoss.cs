using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class SecondBoss : BaseBoss, IDamageable, IBossWarningMissiles
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
using DG.Tweening;
using Lean.Pool;
using NaughtyAttributes;
using System;
using UnityEngine;

public class BossRocketMissile : MonoBehaviour
{
    public static event Action OnRocketOnBossExplode;

    [BoxGroup("Data"), SerializeField] private MissileData _data;

    [BoxGroup("Explosion Vfx"), SerializeField]
    private Transform _explosionSpawnTransform;

    [BoxGroup("Thruster"), SerializeField] private Transform _thrusterTransform;

    [BoxGroup("Visuals"), SerializeField] private Transform[] _visualsArray;

    [BoxGroup("Colliders"), SerializeField]
    private Collider[] _colliderArray;

    private Tweener _moveTween;
    private Tweener _launchTween;
    private Tween _rotateTween;
    private Tween _thrusterScaleTween;
    private RocketThruster _rocketThruster;
    private Transform _parent;
    private Transform _thrusterParent;

    private Vector3 _startPos;
    private Vector3 _startRotation;
    private Vector3 _startScale;
    private Vector3 _thrusterStartPos;
    private Vector3 _thrusterStartRotation;
    private Vector3 _thrusterStartScale;

    private RocketMissileState _state = RocketMissileState.Deactivated;

    private bool _ignorePlayer;
    private bool _isPreparingLaunch;
    private bool _isMovingTowardsBoss;

    private void Awake()
    {
        _startPos = transform.localPosition;
        _startRotation = transform.localRotation.eulerAngles;
        _startScale = transform.localScale;
        _thrusterStartPos = _thrusterTransform.localPosition;
        _thrusterStartRotation = _thrusterTransform.localRotation.eulerAngles;
        _thrusterStartScale = _thrusterTransform.localScale;
        _parent = transform.parent;
        _thrusterParent = _thrusterTransform.parent;

        _rocketThruster = _thrusterTransform.GetComponent<RocketThruster>();

        UpdateVisualsVisibility(false);
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;

        if (BaseBoss.Instance != null)
        {
            if (BaseBoss.Instance is BaseBoss baseBoss)
            {
                baseBoss.OnHide += BaseBoss_OnHide;
                baseBoss.OnDash += BaseBoss_OnDash;
            }

            if (BaseBoss.Instance is IDamageable damageAbleBoss)
                damageAbleBoss.OnDie += DamageableBoss_OnDie;
        }

        _ignorePlayer = false;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

        if (BaseBoss.Instance != null)
        {
            if (BaseBoss.Instance is BaseBoss baseBoss)
            {
                baseBoss.OnHide -= BaseBoss_OnHide;
                baseBoss.OnDash -= BaseBoss_OnDash;
            }

            if (BaseBoss.Instance is IDamageable damageAbleBoss)
                damageAbleBoss.OnDie -= DamageableBoss_OnDie;
        }
    }

    private void PlayerRespawn_OnPlayerRespawn() => Reset();

    private void BaseBoss_OnHide() => AutoDestructFromBossAction();

    private void BaseBoss_OnDash() => AutoDestructFromBossAction();

    private void AutoDestructFromBossAction()
    {
        if (_isMovingTowardsBoss || _isPreparingLaunch) AutoDestruct();
    }

    private void DamageableBoss_OnDie() => AutoDestruct();

    private void AutoDestruct()
    {
        if (_state == RocketMissileState.Activated)
        {
            if (_launchTween != null) _launchTween.Kill();
            if (_moveTween != null) _moveTween.Kill();
            SpawnExplosionVfx();
            Reset();
        }
    }

    public void Activate()
    {
        LeanPool.Spawn(_data.SpawnVfx, _explosionSpawnTransform.position, _data.SpawnVfx.transform.rotation);
        Reset();
        UpdateVisualsVisibility(true);
        Move();
        _state = RocketMissileState.Activated;
    }

    private void Move()
    {
        Vector3 endValue = new Vector3(transform.localPosition.x - 250f, transform.localPosition.y,
            transform.localPosition.z);
        if (_moveTween != null) _moveTween.Kill();
        _moveTween = transform.DOLocalMove(endValue, _data.MoveSpeed)
            .SetSpeedBased()
            .SetEase(_data.MoveEase)
            .SetLoops(-1, LoopType.Incremental)
            .SetUpdate(UpdateType.Fixed);
    }

    public void OnTriggerEnterRocketTip(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (other.TryGetComponent(out CharacterPlayerController player) && !_ignorePlayer)
        {
            if (MovementController.Instance.MissileDeflectAction.HasMissileInvincibility)
            {
                OnTriggerEnterRocketBody(other);
                MovementController.Instance.MissileDeflectAction.SpawnParryVfx();
                return;
            }

            KillPlayer(player);
        }
    }

    public void OnTriggerEnterRocketBody(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (other.TryGetComponent(out CharacterPlayerController player) && !_ignorePlayer)
        {
            if (BaseBoss.Instance.IsDashing || BaseBoss.Instance.IsHiding) AutoDestruct();

            if (MovementController.Instance.IsFloating)
            {
                KillPlayer(player);
                return;
            }

            transform.parent = null;
            _ignorePlayer = true;
            UpdateColliders(false);
            ReduceThruster();
            ChangeDirectionTowardsBoss();
        }
    }

    private void KillPlayer(CharacterPlayerController player)
    {
        transform.parent = null;
        SpawnExplosionVfx();
        if (SoundEffectsManager.Instance != null) SoundEffectsManager.Instance.PlayRocketExplosionSfx();
        player.Die();
        UpdateColliders(false);
    }

    private void SpawnExplosionVfx() =>
        LeanPool.Spawn(_data.ExplosionVfx, _explosionSpawnTransform.position, _data.ExplosionVfx.transform.rotation);

    private void ChangeDirectionTowardsBoss()
    {
        Vector3 movePos = Vector3.zero;
        if (_moveTween != null) _moveTween.Kill();
        if (BaseBoss.Instance is IDamageable damageableBoss)
        {
            movePos = damageableBoss.RocketIncomingPosition;
            _moveTween = transform.DOLocalMove(movePos, _data.MoveSpeedReverse)
                .SetSpeedBased()
                .SetEase(_data.MoveReverseEase)
                .SetUpdate(UpdateType.Fixed);

            _isMovingTowardsBoss = true;
            _moveTween.OnUpdate(() =>
            {
                _moveTween.ChangeEndValue(damageableBoss.RocketIncomingPosition, true);
                float distanceToWaypoint = Vector3.Distance(transform.position, damageableBoss.RocketIncomingPosition);
                if (distanceToWaypoint <= _data.RevertDistance)
                {
                    LaunchTowardsBossEnemy();
                    _moveTween.Kill();
                }
            });

            RotateTowardsBossEnemy();
        }
    }

    private void LaunchTowardsBossEnemy()
    {
        if (_isPreparingLaunch) return;
        _isPreparingLaunch = true;
        InitLaunch();
    }

    private void RotateTowardsBossEnemy()
    {
        if (_rotateTween != null) _rotateTween.Kill();
        _rotateTween = transform.DOLocalRotate(_data.FullRotation, _data.FullRotateDuration, RotateMode.FastBeyond360)
            .SetEase(_data.FullRotateEase);
    }

    private void InitLaunch()
    {
        if (_launchTween != null) _launchTween.Kill();
        ChargeThruster();
        Vector3 movePos = Vector3.zero;
        BaseBoss boss = BaseBoss.Instance;
        movePos = boss.BodyTransform.position;
        _launchTween = transform.DOLocalMove(movePos, _data.LaunchMoveSpeed)
            .SetSpeedBased()
            .SetEase(Ease.InQuad)
            .SetUpdate(UpdateType.Fixed);

        _launchTween.OnUpdate(() =>
        {
            _launchTween.ChangeEndValue(boss.BodyTransform.position, _data.LaunchMoveSpeed, true);
            float distanceToWaypoint = Vector3.Distance(transform.position, boss.BodyTransform.position);
            if (distanceToWaypoint <= _data.MinDistanceFromBoss)
            {
                ExplodeOnBoss(boss.BodyTransform.position);
                _launchTween.Kill();
            }
        });
    }

    private void ReduceThruster()
    {
        if (_thrusterScaleTween != null) _thrusterScaleTween.Kill();
        _thrusterScaleTween = _thrusterTransform.DOScale(_data.ThrusterReducedScale, _data.ThrusterChargeDuration)
            .SetEase(_data.ThrusterChargeEase);
    }

    private void ChargeThruster()
    {
        if (_thrusterScaleTween != null) _thrusterScaleTween.Kill();
        _thrusterScaleTween = _thrusterTransform.DOScale(_data.ThrusterChargingScale, _data.ThrusterChargeDuration)
            .SetEase(_data.ThrusterChargeEase).SetDelay(_data.ThrusterChargeUpDelay);
    }

    private void ExplodeOnBoss(Vector3 explosionPosition)
    {
        OnRocketOnBossExplode?.Invoke();
        _thrusterTransform.transform.parent = null;
        UpdateVisualsVisibility(false);
        LeanPool.Spawn(_data.ExplosionVfx, explosionPosition, _data.ExplosionVfx.transform.rotation);
        _rocketThruster.DisableThrusterFires();
        _state = RocketMissileState.Deactivated;
        if (SoundEffectsManager.Instance != null) SoundEffectsManager.Instance.PlayRocketExplosionSfx();
        if (BaseBoss.Instance is IDamageable damageableBoss) damageableBoss.TakeDamage(_data.Damage);
    }

    public void Reset()
    {
        _ignorePlayer = false;
        if (_moveTween != null) _moveTween.Kill();
        if (_launchTween != null) _launchTween.Kill();
        if (_rotateTween != null) _rotateTween.Kill();
        if (_thrusterScaleTween != null) _thrusterScaleTween.Kill();
        transform.parent = _parent;
        transform.localPosition = _startPos;
        transform.localRotation = Quaternion.Euler(_startRotation);
        transform.localScale = _startScale;
        _thrusterTransform.transform.parent = _thrusterParent;
        _thrusterTransform.transform.localPosition = _thrusterStartPos;
        _thrusterTransform.transform.localRotation = Quaternion.Euler(_thrusterStartRotation);
        _thrusterTransform.localScale = _thrusterStartScale;
        _isPreparingLaunch = false;
        _isMovingTowardsBoss = false;
        _state = RocketMissileState.Deactivated;
        _rocketThruster.EnableThrusterFires();
        UpdateVisualsVisibility(false);
        UpdateColliders(true);
    }

    private void UpdateVisualsVisibility(bool enable)
    {
        for (int i = 0; i < _visualsArray.Length; i++)
            _visualsArray[i].gameObject.SetActive(enable);

        if (enable)
            _rocketThruster.EnableThrusterFires();
        else
            _rocketThruster.DisableThrusterFires();
    }

    private void UpdateColliders(bool enable)
    {
        for (int i = 0; i < _colliderArray.Length; i++)
            _colliderArray[i].enabled = enable;
    }
}
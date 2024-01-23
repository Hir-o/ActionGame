
using DG.Tweening;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [BoxGroup("Data"), SerializeField] private GrenadeData _data;

    [BoxGroup("Particle Systems"), SerializeField]
    private ParticleSystem[] _particleArray;

    private Collider _collider;
    private Tweener _moveTween;

    private Vector3 _startPosition;

    private PlayerProjectileState _state = PlayerProjectileState.Idle;

    private bool _isLaunched;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _startPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;

        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance is BaseBoss baseBoss)
        {
            baseBoss.OnHide += BaseBoss_OnHide;
            baseBoss.OnDash += BaseBoss_OnDash;
        }
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance is BaseBoss baseBoss)
        {
            baseBoss.OnHide -= BaseBoss_OnHide;
            baseBoss.OnDash -= BaseBoss_OnDash;
        }
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        Reset();
        UpdateGraphics(true);
        UpdateCollider(true);
    }

    private void BaseBoss_OnHide() => AutoDestructFromBossAction();

    private void BaseBoss_OnDash() => AutoDestructFromBossAction();

    private void AutoDestructFromBossAction()
    {
        if (_isLaunched) AutoDestruct();
    }

    private void AutoDestruct()
    {
        if (_state == PlayerProjectileState.Launched)
        {
            if (_moveTween != null) _moveTween.Kill();
            SpawnExplosionVfx();
            Reset();
        }
    }

    private void Launch()
    {
        Vector3 movePos = Vector3.zero;
        if (BaseBoss.Instance is IDamageable damageableBoss)
        {
            _state = PlayerProjectileState.Launched;
            BaseBoss boss = BaseBoss.Instance;
            movePos = boss.BodyTransform.position;
            if (_moveTween != null) _moveTween.Kill();
            _moveTween = transform.DOMove(movePos, _data.MoveSpeed)
                .SetSpeedBased()
                .SetEase(_data.MoveEase)
                .SetUpdate(UpdateType.Fixed)
                .OnUpdate(() =>
                {
                    _moveTween.ChangeEndValue(boss.BodyTransform.position, _data.MoveSpeed, true);
                    float distanceFromBoss = Vector3.Distance(transform.position, boss.BodyTransform.position);

                    if (distanceFromBoss <= _data.MinDistanceFromBoss)
                    {
                        HitBoss(boss.BodyTransform, ref damageableBoss);
                        _moveTween.Kill();
                    }
                });

            _isLaunched = true;
        }
    }

    private void HitBoss(Transform explosionTransform, ref IDamageable damageableBoss)
    {
        UpdateGraphics(false);
        UpdateCollider(false);
        SpawnExplosionVfx(explosionTransform);
        damageableBoss.TakeDamage(_data.Damage);
        _state = PlayerProjectileState.Idle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (BaseBoss.Instance is FourthBoss boss)
        {
            if (boss.BossState == BossState.Dead || boss.IsHiding)
            {
                AutoDestruct();
                return;
            }
        }

        if (other.TryGetComponent(out CharacterPlayerController player))
        {
            if (_state == PlayerProjectileState.Idle)
                Launch();
        }
    }

    private void SpawnExplosionVfx(Transform explosionTransform = null)
    {
        Transform explosionTransformTemp = explosionTransform == null ? transform : explosionTransform;
        LeanPool.Spawn(_data.ExplosionVfx, explosionTransformTemp.position, transform.localRotation);
    }

    private void Reset()
    {
        if (_moveTween != null) _moveTween.Kill();
        transform.localPosition = _startPosition;
        _isLaunched = false;
        _state = PlayerProjectileState.Idle;
    }

    private void UpdateGraphics(bool enable)
    {
        for (int i = 0; i < _particleArray.Length; i++)
        {
            if (enable)
                _particleArray[i].Play();
            else
                _particleArray[i].Stop();
        }
    }

    private void UpdateCollider(bool enable) => _collider.enabled = enable;
}
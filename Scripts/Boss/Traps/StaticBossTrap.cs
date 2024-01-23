using System;
using Leon.Extensions;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class StaticBossTrap : MonoBehaviour
{
    [BoxGroup("Boss Trap"), SerializeField]
    private float _speed = 5f;

    [BoxGroup("Boss Trap"), SerializeField]
    private Ease _ease = Ease.Linear;

    [BoxGroup("Ground Layers"), SerializeField]
    private LayerMask _groundMask;

    [BoxGroup("Animation"), SerializeField]
    private float _scaleSpeed = 5f;

    [BoxGroup("Animation"), SerializeField]
    private Ease _scaleEase = Ease.InOutElastic;

    [BoxGroup("Scale Tween"), SerializeField]
    private Vector3 _newScale = new Vector3(1.4f, 1.4f, 1.4f);

    private Tween _scaleTween;
    private Tween _moveTween;
    private Vector3 _startScale;

    private ObjectLeanDespawner _objectLeanDespawner;

    private void Awake()
    {
        _startScale = transform.localScale;
        _objectLeanDespawner = GetComponent<ObjectLeanDespawner>();
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable secondBoss)
            secondBoss.OnDie += SecondBoss_OnDie;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        if (BaseBoss.Instance != null && BaseBoss.Instance is IDamageable secondBoss)
            secondBoss.OnDie -= SecondBoss_OnDie;
        KillTweens();
        transform.localScale = _startScale;
    }

    private void PlayerRespawn_OnPlayerRespawn() => DespawnInstantly();
    private void SecondBoss_OnDie() => DespawnInstantly();

    private void DespawnInstantly()
    {
        StopMoving();
        _objectLeanDespawner.InstantDespawn();
    }

    public void StartMoving(Vector3 targetPosition)
    {
        TweenScale();
        StopMoving();
        _moveTween = transform.DOMove(targetPosition, _speed)
            .SetSpeedBased()
            .SetEase(_ease);
    }

    private void TweenScale()
    {
        if (_scaleTween != null) _scaleTween.Kill();
        _scaleTween = transform
            .DOScale(_newScale, _scaleSpeed)
            .SetSpeedBased()
            .SetEase(_scaleEase);
    }

    public void StopMoving()
    {
        if (_moveTween != null) _moveTween.Kill();
    }

    public void OnTrapTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BossFlamethrower bossFlamethrower)) return;
        if (other.TryGetComponent(out BossFirethrowerAttackTrigger bossFirethrowerAttackTrigger))
            return;

        if (_groundMask.IsInLayerMask(other.gameObject))
        {
            StopMoving();
            return;
        }

        if (other.TryGetComponent(out CharacterPlayerController player))
            player.Die();
    }

    private void KillTweens()
    {
        if (_moveTween != null) _moveTween.Kill();
        if (_scaleTween != null) _scaleTween.Kill();
    }
}
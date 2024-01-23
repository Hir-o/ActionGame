using System;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class EnemyJumpPad : BaseJumpad
{
    public static event Action OnPlayerBounce;
    public static event Action OnAnyEnemyDestroyed;

    [BoxGroup("Enemy Collider"), SerializeField]
    private Collider _enemyCollider;

    [SerializeField] private bool _isDestructable;
    [SerializeField] private Transform _explodeVfx;
    [SerializeField] private Collider _collider;
    [SerializeField] private float disableEnemyDelay;

    private BaseEnemy _baseEnemy;

    #region Properties

    public bool IsDestructable => _isDestructable;

    #endregion

    private void Awake()
    {
        _baseEnemy = GetComponentInParent<BaseEnemy>();
        _collider = GetComponent<Collider>();
        if (!IsDestructable)
            Assert.IsNotNull(_enemyCollider,
                $"Enemy collider is null in {_enemyCollider.gameObject.name}. Make sure that you assing the right enemy collider in the EnemyJumpad.cs script.");
    }

    private void Start()
    {
        if (!IsDestructable)
            _collider.gameObject.SetActive(false);
    }

    private void OnEnable() => PlayerRespawn_OnPlayerRespawn();

    private void PlayerRespawn_OnPlayerRespawn()
    {
        if (_baseEnemy != null) _baseEnemy.IsDestroyed = false;
        _collider.enabled = true;
        if (_enemyCollider == null) return;
        _enemyCollider.enabled = true;
    }

    public void OnJumpadTriggerEnter(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (_baseEnemy != null && _baseEnemy.IsDestroyed) return;
        if (other.transform.root.gameObject.GetInstanceID() ==
            CharacterPlayerController.Instance.gameObject.GetInstanceID())
        {
            OnPlayerBounce?.Invoke();
            MovementController.Instance.OnTriggerJumpFromOtherSources(_jumpForce, _horizontalForce, false, false);
            if (_invertCharacterDirection) MovementController.Instance.SwitchMoveDirection();
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        if (_baseEnemy != null) _baseEnemy.IsDestroyed = true;
        OnAnyEnemyDestroyed?.Invoke();
        if (SoundEffectsManager.Instance != null) SoundEffectsManager.Instance.PlayEnemyDestructSfx();
        DOVirtual.Float(disableEnemyDelay, 0f, disableEnemyDelay, value => { }).OnComplete(DisableEnemy);
    }

    private void DisableEnemy()
    {
        LeanPool.Spawn(_explodeVfx, transform.position, _explodeVfx.rotation);
        _collider.enabled = false;
        if (_baseEnemy != null && _baseEnemy.EnemyTransform != null)
            _baseEnemy.EnemyTransform.gameObject.SetActive(false);
        if (_enemyCollider == null) return;
        _enemyCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        return;
    }
}
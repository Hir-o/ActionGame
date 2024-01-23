using System;
using CharacterMovement;
using Leon;
using UnityEngine;

public class CharacterPlayerController : SceneSingleton<CharacterPlayerController>
{
    public static event Action OnPlayerDied;
    public static event Action OnPlayerDiedCamera;
    public static event Action<DeathType> OnPlayerDiedVfx;

    [SerializeField] private bool _isDead;

    [SerializeField] private LayerMask _hazardLayer;
    protected float _defaultColliderRadius;
    protected float _defaultColliderHeight;
    protected Vector3 _defaultColliderCenter;

    private CharacterController _characterController;
    private CollectableMagnet _collectableMagnet;

    #region Properties

    public CharacterController CharacterController
    {
        get => _characterController;
        set => _characterController = value;
    }

    public bool IsDead
    {
        get => _isDead;
        private set
        {
            _isDead = value;
            if (_isDead) OnPlayerDied?.Invoke();
        }
    }

    public CollectableMagnet CollectableMagnet => _collectableMagnet;

    #endregion

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += ResetState;
        FloatAction.OnAnyUpdateFloatCollider += FloatAction_OnAnyUpdateFloatCollider;
        FloatAction.OnAnyResetCollider += FloatAction_OnAnyResetCollider;
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= ResetState;
        FloatAction.OnAnyUpdateFloatCollider -= FloatAction_OnAnyUpdateFloatCollider;
        FloatAction.OnAnyResetCollider -= FloatAction_OnAnyResetCollider;
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
    }

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _collectableMagnet = GetComponentInChildren<CollectableMagnet>();
        _defaultColliderRadius = _characterController.radius;
        _defaultColliderHeight = _characterController.height;
        _defaultColliderCenter = _characterController.center;
    }

    private void FixedUpdate()
    {
        MovementController.Instance.UpdateMovement();
        LedgeChecker.Instance.CheckForLedges(Time.fixedDeltaTime);
    }

    private void FloatAction_OnAnyUpdateFloatCollider(float newRadius, float newHeight, Vector3 newCenter)
    {
        _characterController.radius = newRadius;
        _characterController.height = newHeight;
        _characterController.center = newCenter;
    }

    private void FloatAction_OnAnyResetCollider()
    {
        _characterController.radius = _defaultColliderRadius;
        _characterController.height = _defaultColliderHeight;
        _characterController.center = _defaultColliderCenter;
    }

    private void LevelFinish_OnAnyLevelCompleted()
    {
        //todo implement if necessary
    }

    public void Die()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.IsLevelFinished) return;
        if (IsDead) return;
        
        MovementController.Instance.ResetMovement();
        
        IsDead = true;
        
        OnPlayerDiedCamera?.Invoke();

        MovementController.Instance.ResetRotation();
        if (MovementController.Instance.ClimbAction.IsClimbing)
        {
            MovementController.Instance.ClimbAction.StopClimb();
            MovementController.Instance.SetMoveDirection(MoveDirection.Right);
        }
    }

    private void ResetState() => _isDead = false;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var hitLayer = hit.gameObject.layer;

        if (hit.gameObject.TryGetComponent(out IColliderDisable colliderDisable))
        {
            colliderDisable.DisableCollider();
            OnPlayerDiedVfx?.Invoke(DeathType.HazardFogDeath);
        }
        else
        {
            if (IsLayerCollision(hitLayer, _hazardLayer))
                OnPlayerDiedVfx?.Invoke(DeathType.NormalDeath);
        }

        if (IsLayerCollision(hitLayer, _hazardLayer)) Die();
    }

    private bool IsLayerCollision(int layer, LayerMask layerMask) => (1 << layer & layerMask.value) != 0;
}
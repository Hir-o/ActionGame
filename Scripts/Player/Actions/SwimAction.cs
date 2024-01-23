

using System;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimAction : FloatAction
{
    public static event Action OnPlayerSwim;
    public static event Action OnPlayerSwimCancel;
    public static event Action<FloatActionDirection?>  OnAnySwimAscending;
    public static event Action<FloatActionDirection?>  OnAnyStopSwimAscending;
    public static event Action<FloatActionDirection?>  OnAnySwimDescending;
    public static event Action<FloatActionDirection?>  OnAnyStopSwimDescending;

    [BoxGroup("Ground Check"), SerializeField]
    private Transform _groundRayPosition;

    [BoxGroup("Ground Check"), SerializeField]
    private float _groundRayLength = .5f;

    [BoxGroup("Ground Check"), SerializeField]
    private float _swimUpForce = 0.5f;

    [BoxGroup("Ground Check"), SerializeField]
    private LayerMask _groundedLayerMask;

    [BoxGroup("Particles"), SerializeField]
    private GameObject _bubblesVfx;

    private GameObject _spawnedBubblesVfx;
    
    #region Properties

    public float SwimUpForce => _swimUpForce;
    
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        WaterEntry.OnDiveIntoWater += WaterEntry_OnDiveIntoWater;
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        WaterEntry.OnDiveIntoWater -= WaterEntry_OnDiveIntoWater;
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
    }

    private void WaterEntry_OnDiveIntoWater() => Float();
    private void CharacterPlayerController_OnPlayerDied() => EndFloating();
    private void PlayerRespawn_OnPlayerRespawn() => EndFloating();

    protected override void Float()
    {
        if (IsFloating) return;
        base.Float();
        OnPlayerSwim?.Invoke();
        _spawnedBubblesVfx = LeanPool.Spawn(_bubblesVfx, transform.position, Quaternion.identity, transform);
    }

    public override void EndFloating()
    {
        base.EndFloating();
        OnPlayerSwimCancel?.Invoke();
        if (_spawnedBubblesVfx != null) LeanPool.Despawn(_spawnedBubblesVfx);
    }

    protected override void OnStopAscending(InputAction.CallbackContext context)
    {
        base.OnStopAscending(context);
        OnAnyStopSwimAscending?.Invoke(_floatActionDirection);
    }

    protected override void OnStopDescending(InputAction.CallbackContext context)
    {
        base.OnStopDescending(context);
        OnAnyStopSwimDescending?.Invoke(_floatActionDirection);
    }

    protected override void Ascend()
    {
        base.Ascend();
        OnAnySwimAscending?.Invoke(_floatActionDirection);
    }
    
    protected override void Descend()
    {
        base.Descend();
        OnAnySwimDescending?.Invoke(_floatActionDirection);
    }

    public bool IsGroundedRayUnderwater =>
        Physics.Raycast(_groundRayPosition.position, Vector3.down, _groundRayLength, _groundedLayerMask);
}
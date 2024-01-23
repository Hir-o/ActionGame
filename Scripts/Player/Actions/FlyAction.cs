

using System;
using UnityEngine.InputSystem;

public class FlyAction : FloatAction
{
    public static event Action OnPlayerFly;
    public static event Action OnPlayerFlyCancel;
    public static event Action<bool> OnEquippFlyingEquipment;

    public static event Action<FloatActionDirection?> OnAnyFlyAscending;
    public static event Action<FloatActionDirection?> OnAnyFlyDescending;
    public static event Action<FloatActionDirection?> OnAnyStopFlyAscending;
    public static event Action<FloatActionDirection?> OnAnyStopFlyDescending;

    protected override void OnEnable()
    {
        base.OnEnable();
        FlyingEquipmentCollectable.OnAnyCollectFlyingEquipment +=
            FlyingEquipmentCollectable_OnAnyCollectFlyingEquipment;
        Grounder.OnGroundFlyingCharacter += Grounder_OnAnyGroundFlyingCharacter;
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FlyingEquipmentCollectable.OnAnyCollectFlyingEquipment -=
            FlyingEquipmentCollectable_OnAnyCollectFlyingEquipment;
        Grounder.OnGroundFlyingCharacter -= Grounder_OnAnyGroundFlyingCharacter;
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
    }

    private void FlyingEquipmentCollectable_OnAnyCollectFlyingEquipment() => Float();

    private void Grounder_OnAnyGroundFlyingCharacter() => EndFloating();
    private void CharacterPlayerController_OnPlayerDied() => EndFloating();

    protected override void Float()
    {
        base.Float();
        OnPlayerFly?.Invoke();
        OnEquippFlyingEquipment?.Invoke(IsFloating);
    }

    public override void EndFloating()
    {
        base.EndFloating();
        OnPlayerFlyCancel?.Invoke();
        OnEquippFlyingEquipment?.Invoke(IsFloating);
    }

    protected override void OnStopAscending(InputAction.CallbackContext context)
    {
        base.OnStopAscending(context);
        OnAnyStopFlyAscending?.Invoke(_floatActionDirection);
    }

    protected override void OnStopDescending(InputAction.CallbackContext context)
    {
        base.OnStopDescending(context);
        OnAnyStopFlyDescending?.Invoke(_floatActionDirection);
    }

    protected override void Ascend()
    {
        base.Ascend();
        OnAnyFlyAscending?.Invoke(_floatActionDirection);
    }

    protected override void Descend()
    {
        base.Descend();
        OnAnyFlyDescending?.Invoke(_floatActionDirection);
    }
}
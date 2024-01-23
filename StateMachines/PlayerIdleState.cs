using System;

public class PlayerIdleState : PlayerBaseState
{
    public static Action<bool> OnPlayerIdle;

    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        : base(context, playerStateFactory)
    {
    }

    public override void EnterState()
    {
        OnPlayerIdle?.Invoke(Context.IsMovementPressed);
        Context.AppliedMovementX = 0f;
        Context.AppliedMovementZ = 0f;
    } 

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsMovementPressed) SwitchState(PlayerStateFactory.Run());
    }

    public override void InitializeSubState()
    {
    }
}
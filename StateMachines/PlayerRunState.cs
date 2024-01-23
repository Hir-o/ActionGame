using System;

public class PlayerRunState : PlayerBaseState
{
    public static Action<bool> OnPlayerRun;
    
    public PlayerRunState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        : base(context, playerStateFactory)
    {
    }

    public override void EnterState() => OnPlayerRun?.Invoke(Context.IsMovementPressed);

    public override void UpdateState()
    {
        CheckSwitchStates();
        Context.AppliedMovementX = Context.CurrentMovementInput.x;
        Context.AppliedMovementZ = Context.CurrentMovementInput.y;
    } 

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsMovementPressed) SwitchState(PlayerStateFactory.Idle());
    }

    public override void InitializeSubState()
    {
    }
}
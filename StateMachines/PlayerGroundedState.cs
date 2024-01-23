using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        : base(context, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Context.CurrMovementY = Context.GroundedGravity;
        Context.AppliedMovementY = Context.GroundedGravity;
    }

    public override void UpdateState() => CheckSwitchStates();

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsJumpPressed && !Context.RequireNewJumpPress)
            SwitchState(PlayerStateFactory.Jump());
    }

    public override void InitializeSubState()
    {
        if (Context.IsMovementPressed)
            SetSubState(PlayerStateFactory.Run());
        else
            SetSubState(PlayerStateFactory.Idle());
    }
}
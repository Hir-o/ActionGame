using System;
using DG.Tweening;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public static Action OnPlayerJump;
    public static Action<bool> OnPlayerJumpCancel;

    private float _groundCheckDelay = 0.25f;
    private float _currGroundCheckDelay;

    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        : base(context, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _currGroundCheckDelay = _groundCheckDelay;
        DOVirtual.Float(_currGroundCheckDelay, 0f, _currGroundCheckDelay, value => { _currGroundCheckDelay = value; });

        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        if (Context.IsJumpPressed) Context.RequireNewJumpPress = true;
        OnPlayerJumpCancel?.Invoke(Context.IsMovementPressed);
    }

    public override void CheckSwitchStates()
    {
        if (_currGroundCheckDelay > 0f) return;
        if (Context.IsGrounded)
        {
            Context.IsJumping = false;
            SwitchState(PlayerStateFactory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    private void HandleJump()
    {
        Context.IsJumping = true;
        OnPlayerJump?.Invoke();
        Context.CurrMovementY = Context.InitialJumpVelocity;
        Context.AppliedMovementY = Context.InitialJumpVelocity;
    }

    private void HandleGravity()
    {
        // Frame independent jump. Velocity verlet integration for fall calculation after jumping
        // Resource: https://www.youtube.com/watch?v=h2r3_KjChf4 at 13:40
        bool isFalling = Context.CurrMovementY <= 0f || !Context.IsJumpPressed;
        float fallMultiplier = 2f;
        if (isFalling)
        {
            float previousYVelocity = Context.CurrMovementY;
            Context.CurrMovementY = Context.CurrMovementY + (Context.Gravity * Time.deltaTime * fallMultiplier);
            Context.AppliedMovementY =
                Mathf.Max((previousYVelocity + Context.CurrMovementY) * 0.5f, Context.FallClampSpeed);
        }
        else
        {
            float previousYVelocity = Context.CurrMovementY;
            Context.CurrMovementY = Context.CurrMovementY + (Context.Gravity * Time.deltaTime);
            Context.AppliedMovementY = (previousYVelocity + Context.CurrMovementY) * 0.5f;
        }
    }
}
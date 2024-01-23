
using System;
using CharacterMovement;
using UnityEngine;

public class ClimbAction : BaseAction
{
    public static event Action<float> OnPlayerClimb;
    public static event Action OnPlayerClimbCancel;
    public static event Action OnLeaveFinalLadder;

    [SerializeField] private float _climbSpeed = 5f;

    private bool _isClimbing;
    private bool _canJumpFromLadder;
    private MoveDirection _jumpDirection;
    private Ladder _currLader;

    public float ClimbSpeed => _climbSpeed;

    public bool IsClimbing
    {
        get => _isClimbing;
        set => _isClimbing = value;
    }

    public bool CanJumpFromLadder
    {
        get => _canJumpFromLadder;
        set => _canJumpFromLadder = value;
    }

    public Ladder CurrLadder => _currLader;

    public void Climb(MoveDirection jumpDirection, Ladder ladder)
    {
        if (_isClimbing) return;
        _isClimbing = true;
        _currLader = ladder;
        _jumpDirection = jumpDirection;
        CanJumpFromLadder = true;
        _playerMovementController.IsSliding = false;
        _playerMovementController.IsJumping = false;
        _playerMovementController.IsJumpPressed = false;
        _playerMovementController.IsDoubleJumping = true;
        _playerMovementController.IsDoubleJumpPressed = true;
        OnPlayerClimb?.Invoke(_playerMovementController.Direction);
    }

    public void StopClimb(bool jump = false)
    {
        if (!_isClimbing) return;
        _isClimbing = false;
        OnPlayerClimbCancel?.Invoke();
        _playerMovementController.ApplyMovementSpeed();
        if (jump) _playerMovementController.AutoJump(_playerMovementController.InitialJumpVelocity);
        _playerMovementController.SetMoveDirection(_jumpDirection);
        if (_currLader.IsFinalLadderPart) OnLeaveFinalLadder?.Invoke();
        _currLader = null;
        CanJumpFromLadder = false;
    }
}
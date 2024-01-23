
using System;
using UnityEngine;

public class JumpPad : BaseJumpad
{
    public event Action OnTriggerJumpPad;

    [SerializeField] private bool _slowDownPlayerMovement;
    [SerializeField] private bool _disableDoubleJump;
    [SerializeField] private bool _applyAsDownforce;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            float force = _applyAsDownforce ? -_jumpForce : _jumpForce;
            movementController.OnTriggerJumpFromOtherSources(force, _horizontalForce, _slowDownPlayerMovement);
            OnTriggerJumpPad?.Invoke();
            
            if (SoundEffectsManager.Instance != null) SoundEffectsManager.Instance.PlayJumpPadSfx();

            if (_invertCharacterDirection) movementController.SwitchMoveDirection();
            if (_disableDoubleJump)
            {
                movementController.IsDoubleJumping = true;
                movementController.IsDoubleJumpPressed = true;
            }
        }
    }
}
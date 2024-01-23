
using System;
using UnityEngine;

public class Grounder : Invisibler, IUngrabbable
{
    public static event Action OnGroundFlyingCharacter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            OnGroundFlyingCharacter?.Invoke();
            movementController.IsDoubleJumping = true;
            movementController.IsDoubleJumpPressed = true;
        }
    }
}

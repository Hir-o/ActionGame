

using UnityEngine;

public class WaterExitJumpad : BaseJumpad
{
    private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _meshRenderer.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            if (movementController.IsJumping) return;
            movementController.OnTriggerJumpFromOtherSources(_jumpForce, _horizontalForce);
            if (_invertCharacterDirection) movementController.SwitchMoveDirection();
        }
    }
}

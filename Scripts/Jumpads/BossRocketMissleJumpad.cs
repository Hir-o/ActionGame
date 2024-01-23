
using System;
using UnityEngine;

public class BossRocketMissleJumpad : BaseJumpad
{
    public static event Action OnPlayerRocketBounce;

    private bool _hasBeenTriggerd;

    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    private void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn() => _hasBeenTriggerd = false;
    
    protected override void OnTriggerEnter(Collider other)
    {
    }
    
    public void OnJumpadTriggerEnter(Collider other)
    {
        if (_hasBeenTriggerd) return;
        if (CharacterPlayerController.Instance.IsDead) return;
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            OnPlayerRocketBounce?.Invoke();
            _hasBeenTriggerd = true;
            movementController.OnTriggerJumpFromOtherSources(_jumpForce, _horizontalForce);
            if (_invertCharacterDirection) movementController.SwitchMoveDirection();
        }
    }
}


using CharacterMovement;
using UnityEngine;

public class Ladder : MonoBehaviour, IUngrabbable
{
    [SerializeField] private bool _isFinalLadderPart;
    [SerializeField] private MoveDirection _moveDirection;

    public bool IsFinalLadderPart => _isFinalLadderPart;
    
    private void OnTriggerEnter(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (other.gameObject.TryGetComponent(out MovementController playerMovement))
        {
            playerMovement.ResetMovementSpeed();
            playerMovement.ClimbAction.Climb(_moveDirection, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (other.gameObject.TryGetComponent(out MovementController playerMovement))
            playerMovement.ClimbAction.StopClimb(true);
    }
}
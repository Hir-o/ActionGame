using UnityEngine;

public class WallAndPlatformCollider : BaseCollider
{
    private MovementController _movementController;

    private void Awake() => _movementController = GetComponentInParent<MovementController>();

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        MovementController.OnPlayerJump += ClearCollidedObjects;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        MovementController.OnPlayerJump -= ClearCollidedObjects;
    }

    private void PlayerRespawn_OnPlayerRespawn() => ClearCollidedObjects();

    private void OnTriggerStay(Collider other)
    {
        if (!_movementController.ClimbAction.IsClimbing) return;
        if (!_collidedObjects.Contains(other.gameObject))
            _collidedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_movementController.ClimbAction.IsClimbing) return;
        if (_collidedObjects.Contains(other.gameObject))
            _collidedObjects.Remove(other.gameObject);
    }

    public void ClearCollidedObjects() => _collidedObjects.Clear();
}
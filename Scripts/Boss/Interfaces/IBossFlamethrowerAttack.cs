using UnityEngine;

public interface IBossFlamethrowerAttack
{
    public void TeleportAtAttackPosition(Vector3 teleportDestination);
    public void ShootFlamethrower();
}

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
    [BoxGroup("Trigger"), SerializeField] protected float _triggerDistance = 5f;
    [BoxGroup("Trigger"), SerializeField] protected float _checkTriggerInterval = 0.5f;

    protected WaitForSeconds _wait;
    protected Coroutine _coroutine;
    private ITrapHitSfx _trapHitSfx;

    protected virtual void Awake() => _trapHitSfx = GetComponent<ITrapHitSfx>();
    protected virtual void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    protected virtual void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn() => Reset();
    protected abstract IEnumerator TryToTriggerTrap();

    protected float GetXDistanceToPlayer()
    {
        Vector3 flatEnemyPosition = new Vector3(transform.position.x, 0f, 0f);
        Vector3 flatPlayerPosition = new Vector3(MovementController.Instance.transform.position.x, 0f, 0f);
        return Vector3.Distance(flatEnemyPosition, flatPlayerPosition);
    }

    protected abstract void Reset();

    protected void OnTriggerEnter(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (other.TryGetComponent(out CharacterPlayerController player))
        {
            if (_trapHitSfx != null) _trapHitSfx.PlayHitSfx();
            player.Die();
        }
    }
}
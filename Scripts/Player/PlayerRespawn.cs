
using System;
using DG.Tweening;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public static event Action OnPlayerRespawn;

    [SerializeField] private float _respawnDelay = 1f;

    private WaitForSeconds _wait;

    private void OnEnable() => CharacterPlayerController.OnPlayerDied += Respawn;
    private void OnDisable() => CharacterPlayerController.OnPlayerDied -= Respawn;

    private void Respawn()
    {
        DOVirtual.Float(_respawnDelay, 0f, _respawnDelay, value => { }).OnComplete(() =>
        {
            transform.position = CheckpointManager.Instance.PlayerRespawnPosition;
        });

        DOVirtual.Float(_respawnDelay + 0.25f, 0f, _respawnDelay + 0.25f, value => { }).OnComplete(() =>
        {
            OnPlayerRespawn?.Invoke();
        });
    }
}
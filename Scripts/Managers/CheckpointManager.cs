

using Leon;
using UnityEngine;

public class CheckpointManager : SceneSingleton<CheckpointManager>
{
    private Vector3 _playerRespawnPosition;
    private int _currCheckpointIndex = -1;

    public Vector3 PlayerRespawnPosition => _playerRespawnPosition;

    protected override void Awake() => _playerRespawnPosition = CharacterPlayerController.Instance.transform.position;
    private void OnEnable() => Checkpoint.OnNewCheckpointReached += UpdateCheckpoint;
    private void OnDisable() => Checkpoint.OnNewCheckpointReached -= UpdateCheckpoint;

    private void UpdateCheckpoint(int checkpointOrderIndex, Vector3 newRespawnPosition)
    {
        if (_currCheckpointIndex >= checkpointOrderIndex) return;
        _currCheckpointIndex = checkpointOrderIndex;
        _playerRespawnPosition = newRespawnPosition;
    }
}
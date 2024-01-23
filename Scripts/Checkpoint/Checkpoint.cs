using System;
using NaughtyAttributes;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IUngrabbable
{
    [Range(1, 100)] [SerializeField] private int _checkpointOrderIndex;
    public static event Action<int, Vector3> OnNewCheckpointReached;

    public event Action OnCheckpointReached;

    [BoxGroup("Player Respawn Position"), SerializeField]
    private Transform _respawnTransform;

    private bool _hasBeenReached;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasBeenReached) return;
        if (other.gameObject.TryGetComponent(out CharacterPlayerController player))
        {
            OnNewCheckpointReached?.Invoke(_checkpointOrderIndex, _respawnTransform.position);
            OnCheckpointReached?.Invoke();
            _hasBeenReached = true;
        }
    }
}
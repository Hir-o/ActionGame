
using System;
using Leon;
using UnityEngine;

public class LevelFinish : SceneSingleton<LevelFinish>
{
    [SerializeField] private GameObject _celebrateCameraGameObject;

    private LevelFinishSoundEvent _levelFinishSoundEvent;

    public static event Action OnAnyLevelCompleted;
    public static event Action<Transform> OnAnyPassCelebrateCamera;

    private void Awake() => _levelFinishSoundEvent = GetComponent<LevelFinishSoundEvent>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementController movementController))
        {
            _celebrateCameraGameObject.SetActive(true);
            OnAnyLevelCompleted?.Invoke();
            OnAnyPassCelebrateCamera?.Invoke(_celebrateCameraGameObject.transform);
        }
    }
}
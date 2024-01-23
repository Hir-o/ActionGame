using UnityEngine;
using Lean.Pool;
using NaughtyAttributes;

public class RainManager : MonoBehaviour
{
    [SerializeField] private Transform _rainVFXTransform;
    [SerializeField] private float _ySpawnOffset;

    private Transform _spawnedRainVfxTransform;

    private void OnEnable() => LevelFinish.OnAnyPassCelebrateCamera += Level_OnAnyPassCelebrateCamera;
    private void OnDisable() => LevelFinish.OnAnyPassCelebrateCamera -= Level_OnAnyPassCelebrateCamera;

    private void Level_OnAnyPassCelebrateCamera(Transform celebrateCameraTransform)
    {
        if (_spawnedRainVfxTransform == null) return;
        _spawnedRainVfxTransform.parent = celebrateCameraTransform;
    }

    private void Start()
    {
        Vector3 spawnPosition = CharacterPlayerController.Instance.transform.position;
        spawnPosition.y += _ySpawnOffset;
        _spawnedRainVfxTransform = LeanPool.Spawn(_rainVFXTransform, spawnPosition, _rainVFXTransform.transform.rotation, CharacterPlayerController.Instance.transform);
    }
}

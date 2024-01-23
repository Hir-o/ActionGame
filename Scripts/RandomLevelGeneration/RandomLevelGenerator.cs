
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomLevelGenerator : SceneSingleton<RandomLevelGenerator>
{
    [SerializeField] private float _distanceBetweenParts = 543.3177f;
    [SerializeField] private LevelPart[] _levelPartArray;

    private LevelPart _spawnedLevelPart;

    private List<LevelPart> _spawnedLevelPartList = new List<LevelPart>();

    protected override void Awake()
    {
        base.Awake();
        SpawnNewLevelPart();
    }

    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    private void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn()
    {
        ResetLevelParts();
        _spawnedLevelPartList.ForEach(levelPart => LeanPool.Despawn(levelPart));
        _spawnedLevelPartList.Clear();
        SpawnNewLevelPart();
    }

    public void SpawnNewPartManually()
    {
        float newXPosition = _distanceBetweenParts;
        SpawnNewLevelPart(newXPosition);
    }

    public void SpawnNewLevelPartAtPosition(float levelPartPosition)
    {
        float newXPosition = levelPartPosition + _distanceBetweenParts;
        SpawnNewLevelPart(newXPosition);
    }

    private void SpawnNewLevelPart(float xPosition = 0f)
    {
        LevelPart[] availableLayerPartArray = GetAvailableLayerPartArray();
        if (availableLayerPartArray.Length == 0)
        {
            ResetLevelParts();
            availableLayerPartArray = GetAvailableLayerPartArray();
        }

        int randomLayerPart = Random.Range(0, availableLayerPartArray.Length);
        Vector3 spawnPosition = new Vector3(xPosition, 0f, 0f);
        LevelPart newLevelPart = availableLayerPartArray[randomLayerPart];
        _spawnedLevelPart = LeanPool.Spawn(newLevelPart, spawnPosition, newLevelPart.transform.rotation);
        _spawnedLevelPartList.Add(_spawnedLevelPart);
        DespawnOldLevelPart();
        newLevelPart.IsSpawned = true;
    }

    private void DespawnOldLevelPart()
    {
        if (_spawnedLevelPartList.Count > 2)
        {
            LevelPart oldLevelPart = _spawnedLevelPartList.First();
            _spawnedLevelPartList.Remove(oldLevelPart);
            LeanPool.Despawn(oldLevelPart);
        }
    }

    private void ResetLevelParts()
    {
        for (int i = 0; i < _levelPartArray.Length; i++)
            _levelPartArray[i].IsSpawned = false;
    }

    private LevelPart[] GetAvailableLayerPartArray() => _levelPartArray.Where(x => x.IsSpawned == false).ToArray();
}
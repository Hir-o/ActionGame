using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class VehiclePath : MonoBehaviour
{
    [BoxGroup("Path"), SerializeField] private Transform _startTransform;
    [BoxGroup("Path"), SerializeField] private Transform _destinationTransform;

    [BoxGroup("Spawnable Vehicles"), SerializeField]
    private GameObject[] _spawnableVehicleArray;

    [BoxGroup("SpawnDelay"), SerializeField]
    private float _minSpawnDelay = 4f;

    [BoxGroup("SpawnDelay"), SerializeField]
    private float _maxSpawnDelay = 8f;

    [BoxGroup("Vehicle Movement Speed"), SerializeField]
    private float _vehicleMovementSpeed = 3f;

    [BoxGroup("Vehicle Rotation"), SerializeField]
    private float _vehicleYRotation  = 90f;

    [SerializeField] private List<Transform> _movingVehicleList;

    private float _spawnTimer;

    private float _randomSpawnDelay => Random.Range(_minSpawnDelay, _maxSpawnDelay);

    private void Start()
    {
        _spawnTimer = _randomSpawnDelay;

        for (int i = 0; i < _movingVehicleList.Count; i++)
            if (_movingVehicleList[i] == null)
                _movingVehicleList.Remove(_movingVehicleList[i]);
    }

    private void FixedUpdate()
    {
        HandleVehicleSpawn();
        HandleVehicleMovement();
    }

    private void HandleVehicleSpawn()
    {
        _spawnTimer -= Time.fixedDeltaTime;
        if (_spawnTimer <= 0f)
        {
            GameObject newVehicle = SelectRandomVehicle();
            GameObject spawnedVehicle = LeanPool.Spawn(newVehicle, _startTransform.position,
                Quaternion.Euler(0, _vehicleYRotation, 0f), transform);
            _movingVehicleList.Add(spawnedVehicle.transform);
            _spawnTimer = _randomSpawnDelay;
        }
    }

    private void HandleVehicleMovement()
    {
        float step = _vehicleMovementSpeed * Time.fixedDeltaTime;
        for (int i = 0; i < _movingVehicleList.Count; i++)
        {
            _movingVehicleList[i].transform.localPosition = Vector3.MoveTowards(
                _movingVehicleList[i].transform.localPosition, _destinationTransform.localPosition, step);

            if (Vector3.Distance(_movingVehicleList[i].transform.localPosition, _destinationTransform.localPosition) <
                0.1f)
            {
                GameObject vehicleToDespawn = _movingVehicleList[i].gameObject;
                _movingVehicleList.Remove(_movingVehicleList[i]);
                LeanPool.Despawn(vehicleToDespawn.gameObject);
            }
        }
    }

    private GameObject SelectRandomVehicle()
    {
        int vehicleArrayLength = _spawnableVehicleArray.Length - 1;
        int randomIndex = Random.Range(0, vehicleArrayLength);
        return _spawnableVehicleArray[randomIndex];
    }
}
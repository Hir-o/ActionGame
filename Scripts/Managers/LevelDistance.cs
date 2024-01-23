using System.Collections;
using UnityEngine;
using System;

public class LevelDistance : MonoBehaviour
{
    private Vector3 _startingPoint;
    private Vector3 _endPoint;

    private float _totalDistance;
    private float _currentDistance;

    private CharacterPlayerController _characterPlayerController;
    private LevelFinish _levelFinish;
    [SerializeField] private float _waitPeriod = 5f;

    public static event Action<float, float> OnCalculatingDistance;

    private WaitForSeconds _waitForSeconds;

    void Awake()
    {
        _characterPlayerController = CharacterPlayerController.Instance;
        _levelFinish = LevelFinish.Instance;
        _startingPoint = _characterPlayerController.transform.position;
        _endPoint = _levelFinish.transform.position;
        _totalDistance = DistanceCalculator.CalculateDistance(_startingPoint, _endPoint);
        _waitForSeconds = new WaitForSeconds(_waitPeriod);
    }

    private void Start() => StartCoroutine(CheckCurrentDistance());

    private IEnumerator CheckCurrentDistance()
    {
        while (true)
        {
            yield return _waitForSeconds;
            _startingPoint = _characterPlayerController.transform.position;
            _endPoint = _levelFinish.transform.position;
            _currentDistance = DistanceCalculator.CalculateDistance(_startingPoint, _endPoint);
            OnCalculatingDistance?.Invoke(_currentDistance, _totalDistance);
        }
    }
}
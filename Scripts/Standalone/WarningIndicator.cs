
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class WarningIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _stationaryIndicator;
    [SerializeField] private float _moveSpeed = 35f;
    [SerializeField] private Ease _moveEase = Ease.OutCubic;
    [SerializeField] private float _indicatorSpawnDuration = .1f;

    private ObjectLeanDespawner _objectLeanDespawner;
    private Tween _movePathTween;
    private List<StationaryIndicator> _stationaryIndicatorList = new List<StationaryIndicator>();

    private void Awake() => _objectLeanDespawner = GetComponent<ObjectLeanDespawner>();

    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        DisableStationaryIndicators();
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        if (_movePathTween != null) _movePathTween.Kill();
        _objectLeanDespawner.InstantDespawn();  
    }

    public void MovePath(Transform[] waypointArray)
    {
        _stationaryIndicatorList.Clear();
        float timer = _indicatorSpawnDuration;
        Vector3[] waypointPoisitionArray = waypointArray.Select(x => x.transform.position).ToArray();
        if (_movePathTween != null) _movePathTween.Kill();
        transform.position = waypointArray[0].position;
        transform.DOPath(waypointPoisitionArray, _moveSpeed, PathType.CatmullRom)
            .SetSpeedBased()
            .SetEase(_moveEase)
            .OnUpdate(() =>
            {
                float _endWaypointDistance =
                    Vector3.Distance(transform.position, waypointArray[waypointArray.Length - 1].position);
                if (_endWaypointDistance < 3f) return;
                if (_stationaryIndicator == null)
                    return;
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = _indicatorSpawnDuration;
                    GameObject indicator = LeanPool.Spawn(_stationaryIndicator.gameObject, transform.position,
                        _stationaryIndicator.transform.rotation);
                    _stationaryIndicatorList.Add(indicator.GetComponent<StationaryIndicator>());
                }
            });
    }

    private void DisableStationaryIndicators()
    {
        if (_stationaryIndicatorList.Count == 0) return;
        _stationaryIndicatorList.ForEach(x => x.DespawnInstanty());
    }
}
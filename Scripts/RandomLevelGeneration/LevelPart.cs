
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    private List<BossHorizontalRocketTrigger> _bossHorizontalRocketTriggerList = new();

    private bool _isSpawned;

    #region Properties

    public int Index => _index;
    public Transform StartTransform => _startTransform;
    public Transform EndTransform => _endTransform;

    public bool IsSpawned
    {
        get => _isSpawned;
        set => _isSpawned = value;
    }

    #endregion

    private void Awake() =>
        _bossHorizontalRocketTriggerList = GetComponentsInChildren<BossHorizontalRocketTrigger>().ToList();

    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    private void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn() => _bossHorizontalRocketTriggerList.ForEach(x => x.Reset());
}
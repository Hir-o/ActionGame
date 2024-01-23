
using System;
using UnityEngine;

public class WaterEntry : Invisibler, IUngrabbable
{
    public static Action OnDiveIntoWater;

    [SerializeField] private Collider _topCollider;

    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    private void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn() => _topCollider.enabled = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            OnDiveIntoWater?.Invoke();
            _topCollider.enabled = true;
        }
    }
}
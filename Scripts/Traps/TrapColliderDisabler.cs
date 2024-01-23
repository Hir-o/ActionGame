using System;
using UnityEngine;

public class TrapColliderDisabler : MonoBehaviour, IColliderDisable
{
    private Collider _collider;

    private void Awake() => _collider = GetComponent<Collider>();

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += ResetCollider;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= ResetCollider;
    }

    public void ResetCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}

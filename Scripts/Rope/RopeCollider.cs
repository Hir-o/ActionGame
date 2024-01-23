using System;
using UnityEngine;

public class RopeCollider : MonoBehaviour
{
    [SerializeField] private SimpleRopeController _ropeController;

    private void Awake()
    {
        if (_ropeController == null) _ropeController = GetComponentInParent<SimpleRopeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ropeController.IsPlayerAttached) return; 
        if (!other.TryGetComponent(out MovementController playerMovement)) return;
        _ropeController.HandleRopeTriggerCollision(other);
    }
}

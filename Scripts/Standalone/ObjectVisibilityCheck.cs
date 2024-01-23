
using System;
using UnityEngine;

public class ObjectVisibilityCheck : MonoBehaviour
{
    public static event Action OnAnyCoinCheck;
    public static event Action OnAnyFlyingEquipmentCheck;

    [SerializeField] private bool _isEnabled = true; 

    private void Update()
    {
        if (!_isEnabled) return;
        OnAnyCoinCheck?.Invoke();
        OnAnyFlyingEquipmentCheck?.Invoke();
    } 
}

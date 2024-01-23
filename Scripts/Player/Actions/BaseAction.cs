

using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public abstract class BaseAction : MonoBehaviour
{
    protected MovementController _playerMovementController;
    
    [BoxGroup("Is Enabled"), SerializeField] private bool _isEnabled = true;

    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    protected virtual void Awake()
    {
        _playerMovementController = GetComponent<MovementController>();
        enabled = _isEnabled;
    }
}

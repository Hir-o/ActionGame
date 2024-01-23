using System;
using UnityEngine;

public class Reorientator : Invisibler, IUngrabbable
{
    public static event Action OnPlayerEnter;

    [SerializeField] private bool _keepCurrentCamera;
    
    private bool _isEnabled;

    private void OnEnable() => CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
    private void OnDisable() => CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
        {
            if (!_isEnabled)
            {
                _isEnabled = true;
                if (!_keepCurrentCamera)
                    OnPlayerEnter?.Invoke();
                return;
            }

            movementController.SwitchMoveDirection();
        }
    }

    private void CharacterPlayerController_OnPlayerDied() => Reset();

    private void Reset() => _isEnabled = false;
}
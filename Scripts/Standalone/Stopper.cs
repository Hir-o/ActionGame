
using UnityEngine;

public class Stopper : Invisibler, IUngrabbable
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovementController movementController))
            movementController.StopMovingHorizontally();
    }
}
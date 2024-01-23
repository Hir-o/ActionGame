using UnityEngine;

public class LedgeCollider : BaseCollider
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IUngrabbable ungrabbableObject)) return;
        if (!_collidedObjects.Contains(other.gameObject))
            _collidedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IUngrabbable ungrabbableObject)) return;
        if (_collidedObjects.Contains(other.gameObject))
            _collidedObjects.Remove(other.gameObject);
    }
}
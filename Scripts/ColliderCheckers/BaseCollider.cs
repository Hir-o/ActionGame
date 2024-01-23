using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCollider : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _collidedObjects = new List<GameObject>();
    public List<GameObject> CollidedObjects => _collidedObjects;
}
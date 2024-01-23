
using System;
using UnityEngine;

public abstract class BaseJumpad : MonoBehaviour, IUngrabbable
{
    [Range(0, 100f), SerializeField] protected float _jumpForce;
    [Range(0, 100f), SerializeField] protected float _horizontalForce;
    [SerializeField] protected bool _invertCharacterDirection;

    protected abstract void OnTriggerEnter(Collider other);
}


using System;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
{
    [SerializeField] private List<Renderer> _rendererList = new List<Renderer>();
    [SerializeField] private List<Material> _materialList = new List<Material>();
    private Vector3 _position;
    private float _initialAlpha;

    #region Properties

    public List<Renderer> RendererList
    {
        get => _rendererList;
        set => _rendererList = value;
    }

    public Vector3 Position => _position;

    public List<Material> MaterialList
    {
        get => _materialList;
        set => _materialList = value;
    }

    public float InitialAlpha
    {
        get => _initialAlpha;
        set => _initialAlpha = value;
    }

    #endregion

    private void Awake()
    {
        _position = transform.position;
        if (_rendererList.Count == 0)
            _rendererList.AddRange(GetComponentsInChildren<Renderer>());
        
        _rendererList.ForEach((renderer) =>
        {
            _materialList.AddRange(renderer.materials);
        });

        _initialAlpha = _materialList[0].color.a;
    }

    public static bool operator ==(FadingObject first, FadingObject second) => Equals(first, second);

    public static bool operator !=(FadingObject first, FadingObject second) => !Equals(first, second);

    public bool Equals(FadingObject other) => (_position) == (other.Position);

    public override int GetHashCode() => HashCode.Combine(_position);
}
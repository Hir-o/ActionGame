using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private GameObject _gfx;
    private Plane[] _cameraFrustum;
    private Collider _collider;
    private MeshRenderer _meshRenderer;

    #region Properties

    public Collider Collider => _collider;
    public MeshRenderer MeshRenderer => _meshRenderer;

    public Plane[] CameraFrustum
    {
        get => _cameraFrustum;
        set => _cameraFrustum = value;
    }

    #endregion

    protected GameObject Gfx
    {
        get => _gfx;
        set => _gfx = value;
    }

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null) _collider = _gfx.GetComponentInChildren<Collider>();
        _meshRenderer = _gfx.GetComponentInChildren<MeshRenderer>();
        if (_collider != null) _collider.enabled = false;
        if (_meshRenderer != null) _meshRenderer.enabled = false;
    }

    protected virtual void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;

    protected virtual void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    protected virtual void PlayerRespawn_OnPlayerRespawn()
    {
        if (_collider != null) _collider.enabled = true;
        if (_meshRenderer != null) _meshRenderer.enabled = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == CharacterPlayerController.Instance.CollectableMagnet.gameObject.GetInstanceID())
            OnItemCollected();
    }

    protected virtual void CheckIfIsInCameraView()
    {
        if (this is FlyingEquipmentCollectable flyingEquipmentCollectable)
            if (flyingEquipmentCollectable.IsCollected)
                return;

        var bounds = _collider.bounds;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool activateComponents = GeometryUtility.TestPlanesAABB(_cameraFrustum, bounds);
        if (_meshRenderer != null) _meshRenderer.enabled = activateComponents;
        _collider.enabled = activateComponents;
    }

    public virtual void OnItemCollected()
    {
        if (_collider != null) _collider.enabled = false;
        if (_meshRenderer != null) _meshRenderer.enabled = false;
    }
}
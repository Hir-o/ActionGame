using System;
using Factory;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;
using LeonFollowPlayer = Leon.FollowPlayer;

public class Coin : BaseCollectable
{
    public static event Action OnCollectCoin;
    
    private ParticleSystem[] _twinkleParticleArray;
    private LeonFollowPlayer _followPlayer;
    private Camera _mainCamera;
    private CollectableTween _collectableTween;
    [SerializeField] private VisibilityEventHandler _visibilityEventHandler;

    private bool _isCollected;
    private bool _canReset = true;
    private bool _isVisible;

    #region Properties

    public bool IsCollected { get; set; }
    public bool IsVisible => _visibilityEventHandler.IsVisible;
    public LeonFollowPlayer FollowPlayer => _followPlayer;

    public bool CanReset
    {
        get => _canReset;
        set => _canReset = value;
    }

    #endregion

    protected override void Awake()
    {
        //base.Awake();
        _followPlayer = GetComponent<LeonFollowPlayer>();
        _collectableTween = GetComponent<CollectableTween>();
        _twinkleParticleArray = GetComponentsInChildren<ParticleSystem>();
        _visibilityEventHandler = GetComponentInChildren<VisibilityEventHandler>();

        CoinsManager.Instance.CoinList.Add(this);
    }

    private void Start() => CoinsManager.Instance.AddToTotalCoinAmount();

    protected override void OnEnable()
    {
        base.OnEnable();
        //ObjectVisibilityCheck.OnAnyCoinCheck += CheckIfIsInCameraView;
        Checkpoint.OnNewCheckpointReached += Checkpoint_OnNewCheckpointReached;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //ObjectVisibilityCheck.OnAnyCoinCheck -= CheckIfIsInCameraView;
        Checkpoint.OnNewCheckpointReached -= Checkpoint_OnNewCheckpointReached;
    }

    protected override void PlayerRespawn_OnPlayerRespawn()
    {
        if (!CanReset) return;
        Reset();
    }

    private void Checkpoint_OnNewCheckpointReached(int checkpointIndex, Vector3 checkpointPosition)
    {
        if (_isCollected) CanReset = false;
    }

    public override void OnItemCollected()
    {
        if (_isCollected) return;
        _isCollected = true;
        //Gfx.SetActive(false);
        foreach (var particleSystem in _twinkleParticleArray) particleSystem.Stop();
        if (_collectableTween != null) _collectableTween.StartTweening(SpawnCollectedVfx);
        if (_followPlayer != null) _followPlayer.Reset();
        OnCollectCoin?.Invoke();
    }

    protected virtual void CheckIfIsInCameraView()
    {
        //todo optimize this method
        if (_isCollected) return;
        var bounds = Collider.bounds;
        CameraFrustum = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
        bool activateComponents = GeometryUtility.TestPlanesAABB(CameraFrustum, bounds);
        Collider.enabled = activateComponents;
    }

    public virtual void SpawnCollectedVfx() =>
        CollectableParticleFactory.Instance.GetNewCoinVfxInstance(transform.position);

    private void Reset()
    {
        _isCollected = false;
        Gfx.SetActive(true);
        foreach (var particleSystem in _twinkleParticleArray) particleSystem.Play();
        if (_collectableTween != null) _collectableTween.Reset();
    }
}
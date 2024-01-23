using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{
    public event Action OnGetDamaged;
    public event Action OnStartCrumbling;
    public event Action OnCrumble;

    [SerializeField] private Collider[] _colliderArray;

    [BoxGroup("Crumble"), SerializeField] private float _crumbleDelay = .25f;

    [BoxGroup("Crumble"), SerializeField] private float _colliderDisableDelay = .2f;

    private CrumblingPlatformAnimationController _crumblingPlatformAnimationController;
    private CrumblingPlatformMaterialFader _crumblingPlatformMaterialFader;
    private Tween _crumbleTween;
    private Tween _delayTween;

    private bool _isCrumbling;

    private void Awake()
    {
        _colliderArray = GetComponents<Collider>();
        _crumblingPlatformAnimationController = GetComponent<CrumblingPlatformAnimationController>();
        _crumblingPlatformMaterialFader = GetComponent<CrumblingPlatformMaterialFader>();
    }

    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    private void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn() => Reset();

    private void OnTriggerEnter(Collider other)
    {
        if (_isCrumbling) return;
        if (other.TryGetComponent(out MovementController _)) StartCrumbling();
    }

    private void StartCrumbling()
    {
        _isCrumbling = true;
        if (_crumbleTween != null && _crumbleTween.IsPlaying()) _crumbleTween.Kill();
        OnGetDamaged?.Invoke();
        _crumbleTween = DOVirtual.DelayedCall(_crumbleDelay, Crumble);
    }

    public void Crumble()
    {
        OnStartCrumbling?.Invoke();
        if (_delayTween != null && _delayTween.IsPlaying()) _delayTween.Kill();
        _delayTween = DOVirtual.DelayedCall(_colliderDisableDelay, () =>
        {
            OnCrumble?.Invoke();
            for (int i = 0; i < _colliderArray.Length; i++)
                _colliderArray[i].enabled = false;
        });
    }

    private void Reset()
    {
        if (_crumbleTween != null && _crumbleTween.IsPlaying()) _crumbleTween.Kill();
        if (_delayTween != null && _delayTween.IsPlaying()) _delayTween.Kill();
        _isCrumbling = false;
        for (int i = 0; i < _colliderArray.Length; i++) _colliderArray[i].enabled = true;
        _crumblingPlatformAnimationController.ResetAnimation();
        _crumblingPlatformMaterialFader.ResetMaterialProperties();
    }
}
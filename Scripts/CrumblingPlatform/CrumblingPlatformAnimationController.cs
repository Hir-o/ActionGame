using System;
using NaughtyAttributes;
using UnityEngine;

public class CrumblingPlatformAnimationController : MonoBehaviour
{
    [BoxGroup("AnimationClipData"), SerializeField] private AnimationClipData _idleClipData;
    [BoxGroup("AnimationClipData"), SerializeField] private AnimationClipData _damagedClipData;
    [BoxGroup("AnimationClipData"), SerializeField] private AnimationClipData _crumblePhase1ClipData;
    
    private CrumblingPlatform _crumblingPlatform;
    private Animator _animator;

    private void Awake()
    {
        _crumblingPlatform = GetComponent<CrumblingPlatform>();
        _animator = GetComponentInChildren<Animator>();   
    }

    private void OnEnable()
    {
        _crumblingPlatform.OnGetDamaged += CrumblingPlatform_OnGetDamaged;
        _crumblingPlatform.OnStartCrumbling += CrumblingPlatform_OnStartCrumbling;
    }

    private void OnDisable()
    {
        _crumblingPlatform.OnGetDamaged -= CrumblingPlatform_OnGetDamaged;
        _crumblingPlatform.OnStartCrumbling -= CrumblingPlatform_OnStartCrumbling;
    }

    private void CrumblingPlatform_OnGetDamaged() => PlayAnimation(_damagedClipData);
    private void CrumblingPlatform_OnStartCrumbling() => PlayAnimation(_crumblePhase1ClipData);

    private void PlayAnimation(AnimationClipData clipData)
    {
        clipData.IsPlayingAnimation = true;
        _animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
    }

    public void ResetAnimation()
    {
        _idleClipData.IsPlayingAnimation = false;
        _damagedClipData.IsPlayingAnimation = false;
        _crumblePhase1ClipData.IsPlayingAnimation = false;
        PlayAnimation(_idleClipData);
    }
}

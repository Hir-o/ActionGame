using System;
using UnityEngine;

public class FirstBossAnimationController : BaseBossAnimationController
{
    private BaseBoss _baseBoss;

    private void Awake()
    {
        base.Awake();
        _baseBoss = GetComponent<BaseBoss>();
    }

    private void OnEnable()
    {
        _baseBoss.OnDash += BaseBoss_OnDash;
        _baseBoss.OnStopDash += BaseBoss_OnStopDash;
    }

    private void OnDisable()
    {
        _baseBoss.OnDash -= BaseBoss_OnDash;
        _baseBoss.OnStopDash -= BaseBoss_OnStopDash;
    }

    private void BaseBoss_OnDash()
    {
        ResetAnimation();
        PlayAnimation(AttackClip);
    }

    private void BaseBoss_OnStopDash()
    {
        ResetAnimation();
        PlayAnimation(IdleClip);
    }

    public override void PlayAnimation(AnimationClipData clipData)
    {
        clipData.IsPlayingAnimation = true;
        Animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
    }

    public override void ResetAnimation()
    {
        IdleClip.IsPlayingAnimation = false;
        AttackClip.IsPlayingAnimation = false;
    }
}
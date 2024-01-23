using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IBirdMechPatrol))]
public class MechBirdAnimationController : BaseEnemyAnimationController
{
    [SerializeField] private AnimationClipData _ascendClip;
    [SerializeField] private AnimationClipData _descendClip;

    private IBirdMechPatrol _birdMechPatrol;

    protected override void Awake()
    {
        base.Awake();
        _birdMechPatrol = GetComponent<IBirdMechPatrol>();
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        _birdMechPatrol.OnAscend += BirdMechPatrol_OnAscend;
        _birdMechPatrol.OnDescend += BirdMechPatrol_OnDescend;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        _birdMechPatrol.OnAscend -= BirdMechPatrol_OnAscend;
        _birdMechPatrol.OnDescend -= BirdMechPatrol_OnDescend;
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        ResetAnimation();
        PlayAnimation(IdleClip);
    }

    private void BirdMechPatrol_OnAscend()
    {
        ResetAnimation();
        PlayAnimation(_ascendClip);
    }

    private void BirdMechPatrol_OnDescend()
    {
        ResetAnimation();
        PlayAnimation(_descendClip);
    }

    public override void ResetAnimation()
    {
        IdleClip.IsPlayingAnimation = false;
        _ascendClip.IsPlayingAnimation = false;
        _descendClip.IsPlayingAnimation = false;
    }

    public override void PlayAnimation(AnimationClipData clipData)
    {
        clipData.IsPlayingAnimation = true;
        Animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
    }
}
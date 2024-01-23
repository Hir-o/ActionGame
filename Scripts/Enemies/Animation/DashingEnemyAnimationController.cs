using UnityEngine;

[RequireComponent(typeof(IDashingBotInterface))]
public class DashingEnemyAnimationController : BaseEnemyAnimationController
{
    private IDashingBotInterface _dashingBotPatrol;

    protected override void Awake()
    {
        base.Awake();
        _dashingBotPatrol = GetComponent<IDashingBotInterface>();
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        _dashingBotPatrol.DashingBotOnIdle += DashingBotPatrol_DashingBotOnIdle;
        _dashingBotPatrol.DashingBotOnAttack += DashingBotPatrol_DashingBotOnAttack;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        _dashingBotPatrol.DashingBotOnIdle -= DashingBotPatrol_DashingBotOnIdle;
        _dashingBotPatrol.DashingBotOnAttack -= DashingBotPatrol_DashingBotOnAttack;
    }

    private void PlayerRespawn_OnPlayerRespawn() => ResetToIdleAnimation();

    private void DashingBotPatrol_DashingBotOnIdle() => ResetToIdleAnimation();

    private void DashingBotPatrol_DashingBotOnAttack()
    {
        if (PatrolClip.IsPlayingAnimation) return;
        ResetAnimation();
        PatrolClip.IsPlayingAnimation = true;
        PlayAnimation(PatrolClip);
    }

    private void ResetToIdleAnimation()
    {
        if (IdleClip.IsPlayingAnimation) return;
        ResetAnimation();
        IdleClip.IsPlayingAnimation = true;
        PlayAnimation(IdleClip);
    }

    public override void ResetAnimation()
    {
        IdleClip.IsPlayingAnimation = false;
        PatrolClip.IsPlayingAnimation = false;
    }

    public override void PlayAnimation(AnimationClipData clipData)
    {
        clipData.IsPlayingAnimation = true;
        Animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
    }
}
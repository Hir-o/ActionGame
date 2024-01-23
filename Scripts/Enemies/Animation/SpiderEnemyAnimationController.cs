using UnityEngine;

[RequireComponent(typeof(ISpiderInterface))]
public class SpiderEnemyAnimationController : BaseEnemyAnimationController
{
    private ISpiderInterface _spiderMechPatrol;

    protected override void Awake()
    {
        base.Awake();
        _spiderMechPatrol = GetComponent<ISpiderInterface>();
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        _spiderMechPatrol.SpiderOnIdle += SpiderMechPatrol_SpiderOnIdle;
        _spiderMechPatrol.SpiderOnAttack += SpiderMechPatrol_SpiderOnDescend;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        _spiderMechPatrol.SpiderOnIdle -= SpiderMechPatrol_SpiderOnIdle;
        _spiderMechPatrol.SpiderOnAttack -= SpiderMechPatrol_SpiderOnDescend;
    }

    private void PlayerRespawn_OnPlayerRespawn() => ResetToIdleAnimation();
    private void SpiderMechPatrol_SpiderOnIdle() => ResetToIdleAnimation();

    private void ResetToIdleAnimation()
    {
        if (IdleClip.IsPlayingAnimation) return;
        ResetAnimation();
        PlayAnimation(IdleClip);
    }

    private void SpiderMechPatrol_SpiderOnDescend()
    {
        if (PatrolClip.IsPlayingAnimation) return;
        ResetAnimation();
        PlayAnimation(PatrolClip);
    }

    public override void PlayAnimation(AnimationClipData clipData)
    {
        clipData.IsPlayingAnimation = true;
        Animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
    }
    
    public override void ResetAnimation()
    {
        IdleClip.IsPlayingAnimation = false;
        PatrolClip.IsPlayingAnimation = false;
    }
}
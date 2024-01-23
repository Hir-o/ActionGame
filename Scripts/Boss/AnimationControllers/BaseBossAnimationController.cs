using UnityEngine;

public abstract class BaseBossAnimationController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private AnimationClipData _idleClip;
    [SerializeField] private AnimationClipData _attackClip;

    public Animator Animator => _animator;
    public AnimationClipData IdleClip => _idleClip;
    public AnimationClipData AttackClip => _attackClip;

    protected virtual void Awake() => _animator = GetComponentInChildren<Animator>();

    public abstract void PlayAnimation(AnimationClipData clipData);
    
    public abstract void ResetAnimation();
}
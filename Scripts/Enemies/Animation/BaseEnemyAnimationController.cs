
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAnimationController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private AnimationClipData _idleClip;
    [SerializeField] private AnimationClipData _patrolClip;

    //properties
    public AnimationClipData IdleClip { get => _idleClip; }
    public AnimationClipData PatrolClip { get => _patrolClip; }
    public Animator Animator { get => _animator; }

    protected virtual void Awake() => _animator = GetComponentInChildren<Animator>();

    public abstract void ResetAnimation();

    public abstract void PlayAnimation(AnimationClipData clipData);
}

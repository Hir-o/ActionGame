using System;
using UnityEngine;

[Serializable]
public class AnimationClipData
{
    [SerializeField] private AnimationClip _animation;
    private int _animId;

    public AnimationClip Animation => _animation;
    [SerializeField] private float _additionalTransitionTime;

    public float AdditionalTransitionTime => _additionalTransitionTime;

    public int AnimId
    {
        get
        {
            if (_animId == 0) _animId = Animator.StringToHash(_animation.name);
            return _animId;
        }
    }
    
    public bool IsPlayingAnimation { get; set; }
}
using System;
using UnityEngine;

[Serializable]
public class CelebrationAnimationClipData : AnimationClipData
{
    [SerializeField] private bool _allowTransitionToOtherAnimationState;
    
    public bool AllowTransitionToOtherAnimationState => _allowTransitionToOtherAnimationState;
}

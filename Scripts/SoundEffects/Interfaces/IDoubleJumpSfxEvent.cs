using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoubleJumpSfxEvent : ISoundEffectEvent
{
    public List<AudioClip> AudioClipList { get; }
}
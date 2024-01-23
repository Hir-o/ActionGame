using System.Collections.Generic;
using UnityEngine;

public interface ICoinCollectSfxEvent : ISoundEffectEvent
{
    public List<AudioClip> AudioClipList { get; }
}
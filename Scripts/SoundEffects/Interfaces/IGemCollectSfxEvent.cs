using System.Collections.Generic;
using UnityEngine;

public interface IGemCollectSfxEvent : ISoundEffectEvent
{
    public List<AudioClip> AudioClipList { get; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpSfxEvent : ISoundEffectEvent
{
    public List<AudioClip> AudioClipList { get; }
}

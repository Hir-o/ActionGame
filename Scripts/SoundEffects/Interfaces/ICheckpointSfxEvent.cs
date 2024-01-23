using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpointSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}
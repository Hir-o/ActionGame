using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRocketExplosionSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}
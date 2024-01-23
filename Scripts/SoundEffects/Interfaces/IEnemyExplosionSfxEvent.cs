using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyExplosionSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}
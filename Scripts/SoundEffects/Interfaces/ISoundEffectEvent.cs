using System;
using UnityEngine;

public interface ISoundEffectEvent
{
    public void Execute(Action<AudioClip, bool, float> sfxPlayCallback, float pitch = 1f);
}
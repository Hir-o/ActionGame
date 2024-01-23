using System;
using UnityEngine;

public class DeathSfxEvent : BaseSingleAudioClipSfxEvent, ICheckpointSfxEvent
{
    public AudioClip AudioClip => _audioClip;

    public void Execute(Action<AudioClip, bool, float> sfxPlayCallback, float pitch = 1) =>
        sfxPlayCallback?.Invoke(_audioClip, false, pitch);
}

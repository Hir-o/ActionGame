using UnityEngine;

public interface IJumpadSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}

using UnityEngine;

public interface ISlideSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}

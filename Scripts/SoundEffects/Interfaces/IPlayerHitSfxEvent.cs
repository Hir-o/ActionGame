using UnityEngine;

public interface IPlayerHitSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}
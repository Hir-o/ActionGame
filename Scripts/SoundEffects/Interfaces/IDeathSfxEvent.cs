using UnityEngine;

public interface IDeathSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}

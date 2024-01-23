using UnityEngine;

public interface ILoseSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}

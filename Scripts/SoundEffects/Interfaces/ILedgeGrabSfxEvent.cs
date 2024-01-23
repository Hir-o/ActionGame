using UnityEngine;

public interface ILedgeGrabSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}
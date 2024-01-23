using UnityEngine;

public interface IWinSfxEvent : ISoundEffectEvent
{
    public AudioClip AudioClip { get; }
}
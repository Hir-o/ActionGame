using UnityEngine;

public interface IContainsAudioClipArray
{
    public AudioClip[] ClipArray { get; set; }
    AudioClip GetRandomAudioClip();
    void UpdateAvailableAudioClipList();
}

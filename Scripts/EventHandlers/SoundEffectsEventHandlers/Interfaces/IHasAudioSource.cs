using UnityEngine;

public interface IHasAudioSource
{
    public AudioSource AudioSource { get; set; }
    public void ExecutePlaySfx();
}

using NaughtyAttributes;
using UnityEngine;

public class BaseSingleAudioClipSfxEvent : MonoBehaviour
{
    [BoxGroup("Audio Clips")] [SerializeField]
    protected AudioClip _audioClip;
}

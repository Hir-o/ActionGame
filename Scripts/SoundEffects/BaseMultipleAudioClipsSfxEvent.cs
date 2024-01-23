using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseMultipleAudioClipsSfxEvent : MonoBehaviour
{
    [BoxGroup("Audio Clips")] [SerializeField]
    protected List<AudioClip> _audioClipList = new List<AudioClip>();
    
    private List<AudioClip> _availableSfxClipList = new List<AudioClip>();

    public List<AudioClip> AvailableSfxClipList
    {
        get => _availableSfxClipList;
        set => _availableSfxClipList = value;
    }
    
    protected virtual void Awake() => UpdateAvailableSfxList();
    
    protected virtual void UpdateAvailableSfxList()
    {
        for (int i = 0; i < _audioClipList.Count; i++)
            AvailableSfxClipList.Add(_audioClipList[i]);
    }
}
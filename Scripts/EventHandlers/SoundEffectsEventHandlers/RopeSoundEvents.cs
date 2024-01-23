using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSoundEvents : MonoBehaviour, IHasAudioSource, IContainsAudioClipArray
{
    [SerializeField] private AudioClip[] _clipArray;

    private AudioSource _audioSource;
    private List<AudioClip> _availableAudioClips = new List<AudioClip>();

    #region Properties

    public AudioSource AudioSource
    {
        get => _audioSource;
        set => _audioSource = value;
    }

    public AudioClip[] ClipArray
    {
        get => _clipArray;
        set => _clipArray = value;
    }

    #endregion
    
    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        UpdateAvailableAudioClipList();
    }

    public void ExecutePlaySfx()
    {
        if (_audioSource == null) return;
        StopAllCoroutines();
        AudioClip clipToPlay = GetRandomAudioClip();
        _audioSource.PlayOneShot(clipToPlay);
    }

    public AudioClip GetRandomAudioClip()
    {
        if (_availableAudioClips.Count == 0) UpdateAvailableAudioClipList();

        int randomClipIndex = Random.Range(0, _availableAudioClips.Count - 1);
        AudioClip selectedSfx = _availableAudioClips[randomClipIndex];
        _availableAudioClips.Remove(selectedSfx);
        return selectedSfx;
    }

    public void UpdateAvailableAudioClipList()
    {
        for (int i = 0; i < _clipArray.Length; i++)
            _availableAudioClips.Add(_clipArray[i]);
    }
}
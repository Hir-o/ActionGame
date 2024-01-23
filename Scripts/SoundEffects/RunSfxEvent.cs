using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RunSfxEvent : BaseMultipleAudioClipsSfxEvent, IRunSfxEvent
{
    public List<AudioClip> AudioClipList
    {
        get => _audioClipList;
    }

    public void Execute(Action<AudioClip, bool, float> sfxPlayCallback, float pitch = 1f)
    {
        if (AvailableSfxClipList.Count == 0) UpdateAvailableSfxList();

        int randomClipIndex = Random.Range(0, AvailableSfxClipList.Count - 1);
        AudioClip selectedSfx = AvailableSfxClipList[randomClipIndex];
        AvailableSfxClipList.Remove(selectedSfx);
        sfxPlayCallback?.Invoke(selectedSfx, false, pitch);
    }
}

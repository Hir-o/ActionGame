using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpSfxEvent : BaseMultipleAudioClipsSfxEvent, IJumpSfxEvent, ISoundEffectCooldown
{
    [SerializeField] private float _cooldownDuration = 0.1f;

    private WaitForSeconds _wait;
    
    private bool _isInCooldown;
    
    public List<AudioClip> AudioClipList
    {
        get => _audioClipList;
    }

    public float CooldownDuration => _cooldownDuration;
    public bool IsInCooldown
    {
        get => _isInCooldown;
        set => _isInCooldown = value;
    }

    private void Awake() => _wait = new WaitForSeconds(_cooldownDuration);

    public void Execute(Action<AudioClip, bool, float> sfxPlayCallback, float pitch = 1f)
    {
        if (IsInCooldown) return;
        StartCoroutine(CooldownSfx());
        if (AvailableSfxClipList.Count == 0) UpdateAvailableSfxList();

        int randomClipIndex = Random.Range(0, AvailableSfxClipList.Count - 1);
        AudioClip selectedSfx = AvailableSfxClipList[randomClipIndex];
        AvailableSfxClipList.Remove(selectedSfx);
        sfxPlayCallback?.Invoke(selectedSfx, false, pitch);
    }

    public IEnumerator CooldownSfx()
    {
        IsInCooldown = true;
        yield return _wait;
        IsInCooldown = false;
    }
}

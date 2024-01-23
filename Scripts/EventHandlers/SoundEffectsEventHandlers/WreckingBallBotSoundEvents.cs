using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WreckingBallBotSoundEvents : MonoBehaviour, IHasAudioSource, IContainsAudioClipArray, IExecuteWithDelay,
    ITrapHitSfx
{
    [SerializeField] private AudioClip[] _clipArray;
    [SerializeField] private float _delay = 0.4f;

    private AudioSource _audioSource;
    private SwingerTrap _swingerTrap;
    private WaitForSeconds _wait;
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

    public WaitForSeconds Wait
    {
        get => _wait;
        set => _wait = value;
    }

    public float Delay
    {
        get => _delay;
        set => _delay = value;
    }

    #endregion

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _swingerTrap = GetComponent<SwingerTrap>();
        _wait = new WaitForSeconds(_delay);
        UpdateAvailableAudioClipList();
    }

    private void OnEnable() => _swingerTrap.OnSwing += SwingerTrap_OnSwing;
    private void OnDisable() => _swingerTrap.OnSwing -= SwingerTrap_OnSwing;

    private void SwingerTrap_OnSwing() => ExecutePlaySfx();

    public void ExecutePlaySfx()
    {
        if (_audioSource == null) return;
        StopAllCoroutines();
        StartCoroutine(ExecuteWithDelay());
    }

    public IEnumerator ExecuteWithDelay()
    {
        yield return _wait;
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

    public void PlayHitSfx()
    {
        if (SoundEffectsManager.Instance == null) return;
        SoundEffectsManager.Instance.TrapHitPlayerSfx();
    }
}
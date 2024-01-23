using DG.Tweening;
using Leon;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISfxManager : SceneSingleton<UISfxManager>
{
    private AudioSource _audioSource;

    [SerializeField] private float soundEffectsVolume = 1;

    [BoxGroup("Generic Button SFX"), SerializeField] private AudioClip _buttonSFX;
    [BoxGroup("Back Button SFX"), SerializeField] private AudioClip _backButtonSFX;
    [BoxGroup("Toggle Button SFX"), SerializeField] private AudioClip _toggleOnSFX;
    [BoxGroup("Toggle Button SFX"), SerializeField] private AudioClip _toggleOffSFX;

    [BoxGroup("Slider SFX"), SerializeField] private AudioClip _sliderSFX;

    [BoxGroup("Buttons SFX"), SerializeField] private AudioClip _stageButtonSFX;
    [BoxGroup("Buttons SFX"), SerializeField] private AudioClip _levelSelectButtonSFX;

    [BoxGroup("Quit Popup SFX"), SerializeField] private AudioClip _closePopupButtonSFX;
    [BoxGroup("Buttons SFX"), SerializeField] private AudioClip _quitButtonSFX;

    #region Properties

    public AudioClip ButtonSFX { get => _buttonSFX; }
    public AudioClip BackButtonSFX { get => _backButtonSFX; }
    public AudioClip ToggleOnSFX { get => _toggleOnSFX; }
    public AudioClip StageButtonSFX { get => _stageButtonSFX; }
    public AudioClip LevelSelectButtonSFX { get => _levelSelectButtonSFX; }
    public AudioClip QuitButtonSFX { get => _quitButtonSFX; }

    #endregion

    protected override void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        _audioSource.volume = SoundEffectsManager.Instance.SoundEffectsVolume;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(audioClip);
    }      
    public void PlayMusicSliderSoundEffect(AudioClip audioClip)
    {
        _audioSource.volume = MusicManager.Instance.MusicVolume;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(audioClip);
    }    
}

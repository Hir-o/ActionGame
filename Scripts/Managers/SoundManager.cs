using System.Collections;
using System.Collections.Generic;
using Leon;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer _audioMixer;

    public AudioMixer AudioMixer => _audioMixer;

    private const string MIXER_MASTER_VOLUME = "MasterVolume";
    private const string MIXER_MUSIC_VOLUME = "MusicVolume";
    private const string MIXER_LEVEL_MUSIC_VOLUME = "LevelMusicVolume";
    private const string MIXER_SFX_VOLUME = "SFXVolume";

    #region Properties

    public float LevelMusicVolume
    {
        get
        {
            _audioMixer.GetFloat(MIXER_LEVEL_MUSIC_VOLUME, out float volume);
            return Mathf.Pow(10f, volume / 20f);
        }
    }

    #endregion

    private void OnEnable() => SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;

    private void SceneManager_OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) => UpdateMasterVolume(1f);

    public void UpdateMasterVolume(float volume) =>
        _audioMixer.SetFloat(MIXER_MASTER_VOLUME, Mathf.Log10(volume) * 20f);

    public void UpdateMusicVolume(float volume) =>
        _audioMixer.SetFloat(MIXER_MUSIC_VOLUME, Mathf.Log10(volume) * 20f);

    public void UpdateLevelMusicVolume(float volume) =>
        _audioMixer.SetFloat(MIXER_LEVEL_MUSIC_VOLUME, Mathf.Log10(volume) * 20f);

    public void UpdateSoundEffectsVolume(float volume) =>
        _audioMixer.SetFloat(MIXER_SFX_VOLUME, Mathf.Log10(volume) * 20f);
}
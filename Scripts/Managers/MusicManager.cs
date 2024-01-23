using Leon;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : SceneSingleton<MusicManager>
{
    private AudioSource _musicAudioSource;

    [SerializeField] private float musicVolume;

    [SerializeField] private AudioClip _levelMusicClip;
    [SerializeField] private AudioClip _menuMusicClip;
    [SerializeField] private AudioClip _bossLevelMusicClip;

    [BoxGroup("Delay"), SerializeField] private float _musicStopOnLevelCompleteDelay = 2.5f;

    private Coroutine _coroutine;
    private bool _isMusicMuted;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            SoundManager.Instance.UpdateMusicVolume(musicVolume);
            _isMusicMuted = musicVolume <= 0.0001;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _musicAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
    }

    private void OnDisable()
    {
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
        SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;
        StopAllCoroutines();
    }

    private void LevelFinish_OnAnyLevelCompleted()
    {
        _coroutine = StartCoroutine(StopMusicCR(_musicStopOnLevelCompleteDelay));
    }

    private void SceneManager_OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SoundManager.Instance.UpdateLevelMusicVolume(0.0001f);
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currSceneIndex == 0)
        {
            StopMusic();
        }
        else
        {
            PlayMusic(_levelMusicClip);
            _coroutine = StartCoroutine(PlayMusicCR());
        }
    }

    private void PlayMusic(AudioClip musicClip)
    {
        if (_musicAudioSource.clip == musicClip) return;
        _musicAudioSource.clip = musicClip;
        _musicAudioSource.Play();
    }

    private void StopMusic()
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = null;
    }

    public void StopMusicAfterDeath()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(StopMusicCR(0f));
    }

    public void PlayMusicOnRespawn()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        //_coroutine = StartCoroutine(PlayMusicCR());
        SoundManager.Instance.UpdateLevelMusicVolume(1f);
    }

    private IEnumerator PlayMusicCR()
    {
        if (_isMusicMuted) yield break;
        while (SoundManager.Instance.LevelMusicVolume < 1f)
        {
            SoundManager.Instance.UpdateLevelMusicVolume(SoundManager.Instance.LevelMusicVolume + .05f);
            if (SoundManager.Instance.LevelMusicVolume > 1f)
                SoundManager.Instance.UpdateLevelMusicVolume(1f);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }

    private IEnumerator StopMusicCR(float stopMusicDelay)
    {
        if (_isMusicMuted) yield break;
        if (stopMusicDelay > 0f) yield return new WaitForSeconds(stopMusicDelay);
        while (SoundManager.Instance.LevelMusicVolume > 0.0001f)
        {
            SoundManager.Instance.UpdateLevelMusicVolume(SoundManager.Instance.LevelMusicVolume - .1f);
            if (SoundManager.Instance.LevelMusicVolume < 0.0001f)
                SoundManager.Instance.UpdateLevelMusicVolume(0.0001f);
            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
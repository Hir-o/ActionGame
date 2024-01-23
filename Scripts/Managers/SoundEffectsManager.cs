using DG.Tweening;
using Leon;
using UnityEngine;

public class SoundEffectsManager : SceneSingleton<SoundEffectsManager>
{
    private AudioSource _soundEffectAudioSource;

    [SerializeField] private float soundEffectsVolume = 1;

    private Tween _sfxCooldownTween;

    private ISoundEffectEvent[] _soundEffectEventArray;

    #region Properties

    public float SoundEffectsVolume
    {
        get => soundEffectsVolume;
        set
        {
            soundEffectsVolume = value;
            SoundManager.Instance.UpdateSoundEffectsVolume(soundEffectsVolume);
        }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _soundEffectAudioSource = GetComponent<AudioSource>();
        _soundEffectEventArray = GetComponentsInChildren<ISoundEffectEvent>();
    }

    private void OnEnable()
    {
        Checkpoint.OnNewCheckpointReached += Checkpoint_OnNewCheckpointReached;
    }

    private void OnDisable()
    {
        Checkpoint.OnNewCheckpointReached -= Checkpoint_OnNewCheckpointReached;
    }

    public void PlayFootstepSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IRunSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    private void CharacterPlayerController_OnPlayerDied()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IDeathSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayJumpSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IJumpSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayCoinCollectSfx(float pitch)
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is ICoinCollectSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect, pitch);
    }

    public void PlayGemCollectSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IGemCollectSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayJumpPadSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IJumpadSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayLedgeGrabSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is ILedgeGrabSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayEnemyDestructSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IEnemyExplosionSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayRocketExplosionSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IRocketExplosionSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    private void Checkpoint_OnNewCheckpointReached(int arg1, Vector3 arg2)
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is ICheckpointSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayLoseSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is ILoseSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void PlayWinSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IWinSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    public void TrapHitPlayerSfx()
    {
        for (int i = 0; i < _soundEffectEventArray.Length; i++)
            if (_soundEffectEventArray[i] is IPlayerHitSfxEvent sfxEvent)
                sfxEvent.Execute(PlaySoundEffect);
    }

    private void PlaySoundEffectOnState(AudioClip sfxClip, bool state = false)
    {
        if (_soundEffectAudioSource.clip != sfxClip) PlaySoundEffect(sfxClip, state);
    }

    private void PlaySoundEffect(AudioClip sfxClip, bool state = false, float pitch = 1f)
    {
        _soundEffectAudioSource.loop = state;
        _soundEffectAudioSource.pitch = pitch;
        _soundEffectAudioSource.PlayOneShot(sfxClip);
    }

    private void PlaySoundEffectWithPitch(AudioClip sfxClip, float pitch = 1f)
    {
        _soundEffectAudioSource.pitch = pitch;
        _soundEffectAudioSource.PlayOneShot(sfxClip);
    }
}
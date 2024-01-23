using UnityEngine;

public class PlayerSoundEvents : MonoBehaviour
{
    [SerializeField] private AudioClip _slidingAudioClip;

    private AudioSource _audioSource;
    private MovementController _movementController;
    private bool _isSoundEffectsManagerActive;

    private bool _isPlayingSlidingSfx;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() => _isSoundEffectsManagerActive = SoundEffectsManager.Instance != null;

    private void OnEnable()
    {
        _movementController.OnPlayLedgeClimbSfx += MovementController_OnPlayLedgeClimbSfx;
        _movementController.OnPlayJumpSfx += MovementController_OnPlayJumpSfx;
        MovementController.OnPlayerSlide += MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel += MovementController_OnPlayerSlideCancel;
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnDisable()
    {
        _movementController.OnPlayLedgeClimbSfx -= MovementController_OnPlayLedgeClimbSfx;
        _movementController.OnPlayJumpSfx -= MovementController_OnPlayJumpSfx;
        MovementController.OnPlayerSlide -= MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel -= MovementController_OnPlayerSlideCancel;
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
    }

    private void MovementController_OnPlayLedgeClimbSfx() => OnPlayLedgeGrabSfx();
    private void MovementController_OnPlayJumpSfx() => OnPlayJumpSfx();
    private void MovementController_OnPlayerSlide() => OnPlaySlideSfx();
    private void MovementController_OnPlayerSlideCancel(bool isMovementPressed) => OnStopSlideSfx();

    private void CharacterPlayerController_OnPlayerDied()
    {
        if (MusicManager.Instance == null) return;
        MusicManager.Instance.StopMusicAfterDeath();
        if (!_isSoundEffectsManagerActive) return;
        SoundEffectsManager.Instance.PlayLoseSfx();
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        if (!_isSoundEffectsManagerActive) return;
        if (MusicManager.Instance == null) return;
        MusicManager.Instance.PlayMusicOnRespawn();
    }

    private void OnPlayJumpSfx()
    {
        if (!_isSoundEffectsManagerActive) return;
        SoundEffectsManager.Instance.PlayJumpSfx();
    }

    private void OnPlayFootstepSfx()
    {
        if (!_isSoundEffectsManagerActive) return;
        SoundEffectsManager.Instance.PlayFootstepSfx();
    }

    private void OnPlayLedgeGrabSfx()
    {
        if (!_isSoundEffectsManagerActive) return;
        SoundEffectsManager.Instance.PlayLedgeGrabSfx();
    }

    public void OnPlaySlideSfx()
    {
        if (_isPlayingSlidingSfx) return;
        if (_audioSource == null) return; 
        _isPlayingSlidingSfx = true;
        _audioSource.clip = _slidingAudioClip;
        _audioSource.Play();
    }

    public void OnStopSlideSfx()
    {
        if (_isPlayingSlidingSfx == false) return;
        if (_audioSource == null) return; 
        _isPlayingSlidingSfx = false;
        _audioSource.Stop();
        _audioSource.clip = null;
    }
}
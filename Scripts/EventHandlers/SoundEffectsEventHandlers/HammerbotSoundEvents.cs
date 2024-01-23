using UnityEngine;

public class HammerbotSoundEvents : MonoBehaviour, IHasAudioSource
{
    [SerializeField] private AudioSource _audioSource;

    private HammerBot _hammerBot;

    public AudioSource AudioSource
    {
        get => _audioSource;
        set => _audioSource = value;
    }
    
    private void Awake()
    {
        _hammerBot = GetComponent<HammerBot>();
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() => _hammerBot.OnHammerHitGround += HammerBot_OnHammerHitGround;
    private void OnDisable() => _hammerBot.OnHammerHitGround -= HammerBot_OnHammerHitGround;

    private void HammerBot_OnHammerHitGround() => ExecutePlaySfx();
    
    public void ExecutePlaySfx()
    {
        if (AudioSource == null) return;
        AudioSource.Play();
    }
}
using System;
using Leon;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using DG.Tweening;

public class CoinsManager : SceneSingleton<CoinsManager>
{
    public event Action OnFinishCollectCoinSequence;

    private AudioSource coinCollectSfx;

    private int coinCollectCounter;

    [SerializeField] private int _coinSequenceStart = 0;
    [SerializeField] private int _coinSequenceCount = 10;

    [Range(-3, 3), SerializeField] private float _coinPitchStart = 0.5f;
    [Range(-3, 3), SerializeField] private float _coinPitchCount = 2.5f;

    [SerializeField] private float coinCollectCountTimer;

    [SerializeField] private AnimationCurve coinCollectPitchCurve;

    [BoxGroup("Rotation"), SerializeField] private float _rotationSpeed = .25f;

    [SerializeField] private List<Coin> _coinList = new List<Coin>();

    private Coroutine _coinCoroutine;
    private Tween _sfxCooldownTween;
    private SoundEffectsManager _soundEffectsManager;
    private int _totalLevelCoinsAmount;
    private int _totalSpecialCoinsAmount;
    private int _collectedCoins;
    private bool _isSoundEffectsManagerPresent;

    #region Properties

    public int TotalLevelCoinsAmount => _totalLevelCoinsAmount;
    public int TotalSpecialCoinsAmount => _totalSpecialCoinsAmount;
    public List<Coin> CoinList => _coinList;

    #endregion

    protected override void Awake() => coinCollectSfx = GetComponent<AudioSource>();

    private void Start()
    {
        _soundEffectsManager = SoundEffectsManager.Instance;
        _isSoundEffectsManagerPresent = _soundEffectsManager != null;
    }

    private void OnEnable()
    {
        Coin.OnCollectCoin += Coin_OnCollectCoin;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        Checkpoint.OnNewCheckpointReached += Checkpoint_OnNewCheckpointReached;
        KeyFrameSelection();
    }

    private void OnDisable()
    {
        Coin.OnCollectCoin -= Coin_OnCollectCoin;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        Checkpoint.OnNewCheckpointReached -= Checkpoint_OnNewCheckpointReached;
    }

    private void Update()
    {
        for (int i = 0; i < CoinList.Count; i++)
        {
            if (!CoinList[i].IsVisible) continue;
            Vector3 currRotation = CoinList[i].transform.localRotation.eulerAngles;
            currRotation.y += _rotationSpeed * Time.deltaTime;
            CoinList[i].transform.localRotation = Quaternion.Euler(currRotation);
        }
    }

    private void Coin_OnCollectCoin()
    {
        _collectedCoins++;
        PlaySound();
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        PlayerStats.Instance.ReduceCollectedCoinsAmount(_collectedCoins);
        _collectedCoins = 0;
    }

    private void Checkpoint_OnNewCheckpointReached(int checkpointIndex, Vector3 position) => _collectedCoins = 0;

    private void PlaySound()
    {
        if (_sfxCooldownTween != null) return;
        if (_coinCoroutine != null) StopCoroutine(_coinCoroutine);
        _coinCoroutine = StartCoroutine(CoinCounterCoroutine());
        if (_isSoundEffectsManagerPresent)
        {
            float pitch = coinCollectPitchCurve.Evaluate(coinCollectCounter);
            //reset pitch
            pitch = 1f;
            SoundEffectsManager.Instance.PlayCoinCollectSfx(pitch);
            _sfxCooldownTween = DOVirtual.Float(1f, 0f, .075f, value => { }).OnComplete(() => _sfxCooldownTween = null);
        }
    }

    private void KeyFrameSelection()
    {
        coinCollectPitchCurve =
            AnimationCurve.EaseInOut(_coinSequenceStart, _coinPitchStart, _coinSequenceCount, _coinPitchCount);
    }

    private IEnumerator CoinCounterCoroutine()
    {
        coinCollectCounter++;
        yield return new WaitForSeconds(coinCollectCountTimer);
        coinCollectCounter = 0;
    }

    public void AddToTotalCoinAmount() => _totalLevelCoinsAmount++;
    public void AddToTotalSpecialCoinAmount() => _totalSpecialCoinsAmount++;
}
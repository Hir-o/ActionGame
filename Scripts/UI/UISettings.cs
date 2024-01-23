
using DG.Tweening;
using Lean.Gui;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : SceneSingleton<UISettings>
{
    [BoxGroup("Images"), SerializeField] private Image _soundFxImage;
    [BoxGroup("Images"), SerializeField] private Image _musicImage;
    [BoxGroup("Images"), SerializeField] private Image _batterySavingImage;
    [BoxGroup("Images"), SerializeField] private Image _vibrationImage;

    [BoxGroup("Toggle Images"), SerializeField]
    private Image _batterySavingHandleImage;

    [BoxGroup("Toggle Images"), SerializeField]
    private Image _batterySavingPanelImage;

    [BoxGroup("Toggle Images"), SerializeField]
    private Image _vibrationHandleImage;

    [BoxGroup("Toggle Images"), SerializeField]
    private Image _vibrationPanelImage;

    [BoxGroup("Toggle Buttons"), SerializeField]
    private LeanToggle _batterySavingToggle;

    [BoxGroup("Toggle Buttons"), SerializeField]
    private LeanToggle _vibrationToggle;

    [BoxGroup("Sliders"), SerializeField] private Slider _soundFxSlider;
    [BoxGroup("Sliders"), SerializeField] private Slider _musicSlider;

    [BoxGroup("Sprites"), SerializeField] private Sprite _soundFxOnSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _soundFxOffSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _musicOnSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _musicOffSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _batterySavingOnSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _batterySavingOffSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _vibrationOnSprite;
    [BoxGroup("Sprites"), SerializeField] private Sprite _vibrationOffSprite;

    [BoxGroup("Toggle Sprites"), SerializeField]
    private Sprite _handleOnSprite;

    [BoxGroup("Toggle Sprites"), SerializeField]
    private Sprite _handleOffSprite;

    [BoxGroup("Toggle Sprites"), SerializeField]
    private Sprite _panelOnSprite;

    [BoxGroup("Toggle Sprites"), SerializeField]
    private Sprite _panelOffSprite;

    [BoxGroup("Tweening"), SerializeField] private float _fadeSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fadeEase = Ease.OutQuad;

    [BoxGroup("Buttons"), SerializeField] private Button _backButton;
    [BoxGroup("Buttons"), SerializeField] private Button _creditsButton;

    [BoxGroup("Colors"), SerializeField] private Color _enabledColor;
    [BoxGroup("Colors"), SerializeField] private Color _disabledColor;

    private CanvasGroup _canvasGroup;

    private float _soundFxVolume = 1f;
    private float _musicVolume = 1f;
    private bool _isBatterySavingEnabled;
    private bool _isVibratingEnabled = true;

    #region Properties

    public float SoundFxVolume
    {
        get => _soundFxVolume;
        private set
        {
            _soundFxVolume = value;
            _soundFxImage.sprite = _soundFxVolume > 0f ? _soundFxOnSprite : _soundFxOffSprite;
            _soundFxImage.color = _soundFxVolume > 0f ? _enabledColor : _disabledColor;
            PlayerPrefs.SetFloat(PlayerPrefsConstants.SoundFxVolume, _soundFxVolume);
            PlayerPrefs.Save();
        }
    }

    public float MusicVolume
    {
        get => _musicVolume;
        private set
        {
            _musicVolume = value;
            _musicImage.sprite = _musicVolume > 0f ? _musicOnSprite : _musicOffSprite;
            _musicImage.color = _musicVolume > 0f ? _enabledColor : _disabledColor;
            PlayerPrefs.SetFloat(PlayerPrefsConstants.MusicVolume, _musicVolume);
            PlayerPrefs.Save();
        }
    }

    public bool IsBatterySavingEnabled
    {
        get => _isBatterySavingEnabled;
        private set
        {
            _isBatterySavingEnabled = value;
            PlayerPrefs.SetInt(PlayerPrefsConstants.BatterySaving, _isBatterySavingEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool IsVibratingEnabled
    {
        get => _isVibratingEnabled;
        private set
        {
            _isVibratingEnabled = value;
            PlayerPrefs.SetInt(PlayerPrefsConstants.Vibration, _isVibratingEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        LoadData();
        UpdateAllSettings();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _soundFxSlider.onValueChanged.AddListener(OnSoundFxSliderValueChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _backButton.onClick.AddListener(OnClickBackButton);
        _creditsButton.onClick.AddListener(OnClickCreditsButton);
    }

    private void OnClickBackButton() => FadeOut();

    public void FadeIn()
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }

    public void FadeOut(bool isBackButtonClicked = true)
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase)
            .OnStart(() =>
            {
                if (isBackButtonClicked)
                    UIMenu.Instance.FadeIn();
            })
            .OnComplete(() =>
            {
                if (!isBackButtonClicked)
                    UICredits.Instance.FadeIn();
            });
    }

    private void UpdateAllSettings()
    {
        //_soundFxSlider.value = _soundFxVolume;
        _musicSlider.value = _musicVolume;
        _soundFxImage.sprite = _soundFxVolume > 0f ? _soundFxOnSprite : _soundFxOffSprite;
        _soundFxImage.color = _soundFxVolume > 0f ? _enabledColor : _disabledColor;
        _musicImage.sprite = _musicVolume > 0f ? _musicOnSprite : _musicOffSprite;
        _musicImage.color = _musicVolume > 0f ? _enabledColor : _disabledColor;

        if (!_isBatterySavingEnabled)
        {
            _batterySavingToggle.Toggle();
        }
        else
        {
            _batterySavingToggle.Toggle();
            _batterySavingToggle.Toggle();
        }

        if (!_isVibratingEnabled)
        {
            _vibrationToggle.Toggle();
        }
        else
        {
            _vibrationToggle.Toggle();
            _vibrationToggle.Toggle();
        }
    }

    private void OnSoundFxSliderValueChanged(float amount)
    {
        SoundFxVolume = amount;
        SoundManager.Instance.UpdateSoundEffectsVolume(SoundFxVolume);
    }

    private void OnMusicSliderValueChanged(float amount)
    {
        MusicVolume = amount;
        SoundManager.Instance.UpdateMusicVolume(MusicVolume);
    }

    public void OnBatterySavingToggleOn()
    {
        _batterySavingImage.sprite = _batterySavingOnSprite;
        _batterySavingImage.color = _enabledColor;
        _batterySavingHandleImage.sprite = _handleOnSprite;
        _batterySavingPanelImage.sprite = _panelOnSprite;
        if (!IsBatterySavingEnabled) IsBatterySavingEnabled = true;
    }

    public void OnBatterySavingToggleOff()
    {
        _batterySavingImage.sprite = _batterySavingOffSprite;
        _batterySavingImage.color = _disabledColor;
        _batterySavingHandleImage.sprite = _batterySavingOffSprite;
        _batterySavingHandleImage.sprite = _handleOffSprite;
        _batterySavingPanelImage.sprite = _panelOffSprite;
        if (IsBatterySavingEnabled) IsBatterySavingEnabled = false;
    }

    public void OnVibrationToggleOn()
    {
        _vibrationImage.sprite = _vibrationOnSprite;
        _vibrationImage.color = _enabledColor;
        _vibrationHandleImage.sprite = _handleOnSprite;
        _vibrationPanelImage.sprite = _panelOnSprite;
        if (!IsVibratingEnabled) IsVibratingEnabled = true;
    }

    public void OnVibrationToggleOff()
    {
        _vibrationImage.sprite = _vibrationOffSprite;
        _vibrationImage.color = _disabledColor;
        _vibrationHandleImage.sprite = _handleOffSprite;
        _vibrationPanelImage.sprite = _panelOffSprite;
        if (IsVibratingEnabled) IsVibratingEnabled = false;
    }

    private void OnClickCreditsButton() => FadeOut(false);

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.SoundFxVolume))
        {
            _soundFxVolume = PlayerPrefs.GetFloat(PlayerPrefsConstants.SoundFxVolume);
            SoundManager.Instance.UpdateSoundEffectsVolume(SoundFxVolume);
        }

        if (PlayerPrefs.HasKey(PlayerPrefsConstants.MusicVolume))
        {
            _musicVolume = PlayerPrefs.GetFloat(PlayerPrefsConstants.MusicVolume);
            SoundManager.Instance.UpdateMusicVolume(MusicVolume);
        }

        if (PlayerPrefs.HasKey(PlayerPrefsConstants.BatterySaving))
            _isBatterySavingEnabled = PlayerPrefs.GetInt(PlayerPrefsConstants.BatterySaving) == 1;

        if (PlayerPrefs.HasKey(PlayerPrefsConstants.Vibration))
            _isVibratingEnabled = PlayerPrefs.GetInt(PlayerPrefsConstants.Vibration) == 1;
    }
}
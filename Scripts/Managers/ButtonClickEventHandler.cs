using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leon;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;
using Lean.Gui;

public class ButtonClickEventHandler : SceneSingleton<ButtonClickEventHandler>
{
    [BoxGroup("UI Menu"), SerializeField] private Button _playButton;
    [BoxGroup("UI Menu"), SerializeField] private Button _settingsButton;
    [BoxGroup("UI Menu"), SerializeField] private Button _achievementsButton;
    [BoxGroup("UI Menu"), SerializeField] private Button _collectablesButton;

    [BoxGroup("UI Settings"), SerializeField] private Slider _soundFxSlider;
    [BoxGroup("UI Settings"), SerializeField] private Slider _musicSlider;

    [BoxGroup("UI Settings"), SerializeField] private LeanButton _batterySavingToggleButton;
    [BoxGroup("UI Settings"), SerializeField] private LeanButton _vibrationToggleButton;

    [BoxGroup("UI Settings"), SerializeField] private Button _creditsButton;

    [BoxGroup("UI Reset Progress"), SerializeField] private Button _resetProgressPopupButton;
    [BoxGroup("UI Reset Progress"), SerializeField] private Button _resetButton;
    [BoxGroup("UI Reset Progress"), SerializeField] private Button _closeButton;

    [BoxGroup("UI Back Buttons"), SerializeField] private Button[] _backButtons;

    //private Tween _virtualSFXTween;
    //private Tween _virtualMusicTween;

    //private void Start()
    //{
    //    _playButton.onClick.AddListener(OnPlayButtonClick);
    //    _settingsButton.onClick.AddListener(OnSettingsButtonClick);   
    //    _achievementsButton.onClick.AddListener(OnAchievementsButtonClick);   
    //    _collectablesButton.onClick.AddListener(OnCollectablesButtonClick);

    //    _soundFxSlider.onValueChanged.AddListener(OnSoundFXSliderValueChanged);
    //    _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);

    //    _creditsButton.onClick.AddListener(OnCreditsButtonClick);

    //    //_batterySavingToggleButton.OnClick.AddListener(OnBatterySavingToggleButtonClick);
    //    //_vibrationToggleButton.OnClick.AddListener(OnVibrationToggleButtonClick);

    //    //_resetProgressPopupButton.onClick.AddListener(OnResetProgressButtonClick);
    //    //_resetButton.onClick.AddListener(OnReseButtonClick);
    //    //_closeButton.onClick.AddListener(OnCloseButtonClick);

    //    for (int i = 0; i < _backButtons.Length; i++)
    //    {
    //        _backButtons[i].onClick.AddListener(OnBackButtonClick);
    //    }
    //}

    //public void OnPlayButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnSettingsButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnAchievementsButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnCollectablesButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnCreditsButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnBatterySavingToggleButtonClick() => UISfxManager.Instance.ToggleOnSFX();
    //public void OnVibrationToggleButtonClick() => UISfxManager.Instance.ToggleOnSFX();

    //public void OnResetProgressButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnReseButtonClick() => UISfxManager.Instance.ButtonSFX();
    //public void OnCloseButtonClick() => UISfxManager.Instance.BackButtonSFX();

    //public void OnBackButtonClick() => UISfxManager.Instance.BackButtonSFX();


    //public void OnSoundFXSliderValueChanged(float amount)
    //{
    //    _virtualSFXTween.Kill();
    //    _virtualSFXTween = _virtualSFXTween != null ? 
    //        DOVirtual.Float(1f, 0f, 0.2f, value => { }).OnComplete(() => UISfxManager.Instance.SliderSFX()) : DOVirtual.Float(1f, 0f, 0.2f, value => { });
    //}
    //public void OnMusicSliderValueChanged(float amount)
    //{
    //    _virtualMusicTween.Kill();
    //    _virtualMusicTween = _virtualMusicTween != null ? 
    //        DOVirtual.Float(1f, 0f, 0.2f, value => { }).OnComplete(() => UISfxManager.Instance.SliderSFX()) : DOVirtual.Float(1f, 0f, 0.2f, value => { });
    //}
}


using NaughtyAttributes;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [BoxGroup("FPS Limit"), SerializeField]
    private int _defaultTargetFramerate = 60;

    [BoxGroup("FPS Limit"), SerializeField]
    private int _batterySavingTargetFramerate = 30;

    [BoxGroup("Resolution Scale Reduction"), SerializeField]
    private bool _enableResolutionReduction = false;

    [BoxGroup("Resolution Scale Reduction"), SerializeField]
    private float _minScreenWidth = 720f;

    [BoxGroup("Resolution Scale Reduction"), SerializeField]
    private float _resolutionScaleDivideAmount = 1.5f;

    private int _targetFramerate;

    private void Awake()
    {
        _targetFramerate = _defaultTargetFramerate;
        QualitySettings.vSyncCount = 0;
        LoadData();
        UpdateResolutionScale();
    }

    private void UpdateResolutionScale()
    {
        if (Screen.width > _minScreenWidth && _enableResolutionReduction)
        {
            float halfScreenHeight = Screen.height / _resolutionScaleDivideAmount;
            float halfScreenWidth = Screen.width / _resolutionScaleDivideAmount;
            Screen.SetResolution((int)halfScreenWidth, (int)halfScreenHeight, true, 60);
        }
    }

    private void Start() => Application.targetFrameRate = _targetFramerate;

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.BatterySaving))
            if (PlayerPrefs.GetInt(PlayerPrefsConstants.BatterySaving) == 1)
                _targetFramerate = _batterySavingTargetFramerate;
    }
}
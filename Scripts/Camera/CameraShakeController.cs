using Leon;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraShakeController : SceneSingleton<CameraShakeController>
{
    public CameraShakeProfileData[] _cameraShakeProfileDataArray;

    [SerializeField] private float _dynamicFrequencyDivider = 20f;
    [SerializeField] private float _maxDynamicFrequencyGain = 0.8f;

    private CinemachineVirtualCamera[] _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin[] _cinemachineBasicMultiChannelPerlin;

    private Tween _frequencyShakeDurationTween;
    private Tween _amplitudeShakeDurationTween;

    private void Start()
    {
        _cinemachineVirtualCamera = 
            new CinemachineVirtualCamera[CameraManager.Instance.CameraProfileArray.Length];

        for (int i = 0; i < CameraManager.Instance.CameraProfileArray.Length; i++)
        {
            _cinemachineVirtualCamera[i] = CameraManager.Instance.CameraProfileArray[i].VirtualCamera;
        }

        _cinemachineBasicMultiChannelPerlin = 
            new CinemachineBasicMultiChannelPerlin[_cinemachineVirtualCamera.Length];
        for (int i = 0; i < _cinemachineVirtualCamera.Length; i++)
        {
            _cinemachineBasicMultiChannelPerlin[i] = 
                _cinemachineVirtualCamera[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void ShakeCamera(ShakeType shakeType, bool sliding = false)
    {
        for (int i = 0; i < _cameraShakeProfileDataArray.Length; i++)
        {
            if (_cameraShakeProfileDataArray[i].ShakeType == shakeType)
            {
                for (int y = 0; y < _cinemachineVirtualCamera.Length; y++)
                {
                    if (CinemachineCore.Instance.IsLive(_cinemachineVirtualCamera[y]))
                    {
                        if (_cameraShakeProfileDataArray[i].Dynamic)
                            InitializeDynamicShake(_cameraShakeProfileDataArray[i], y, sliding);
                        else
                            InitializeShake(_cameraShakeProfileDataArray[i], y); 
                        break;
                    }
                }
                break;
            }
        }
    }

    private void InitializeShake(CameraShakeProfileData cameraShakeData, int cameraIndex)
    {
        return;
        _cinemachineBasicMultiChannelPerlin[cameraIndex].m_AmplitudeGain = cameraShakeData.Amplitude;
        _cinemachineBasicMultiChannelPerlin[cameraIndex].m_FrequencyGain = cameraShakeData.Frequency;

        EndCameraShake(cameraShakeData, cameraIndex);
    }

    private void InitializeDynamicShake(CameraShakeProfileData cameraShakeData, int cameraIndex, bool sliding)
    {
        if (sliding)
        {
            _cinemachineBasicMultiChannelPerlin[cameraIndex].m_AmplitudeGain = cameraShakeData.Amplitude;

            if (MovementController.Instance.CurrSpeed > MovementController.Instance.MovementSpeed)
            {
                if ((MovementController.Instance.CurrSpeed / _dynamicFrequencyDivider) < _maxDynamicFrequencyGain)
                    _cinemachineBasicMultiChannelPerlin[cameraIndex].m_FrequencyGain = MovementController.Instance.CurrSpeed / _dynamicFrequencyDivider;
                else
                    _cinemachineBasicMultiChannelPerlin[cameraIndex].m_FrequencyGain = _maxDynamicFrequencyGain;
            }
        }
        else
            EndCameraShake(cameraShakeData, cameraIndex);
    }

    private void EndCameraShake(CameraShakeProfileData cameraShakeData, int cameraIndex)
    {
        if (_frequencyShakeDurationTween != null) _frequencyShakeDurationTween.Kill();
        _frequencyShakeDurationTween = DOTween.To(() => _cinemachineBasicMultiChannelPerlin[cameraIndex].m_FrequencyGain,
            x => _cinemachineBasicMultiChannelPerlin[cameraIndex].m_FrequencyGain = x, 0, cameraShakeData.Duration)
            .SetEase(cameraShakeData.AmplitudeEase);

        if (_amplitudeShakeDurationTween != null) _amplitudeShakeDurationTween.Kill();
        _amplitudeShakeDurationTween = DOTween.To(() => _cinemachineBasicMultiChannelPerlin[cameraIndex].m_AmplitudeGain,
            x => _cinemachineBasicMultiChannelPerlin[cameraIndex].m_AmplitudeGain = x, 0, cameraShakeData.Duration)
            .SetEase(cameraShakeData.AmplitudeEase);
    }
}

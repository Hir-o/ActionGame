using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using ShakeRandomnessMode = DG.Tweening.ShakeRandomnessMode;

[CreateAssetMenu(fileName = "CameraShakeProfileData", menuName = "CameraShakeProfile/CameraShake", order = 0)]
public class CameraShakeProfileData : ScriptableObject 
{
    [BoxGroup("CameraShake"), SerializeField] private bool _dynamic;
    [BoxGroup("CameraShake"), SerializeField] private float _frequency;
    [BoxGroup("CameraShake"), SerializeField] private float _amplitude;
    [BoxGroup("CameraShake"), SerializeField] private float _duration;
    [BoxGroup("CameraShake"), SerializeField] private Ease _amplitudeEase;
    [BoxGroup("CameraShake"), SerializeField] private ShakeType _shakeType;

    public bool Dynamic => _dynamic;
    public float Frequency => _frequency; 
    public float Amplitude => _amplitude; 
    public float Duration => _duration;
    public Ease AmplitudeEase => _amplitudeEase; 
    public ShakeType ShakeType => _shakeType;

}

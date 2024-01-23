using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelButtonTweenerData", menuName = "LevelButtonTweenerData/LevelSelectButtonTweener", order = 1)]
public class LevelButtonTweenerData : ScriptableObject
{
    [BoxGroup("Size"), SerializeField] private Vector2 _initSizeDelta = new Vector2(350f, 350f);
    [BoxGroup("Size"), SerializeField] private Vector2 _finalSizeDelta = new Vector2(250f, 250f);
    [BoxGroup("Size"), SerializeField] private float _sizeDuration = 1f;
    [BoxGroup("Size"), SerializeField] private float _sizeDelay = 0.15f;

    [BoxGroup("Fading"), SerializeField] private float _fadeDuration = 1f;
    [BoxGroup("Fading"), SerializeField] private float _fadeDelay = 0.15f;
    [BoxGroup("Fading"), SerializeField] private Ease _fadeEase = Ease.OutCubic;

    public Vector2 InitSizeDelta => _initSizeDelta;
    public Vector2 FinalSizeDelta => _finalSizeDelta;
    public float SizeDuration => _sizeDuration;
    public float SizeDelay => _sizeDelay;
    public float FadeDuration => _fadeDuration;
    public float FadeDelay => _fadeDelay;
    public Ease FadeEase => _fadeEase;
}
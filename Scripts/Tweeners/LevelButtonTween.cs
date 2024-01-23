
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class LevelButtonTween : MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Tween _sizeTween;
    private Tween _fadeTween;

    [BoxGroup("Data"), SerializeField] private LevelButtonTweenerData _tweenData;

    private void Awake() => AssingComponents();

    private void AssingComponents()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void PlayAnimation(int currIndex)
    {
        KillTweens();
        float delay = currIndex * _tweenData.SizeDelay;
        float fadeDelay = currIndex * _tweenData.FadeDelay;
        _sizeTween = _rectTransform.DOSizeDelta(_tweenData.FinalSizeDelta, _tweenData.SizeDuration)
            .SetEase(EasePresets.Instance.BounceOnceEaseVeryLight).SetDelay(delay);
        _fadeTween = _canvasGroup.DOFade(1f, _tweenData.FadeDuration).SetEase(_tweenData.FadeEase).SetDelay(fadeDelay);
    }

    public void PlayClickAnimation()
    {
        
    }

    public void Reset()
    {
        AssingComponents();
        KillTweens();
        _rectTransform.sizeDelta = _tweenData.InitSizeDelta;
        _canvasGroup.alpha = 0f;
    }

    private void KillTweens()
    {
        if (_sizeTween != null && _sizeTween.IsPlaying()) _sizeTween.Kill();
        if (_fadeTween != null && _fadeTween.IsPlaying()) _fadeTween.Kill();
    }
}
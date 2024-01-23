
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class StageSelectUITweener : MonoBehaviour
{
    [BoxGroup("Tweening"), SerializeField] private float _duration = 0.5f;
    [BoxGroup("Tweening"), SerializeField] private float _initYPos = 120f;
    [BoxGroup("Tweening"), SerializeField] private float _finalYPos = 120f;
    [BoxGroup("Tweening"), SerializeField] private float _delay = 1f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease;

    private RectTransform _rectTransform;
    private Tween _moveTween;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _initYPos);
    }

    public void PlayAnimation()
    {
        if (_moveTween != null) _moveTween.Kill();
        if (_ease == Ease.Unset)
            _moveTween = _rectTransform.DOAnchorPosY(_finalYPos, _duration)
                .SetEase(EasePresets.Instance.BounceOnceEaseVeryLight).SetDelay(_delay);
        else
            _moveTween = _rectTransform.DOAnchorPosY(_finalYPos, _duration).SetEase(_ease).SetDelay(_delay);
    }

    public void Reset()
    {
        if (_moveTween != null) _moveTween.Kill();
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _initYPos);
    }
}
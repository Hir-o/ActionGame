
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PauseButtonTweener : MonoBehaviour
{
    [BoxGroup("Tweening"), SerializeField] private float _duration = 0.5f;
    [BoxGroup("Tweening"), SerializeField] private float _initYPos = 110f;
    [BoxGroup("Tweening"), SerializeField] private float _finalYPos = -40f;
    [BoxGroup("Tweening"), SerializeField] private float _delay = 1f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease;

    private RectTransform _rectTransform;
    private Tween _moveTween;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _initYPos);
    }

    private void Start()
    {
        if (_moveTween != null && _moveTween.IsPlaying()) _moveTween.Kill();
        _moveTween = _rectTransform.DOAnchorPosY(_finalYPos, _duration).SetEase(_ease).SetDelay(_delay);
    }
}
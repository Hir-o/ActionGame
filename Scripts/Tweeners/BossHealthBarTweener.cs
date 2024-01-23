
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarTweener : MonoBehaviour
{
    [BoxGroup("Tweening"), SerializeField] private float _duration = 0.5f;
    [BoxGroup("Tweening"), SerializeField] private float _initYPos = 110f;
    [BoxGroup("Tweening"), SerializeField] private float _finalYPos = -40f;
    [BoxGroup("Tweening"), SerializeField] private float _delay = 1f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease;

    [BoxGroup("Fill Image"), SerializeField]
    private Image _fillAmountImage;

    [BoxGroup("Fill Image"), SerializeField]
    private float _fillDuration = .75f;
    
    [BoxGroup("Fill Image"), SerializeField]
    private float _fillDelay = 1f;

    private RectTransform _rectTransform;
    private Tween _moveTween;
    private Tween _fillAmountTween;

    private void Awake()
    {
        _fillAmountImage.fillAmount = 0f;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _initYPos);
    }

    private void Start()
    {
        if (_moveTween != null && _moveTween.IsPlaying()) _moveTween.Kill();
        if (_fillAmountTween != null && _fillAmountTween.IsPlaying()) _fillAmountTween.Kill();
        _moveTween = _rectTransform.DOAnchorPosY(_finalYPos, _duration).SetEase(_ease).SetDelay(_delay);
        _fillAmountTween = DOVirtual.Float(0f, 1f, _fillDuration, value => _fillAmountImage.fillAmount = value)
            .SetEase(Ease.OutSine).SetDelay(_fillDelay);
    }
}
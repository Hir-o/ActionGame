
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

public class ButtonPopInTweener : MonoBehaviour
{
    [SerializeField] private Vector2 _initSizeDelta = new Vector2(0f, 0f);
    [SerializeField] private Vector2 _finalSizeDelta = new Vector2(152f, 185f);
    [SerializeField] private float _sizeDuration = .75f;
    [SerializeField] private float _sizeDelay;
    [SerializeField] private RectTransform _buttonRectTransform;
    [SerializeField] private CanvasGroup _buttonCanvasGroup;
    [SerializeField] private Image _iconImage;
    [SerializeField] private float _fadeDuration = .75f;
    [SerializeField] private Ease _fadeEase = Ease.OutCubic;
    [SerializeField] private float _fadeDelay;
    [BoxGroup("Skip Start Method"), SerializeField] private bool _skipStart = false;

    private void Awake() => Reset();

    private void Start()
    {
        if (_skipStart) return;
        Animate();
    } 

    public void Animate()
    {
        _buttonRectTransform.DOSizeDelta(_finalSizeDelta, _sizeDuration).SetEase(EasePresets.Instance.BounceOnceEase)
            .SetDelay(_sizeDelay).SetUpdate(true);
        _iconImage.DOFade(1f, _fadeDuration).SetEase(_fadeEase).SetDelay(_fadeDelay).SetUpdate(true)
            .OnComplete(() => _buttonCanvasGroup.blocksRaycasts = true);
    }

    public void Reset()
    {
        _buttonCanvasGroup.blocksRaycasts = false;
        _iconImage.DOFade(0f, 0f).SetUpdate(true);
        _buttonRectTransform.sizeDelta = _initSizeDelta;
    }
}
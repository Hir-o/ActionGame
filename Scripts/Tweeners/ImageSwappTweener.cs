using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwappTweener : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite _positiveSprite;
    [SerializeField] private Sprite _negativeSprite;
    [SerializeField] private float _duration = .5f;

    private int _spriteIndex;

    private void Start()
    {
        if (_targetImage == null)
            _targetImage = GetComponent<Image>();

        if (_targetImage == null || _positiveSprite == null || _negativeSprite == null) return;
        DOVirtual.DelayedCall(_duration, SwapSprite).SetLoops(-1, LoopType.Yoyo);
    }

    private void SwapSprite()
    {
        _targetImage.sprite = _spriteIndex % 2 == 0 ? _positiveSprite : _negativeSprite;
        _spriteIndex++;
    }
}
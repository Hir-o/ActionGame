
using DG.Tweening;
using UnityEngine;

public class WreckingBallTweener : MonoBehaviour
{
    [SerializeField] private Transform _gfxTransform;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Ease _ease = Ease.OutQuad;
    [SerializeField] private float _yOffset = .5f;
    [SerializeField] private float _maxDelay = 0.5f;

    private void Start() => TweenGfx();

    private void TweenGfx()
    {
        float currYPos = _gfxTransform.transform.localPosition.y;
        float newYPos = currYPos + _yOffset;
        float delay = Random.Range(0f, _maxDelay);
        _gfxTransform.DOLocalMoveY(newYPos, _duration).SetEase(_ease).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
    }
}
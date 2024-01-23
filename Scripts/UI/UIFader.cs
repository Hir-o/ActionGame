
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;

public class UIFader : SceneSingleton<UIFader>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    
    [BoxGroup("Tweening"), SerializeField] private float _fadeSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fadeEase = Ease.OutQuad;

    private void Start()
    {
        FadeOut();
    }
    
    public void FadeIn()
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }
    
    public void FadeOut()
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }
}


using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UICollectables : SceneSingleton<UICollectables>
{
    [SerializeField] private Button _backButton;

    [BoxGroup("Tweening"), SerializeField] private float _fadeSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fadeEase = Ease.OutQuad;
    
    private CanvasGroup _canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _backButton.onClick.AddListener(OnClickBackButton);
    }

    public void OnClickBackButton() => FadeOut();

    public void FadeIn()
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }

    public void FadeOut(bool isBackButtonClicked = true)
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase)
            .OnComplete(() =>
            {
                if (isBackButtonClicked)
                    UIMenu.Instance.FadeIn();
            });
    }
}

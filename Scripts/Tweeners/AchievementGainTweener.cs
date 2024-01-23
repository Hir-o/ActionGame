
using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementGainTweener : MonoBehaviour
{
    private float _yOrigin;
    private float _yDestination;

    [BoxGroup("Movement Tweening"), SerializeField]
    private float _moveDuration = 0.5f;

    [BoxGroup("Movement Tweening"), SerializeField]
    private float _moveYAmount = 50f;

    [BoxGroup("Movement Tweening"), SerializeField]
    private float _moveDownDelay = 3f;

    [BoxGroup("Movement Tweening"), SerializeField]
    private Ease _moveEase = Ease.OutQuad;

    [BoxGroup("Completed Image Tweening"), SerializeField]
    private CanvasGroup _completedCanvasGroup;

    [BoxGroup("Completed Image Tweening"), SerializeField]
    private float _completedImageFadeDuration = 0.5f;

    [BoxGroup("Completed Image Tweening"), SerializeField]
    private Ease _imageEase = Ease.OutQuad;

    [BoxGroup("Completed Image Tweening"), SerializeField]
    private Vector2 _completedImageStartSizeDelta;

    [BoxGroup("Completed Image Tweening"), SerializeField]
    private Vector2 _completedImageFinalSizeDelta;

    [BoxGroup("Completed Image Tweening"), SerializeField]
    private float _completedImageScaleDuration = 0.8f;

    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _tmpName;
    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _tmpDescription;
    [BoxGroup("UI"), SerializeField] private Image _iconImage;
    [BoxGroup("UI"), SerializeField] private Image _backgroundImage;

    private Tween _moveUpTween;
    private Tween _moveDownTween;
    private Tween _fadeTween;
    private Tween _scaleTween;
    private RectTransform _completedImageRectTransform;
    private Queue<LocalAchievementData> _achievementsQueue = new Queue<LocalAchievementData>();

    private bool _isAnimating;

    private void Awake()
    {
        _yOrigin = transform.localPosition.y;
        _yDestination = _yOrigin + _moveYAmount;
        _completedCanvasGroup.alpha = 0f;

        _completedImageRectTransform = _completedCanvasGroup.GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        if (_moveUpTween != null) _moveUpTween.Kill();
        if (_moveDownTween != null) _moveDownTween.Kill();
        if (_fadeTween != null) _fadeTween.Kill();
        if (_scaleTween != null) _scaleTween.Kill();
    }

    public void AddToQueue(LocalAchievementData localAchievementData)
    {
        _achievementsQueue.Enqueue(localAchievementData);
        ShowUnlockedAchievements();
    }

    private void ShowUnlockedAchievements()
    {
        if (_achievementsQueue.Count > 0)
            Animate();
    }

    public void Animate()
    {
        if (_isAnimating) return;
        UpdateUI();

        _isAnimating = true;

        _moveUpTween = transform.DOLocalMoveY(_yDestination, _moveDuration)
            .SetEase(_moveEase)
            .SetUpdate(true);

        _moveUpTween.OnComplete(() =>
        {
            _fadeTween = _completedCanvasGroup.DOFade(1f, _completedImageFadeDuration)
                .SetEase(_imageEase)
                .SetUpdate(true);

            _scaleTween = _completedImageRectTransform
                .DOSizeDelta(_completedImageFinalSizeDelta, _completedImageScaleDuration).SetEase(_imageEase)
                .SetUpdate(true);

            _moveDownTween = transform.DOLocalMoveY(_yOrigin, _moveDuration)
                .SetEase(_moveEase)
                .SetUpdate(true)
                .SetDelay(_moveDownDelay).OnComplete(() => Reset());
        });
    }

    private void UpdateUI()
    {
        LocalAchievementData achievementData = _achievementsQueue.Dequeue();
        _tmpName.text = achievementData.Name;
        _tmpDescription.text = achievementData.Description;
        _iconImage.sprite = achievementData.IconSprite;
        _backgroundImage.sprite = achievementData.BackgroundSprite;
    }

    private void Reset()
    {
        _completedCanvasGroup.alpha = 0f;
        _completedImageRectTransform.sizeDelta = _completedImageStartSizeDelta;
        _isAnimating = false;
        ShowUnlockedAchievements();
    }
}
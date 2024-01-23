
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class UITutorialPanelTweener : MonoBehaviour
{
    public event Action OnTutorialPanelTweenerComplete;

    [SerializeField] private bool _isRightTutorialArea = true;
    [SerializeField] private bool _isLeftTutorialArea;
    [BoxGroup("Tweening"), SerializeField] private float _speed = 2f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease = Ease.OutQuad;
    [BoxGroup("Tweening"), SerializeField] private int _loops = 5;
    [BoxGroup("Tweening"), SerializeField] private bool _infiniteLoop;

    private CanvasGroup _canvasGroup;
    private UITutorial _uiTutorial;
    private Tween _tween;

    private bool _isTweenFinished;

    public bool IsTweenFinished => _isTweenFinished;

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    private void Start()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        StartTweening();
    }

    private void OnEnable()
    {
        if (_isLeftTutorialArea)
            MobileInputManager.Instance.OnPressLeftSideOfScreen += MobileInputManager_OnPressLeftSideOfScreen;

        if (_isRightTutorialArea)
            MobileInputManager.Instance.OnPressRightSideOfScreen += MobileInputManager_OnPressRightSideOfScreen;
    }

    private void OnDisable()
    {
        if (_tween != null)
            _tween.Kill();
    }

    private void StartTweening()
    {
        if (_infiniteLoop) _loops = -1;
        if (_tween != null) _tween.Kill();
        _tween = _canvasGroup.DOFade(1f, _speed)
            .SetSpeedBased()
            .SetEase(_ease)
            .SetLoops(_loops, LoopType.Yoyo)
            .SetUpdate(true)
            .OnComplete(FinishTweening);
    }

    private void FinishTweening()
    {
        if (_isTweenFinished) return;
        if (_tween != null) _tween.Kill();
        _isTweenFinished = true;
        OnTutorialPanelTweenerComplete?.Invoke();
    }

    private void MobileInputManager_OnPressLeftSideOfScreen() => FinishTweeningFromInput();
    private void MobileInputManager_OnPressRightSideOfScreen() => FinishTweeningFromInput();

    private void FinishTweeningFromInput()
    {
        if (!gameObject.activeSelf) return;
        TimeScaleManager.Instance.UpdateTimeScale(1f, false);
        if (UIInteractiveTutorial.Instance != null) UIInteractiveTutorial.Instance.Reset();
        if (_tween != null) _tween.Kill();
        _tween = _canvasGroup.DOFade(0f, _speed)
            .SetSpeedBased()
            .SetEase(_ease)
            .SetUpdate(true)
            .OnComplete(FinishTweening);
    }
}
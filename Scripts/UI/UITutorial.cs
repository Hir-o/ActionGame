using DG.Tweening;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    private UITutorialPanelTweener[] _tutorialPanelTweeners;

    private Tween _canvasFadeTween;
    private bool _isTutorialFinished;

    private void Awake() => _tutorialPanelTweeners = GetComponentsInChildren<UITutorialPanelTweener>(true);

    private void OnEnable()
    {
        for (int i = 0; i < _tutorialPanelTweeners.Length; i++)
            _tutorialPanelTweeners[i].OnTutorialPanelTweenerComplete +=
                UITutorialPanelTweener_OnTutorialPanelTweenerComplete;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _tutorialPanelTweeners.Length; i++)
            _tutorialPanelTweeners[i].OnTutorialPanelTweenerComplete -=
                UITutorialPanelTweener_OnTutorialPanelTweenerComplete;
    }

    private void UITutorialPanelTweener_OnTutorialPanelTweenerComplete() => TryToStopTutorial();

    public void TryToStopTutorial()
    {
        if (_isTutorialFinished) return;
        for (int i = 0; i < _tutorialPanelTweeners.Length; i++)
            if (!_tutorialPanelTweeners[i].IsTweenFinished)
                return;

        _isTutorialFinished = true;
        DisableCanvas();
    }

    private void DisableCanvas() => gameObject.SetActive(false);
}
using System;
using DG.Tweening;
using Leon;
using UnityEngine;

public class UIImprovedTutorial : SceneSingleton<UIImprovedTutorial>
{
    public event Action<Action> OnTutorialShow;
    public event Action OnTutorialFinished;

    [SerializeField] private bool _completeWithLeftTap;
    [SerializeField] private bool _completeWithRightTap = true;
    [SerializeField] private float _tutorialDisplayDelay = 1.8f;

    private bool _canSkipTutorial;

    public bool CanSkipTutorial => _canSkipTutorial;

    private void OnEnable()
    {
        if (_completeWithLeftTap)
            MobileInputManager.Instance.OnPressLeftSideOfScreen += MobileInputManager_OnPressLeftSideOfScreen;

        if (_completeWithRightTap)
            MobileInputManager.Instance.OnPressRightSideOfScreen += MobileInputManager_OnPressRightSideOfScreen;
    }

    private void Start() =>
        DOVirtual.DelayedCall(_tutorialDisplayDelay, () => OnTutorialShow?.Invoke(MakeTutorialSkippable));

    private void MobileInputManager_OnPressLeftSideOfScreen() => FinishTweeningFromInput();
    private void MobileInputManager_OnPressRightSideOfScreen() => FinishTweeningFromInput();

    private void MakeTutorialSkippable() => _canSkipTutorial = true;

    private void FinishTweeningFromInput()
    {
        if (!gameObject.activeSelf) return;
        OnTutorialFinished?.Invoke();
    }
}
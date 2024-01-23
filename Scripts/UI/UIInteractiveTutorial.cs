
using System;
using Leon;
using UnityEngine;

public class UIInteractiveTutorial : SceneSingleton<UIInteractiveTutorial>
{
    private UITutorial _uiTutorial;
    private UITutorialPanelTweener[] _uiTutorialPanelTweenerArray;

    [SerializeField] private bool _enableJumping;
    [SerializeField] private bool _enableDoubleJumping;
    [SerializeField] private bool _enableHalting;

    protected override void Awake()
    {
        base.Awake();
        _uiTutorial = GetComponent<UITutorial>();
        _uiTutorialPanelTweenerArray = GetComponentsInChildren<UITutorialPanelTweener>();
    }

    public void EnableTutorial()
    {
        if (_enableJumping) MovementController.Instance.CanJump = true;
        if (_enableDoubleJumping) MovementController.Instance.CanDoubleJump = true;
        if (_enableHalting)
        {
            MovementController.Instance.CanJump = false;
            MovementController.Instance.CanDoubleJump = false;
            MovementController.Instance.HaltAction.enabled = true;
            MovementController.Instance.HaltAction.IsEnabled = true;
        }

        _uiTutorial.enabled = true;
        foreach (var uiTutorialPanelTweener in _uiTutorialPanelTweenerArray)
            uiTutorialPanelTweener.enabled = true;
    }

    public void Reset()
    {
        if (_enableHalting)
        {
            MovementController.Instance.CanJump = true;
            MovementController.Instance.CanDoubleJump = true;
        }
    }
}
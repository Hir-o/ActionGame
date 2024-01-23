
using System;
using UnityEngine;

[Serializable]
public struct UITutorialPart
{
    [SerializeField] private UITutorialPanelTweener _tutorialPanelTweener;
    [SerializeField] private bool _enableJumping;
    [SerializeField] private bool _enableDoubleJumping;
    [SerializeField] private bool _enableGliding;
    [SerializeField] private bool _disableJumpingAfterEnabling;

    public UITutorialPanelTweener TutorialPanelTweener => _tutorialPanelTweener;
    public bool EnableJumping => _enableJumping;
    public bool EnableDoubleJumping => _enableDoubleJumping;
    public bool EnableGliding => _enableGliding;
    public bool DisableJumpingAfterEnabling => _disableJumpingAfterEnabling;
}

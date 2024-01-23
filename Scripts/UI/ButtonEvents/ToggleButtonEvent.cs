using Lean.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButtonEvent : MonoBehaviour, IButtonAudioClickEvent
{
    private void Awake()
    {
        LeanButton leanButton = GetComponent<LeanButton>();
        leanButton.OnClick.AddListener(HandleButtonClickEvent);
    }
    public void HandleButtonClickEvent()
    {
        UISfxManager.Instance.PlaySoundEffect(UISfxManager.Instance.ToggleOnSFX);
    }
}

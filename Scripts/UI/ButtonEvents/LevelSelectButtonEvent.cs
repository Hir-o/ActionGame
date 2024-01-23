using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonEvent : MonoBehaviour, IButtonAudioClickEvent
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HandleButtonClickEvent);
    }

    public void HandleButtonClickEvent()
    {
        UISfxManager.Instance.PlaySoundEffect(UISfxManager.Instance.LevelSelectButtonSFX);
    }
}

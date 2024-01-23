using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXSliderEvent : MonoBehaviour, ISliderAudioClickEvent
{
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleSliderValueChangeEvent);
    }

    public void HandleSliderValueChangeEvent(float amount)
    {
        UISfxManager.Instance.PlaySoundEffect(UISfxManager.Instance.ToggleOnSFX);
    }
}

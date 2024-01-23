using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishSoundEvent : MonoBehaviour
{
    private FinishStructureTweener _finishStructureTweener;

    private void Awake() => _finishStructureTweener = GetComponent<FinishStructureTweener>();
    private void OnEnable() => _finishStructureTweener.OnWindowsOpen += FinishStructureTweener_OnWindowsOpen;
    private void OnDisable() => _finishStructureTweener.OnWindowsOpen -= FinishStructureTweener_OnWindowsOpen;

    private void FinishStructureTweener_OnWindowsOpen()
    {
        if (SoundEffectsManager.Instance == null) return;
        SoundEffectsManager.Instance.PlayWinSfx();
    }
}
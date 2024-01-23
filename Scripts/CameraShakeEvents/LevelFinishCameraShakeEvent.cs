using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishCameraShakeEvent : MonoBehaviour
{
    private void OnEnable()
    {
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
    }
    private void OnDisable()
    {
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;
    }
    private void LevelFinish_OnAnyLevelCompleted()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.LevelFinish);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishVFXEvent : MonoBehaviour
{
    private void OnEnable()
    {
        LevelFinish.OnAnyLevelCompleted += LevelFinish_OnLevelCompleted;



    }
    private void OnDisable()
    {
        LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnLevelCompleted;

    }

    private void LevelFinish_OnLevelCompleted()
    {

      
            if (FinishParticleFactory.Instance != null)
                FinishParticleFactory.Instance.GetNewFinishVfxInstance(transform.position);
       
    }
}

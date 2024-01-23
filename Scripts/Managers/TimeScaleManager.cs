
using Leon;
using UnityEngine;

public class TimeScaleManager : SceneSingleton<TimeScaleManager>
{
    private bool _isTutorialEnabled;
    
    public void UpdateTimeScale(float value)
    {
        if (_isTutorialEnabled && value > 0f) return;
        Time.timeScale = value;
    }

    public void UpdateTimeScale(float value, bool isTutorialEnabled)
    {
        _isTutorialEnabled = isTutorialEnabled;
        Time.timeScale = value;
    }

    public void UpdateTimeScaleOnChangeScene(float value = 1f) => Time.timeScale = value;
}

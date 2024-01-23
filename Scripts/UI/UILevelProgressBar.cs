using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelProgressBar : MonoBehaviour
{
    [SerializeField] private SliderPresenter _sliderPresenter;

    private void OnEnable() => LevelDistance.OnCalculatingDistance += LevelDistance_OnCalculatingDistance;

    private void OnDisable() => LevelDistance.OnCalculatingDistance -= LevelDistance_OnCalculatingDistance;

    private void LevelDistance_OnCalculatingDistance(float currentDistance, float totalDistance) =>
        _sliderPresenter.UpdateSlider(currentDistance, totalDistance);
}
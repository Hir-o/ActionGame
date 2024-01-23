using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPresenter : MonoBehaviour
{
    private Slider _progressSlider;
  

    private void Awake()
    {
        _progressSlider = gameObject.GetComponent<Slider>();
        _progressSlider.value = 0f;

    }
    public void UpdateSlider(float currentDistance,float totalDistance)
    {
        _progressSlider.maxValue = totalDistance;
        _progressSlider.value = totalDistance - currentDistance;
    }

}

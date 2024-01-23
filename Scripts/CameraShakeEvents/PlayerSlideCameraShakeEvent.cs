using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideCameraShakeEvent : MonoBehaviour
{
    bool _slideCancel;
    private void OnEnable()
    {
        MovementController.OnPlayerSlide += MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel += MovementController_OnPlayerSlideCancel;
    }


    private void OnDisable()
    {
        MovementController.OnPlayerSlide -= MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel -= MovementController_OnPlayerSlideCancel;
    }
    private void MovementController_OnPlayerSlide()
    {
        CameraShakeController.Instance.ShakeCamera(ShakeType.Sliding, true);
        _slideCancel = true;
    }

    private void MovementController_OnPlayerSlideCancel(bool obj)
    {
        if (_slideCancel)
        {
            CameraShakeController.Instance.ShakeCamera(ShakeType.Sliding, false);
            _slideCancel = false;
        }
    }
}

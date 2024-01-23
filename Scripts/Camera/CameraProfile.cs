using System;
using Cinemachine;
using UnityEngine;

[Serializable]
public struct CameraProfile
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CameraProfileType _cameraProfileType;

    public CinemachineVirtualCamera VirtualCamera => _virtualCamera;
    public CameraProfileType CameraProfileType => _cameraProfileType;

    public void UpdateVirtualCamera(bool enabled)
    {
        _virtualCamera.gameObject.SetActive(enabled);
        /*if (_cameraProfileType == CameraProfileType.RopeHanging)
        {
            RopeHangAction ropeHangAction = MovementController.Instance.RopeHangAction;
            if (!ropeHangAction.IsHangingFromRope) return;
            Transform currentHangedRope = MovementController.Instance.RopeHangAction.CurrentHangedRope;
            _virtualCamera.m_Follow = currentHangedRope;
            _virtualCamera.m_LookAt = currentHangedRope;
        }*/
    }
}

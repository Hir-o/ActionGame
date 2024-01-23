using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;

public class CameraManager : SceneSingleton<CameraManager>
{
    [SerializeField] private CameraProfile[] _cameraProfileArray;
    private CameraProfileType _currCameraProfileType = CameraProfileType.Idle;

    [BoxGroup("Rope release"), SerializeField]
    private float _ropeReleaseCameraSwitchDelay = 1f;

    private Tween _ropeReleaseTween;
    public CameraProfile[] CameraProfileArray => _cameraProfileArray; 

    private void OnEnable()
    {
        ClimbAction.OnPlayerClimb += ClimbAction_OnPlayerClimb;
        ClimbAction.OnLeaveFinalLadder += ClimbAction_OnLeaveFinalLadder;
        CharacterPlayerController.OnPlayerDiedCamera += CharacterPlayerController_OnPlayerDiedCamera;
        RopeHangAction.OnAnyGrabRope += RopeHangAction_OnAnyGrabRope;
        RopeHangAction.OnAnyReleaseRope += RopeHangAction_OnAnyReleaseRope;
        MovementController.OnPlayerRun += MovementController_OnPlayerRun;
        Reorientator.OnPlayerEnter += Reorientator_OnPlayerEnter;
        CameraSwitcher.OnAnySwitchCamera += CameraSwitcher_OnAnySwitchCamera;
        FlyAction.OnPlayerFly += FlyAction_OnPlayerFly;
        FlyAction.OnPlayerFlyCancel += FlyAction_OnPlayerFlyCancel;
        SwimAction.OnPlayerSwim += SwimAction_OnPlayerSwim;
        SwimAction.OnPlayerSwimCancel += SwimAction_OnPlayerSwimCancel;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnDisable()
    {
        ClimbAction.OnPlayerClimb -= ClimbAction_OnPlayerClimb;
        ClimbAction.OnLeaveFinalLadder -= ClimbAction_OnLeaveFinalLadder;
        CharacterPlayerController.OnPlayerDiedCamera -= CharacterPlayerController_OnPlayerDiedCamera;
        RopeHangAction.OnAnyGrabRope -= RopeHangAction_OnAnyGrabRope;
        RopeHangAction.OnAnyReleaseRope -= RopeHangAction_OnAnyReleaseRope;
        MovementController.OnPlayerRun -= MovementController_OnPlayerRun;
        Reorientator.OnPlayerEnter -= Reorientator_OnPlayerEnter;
        CameraSwitcher.OnAnySwitchCamera -= CameraSwitcher_OnAnySwitchCamera;
        FlyAction.OnPlayerFly -= FlyAction_OnPlayerFly;
        FlyAction.OnPlayerFlyCancel -= FlyAction_OnPlayerFlyCancel;
        SwimAction.OnPlayerSwim -= SwimAction_OnPlayerSwim;
        SwimAction.OnPlayerSwimCancel -= SwimAction_OnPlayerSwimCancel;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
    }

    private void ClimbAction_OnPlayerClimb(float direction) => UpdateCameras(CameraProfileType.Climbing);
    private void ClimbAction_OnLeaveFinalLadder() => UpdateCameras(CameraProfileType.Normal);
    private void CharacterPlayerController_OnPlayerDiedCamera() => UpdateCameras(CameraProfileType.Dead);
    private void RopeHangAction_OnAnyGrabRope(float direction) => UpdateCameras(CameraProfileType.RopeHanging);
    private void RopeHangAction_OnAnyReleaseRope()
    {
        if (_ropeReleaseTween != null) _ropeReleaseTween.Kill();
        _ropeReleaseTween = DOVirtual.Float(1f, 0f, _ropeReleaseCameraSwitchDelay, value => { })
            .OnComplete(() => UpdateCameras(CameraProfileType.Normal));
    }

    private void MovementController_OnPlayerRun(bool isMovementPressed)
    {
        if (_currCameraProfileType == CameraProfileType.WallJumping) return;
        if (isMovementPressed)
            UpdateCameras(CameraProfileType.Normal);
    }

    private void Reorientator_OnPlayerEnter() => UpdateCameras(CameraProfileType.WallJumping);

    private void CameraSwitcher_OnAnySwitchCamera(CameraProfileType cameraProfileType) =>
        UpdateCameras(cameraProfileType);
    private void FlyAction_OnPlayerFly() => UpdateCameras(CameraProfileType.Floating);
    private void FlyAction_OnPlayerFlyCancel() => UpdateCameras(CameraProfileType.Normal);
    private void SwimAction_OnPlayerSwim() => UpdateCameras(CameraProfileType.Floating);
    private void SwimAction_OnPlayerSwimCancel() => UpdateCameras(CameraProfileType.Normal);

    private GameObject _camGameObject;
    
    private void PlayerRespawn_OnPlayerRespawn()
    {
        if (_currCameraProfileType == CameraProfileType.Dead)
            foreach (var cam in CameraProfileArray)
            {
                if (!cam.VirtualCamera.gameObject.activeSelf) continue;
                _camGameObject = cam.VirtualCamera.gameObject;
                _camGameObject.SetActive(false);
            }

        foreach (var t in CameraProfileArray)
        {
            if (!t.VirtualCamera.gameObject.activeSelf) continue;
            _camGameObject = t.VirtualCamera.gameObject;
            _camGameObject.SetActive(false);
            DOVirtual.Float(1f, 0f, .25f, value => { }).OnComplete(() => _camGameObject.SetActive(true));
        }
    }

    private void UpdateCameras(CameraProfileType cameraProfileType)
    {
        if (_ropeReleaseTween != null) _ropeReleaseTween.Kill();
        if (_currCameraProfileType == cameraProfileType) return;
        _currCameraProfileType = cameraProfileType;

        for (int i = 0; i < CameraProfileArray.Length; i++)
        {
            CameraProfileType profile = CameraProfileArray[i].CameraProfileType;
            CameraProfileArray[i].UpdateVirtualCamera(profile == cameraProfileType);
        }
    }
}
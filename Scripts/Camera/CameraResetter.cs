using System;
using Cinemachine;
using DG.Tweening;
using Leon;
using UnityEngine;

public class CameraResetter : SceneSingleton<CameraResetter>
{
    public event Action OnDisableVirtualCamera;
    public event Action OnReEnableVirtualCamera;

    [SerializeField] private CinemachineVirtualCamera[] _virtualCameras;
    private float _originalPosZ;

    private void Awake() => _originalPosZ = transform.position.z;
    private void OnEnable() => PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    private void OnDisable() => PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;

    private void PlayerRespawn_OnPlayerRespawn()
    {
        Vector3 targetPosition = CharacterPlayerController.Instance.transform.position;
        targetPosition.z = _originalPosZ;
        transform.position = targetPosition;

        OnDisableVirtualCamera?.Invoke();
        for (int i = 0; i < _virtualCameras.Length; i++)
        {
            _virtualCameras[i].PreviousStateIsValid = false;
            _virtualCameras[i].transform.position = targetPosition;
        }

        OnReEnableVirtualCamera?.Invoke();
        DOVirtual.DelayedCall(.25f, () =>
        {
            for (int i = 0; i < _virtualCameras.Length; i++)
            {
                _virtualCameras[i].PreviousStateIsValid = true;
            }
        });
    }
}
using Cinemachine;
using UnityEngine;

public class CinemachinePropertyAssigner : MonoBehaviour
{
    [SerializeField] private bool _isDisabledByDefault;

    private Transform _playerTransform;
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _playerTransform = CharacterPlayerController.Instance.transform;

        CameraResetter.Instance.OnDisableVirtualCamera += CameraResetter_OnDisableVirtualCamera;
        CameraResetter.Instance.OnReEnableVirtualCamera += CameraResetter_OnReEnableVirtualCamera;

        AssingProperties();
        if (_isDisabledByDefault) gameObject.SetActive(false);
    }

    private void CameraResetter_OnDisableVirtualCamera() => UnassignProperties();
    private void CameraResetter_OnReEnableVirtualCamera() => AssingProperties();

    public void AssingProperties()
    {
        _virtualCamera.Follow = _playerTransform;
        _virtualCamera.LookAt = _playerTransform;
    }

    public void UnassignProperties()
    {
        _virtualCamera.Follow = null;
        _virtualCamera.LookAt = null;
    }
}

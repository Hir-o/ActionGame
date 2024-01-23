using System;
using UnityEngine;

public class BossDashTweener : MonoBehaviour
{
    private BaseBoss _baseBoss;

    [SerializeField] private Transform _rotatingTransform;
    [SerializeField] private Vector3 _newRotation;

    private Vector3 _oldRotation;

    private void Awake()
    {
        _baseBoss = GetComponent<BaseBoss>();
        _oldRotation = _rotatingTransform.localRotation.eulerAngles;
    }

    private void OnEnable()
    {
        _baseBoss.OnDash += BaseBoss_OnDash;
        _baseBoss.OnStopDash += BaseBoss_OnStopDash;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnDisable()
    {
        _baseBoss.OnDash -= BaseBoss_OnDash;
        _baseBoss.OnStopDash -= BaseBoss_OnStopDash;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
    }

    private void BaseBoss_OnDash() => _rotatingTransform.localRotation = Quaternion.Euler(_newRotation);
    private void BaseBoss_OnStopDash() => ResetRotation();
    private void PlayerRespawn_OnPlayerRespawn() => ResetRotation();
    private void ResetRotation() => _rotatingTransform.localRotation = Quaternion.Euler(_oldRotation);
}
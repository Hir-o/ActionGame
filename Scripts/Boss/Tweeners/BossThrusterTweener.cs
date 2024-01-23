using System;
using UnityEngine;

public class BossThrusterTweener : MonoBehaviour
{
    private BaseBoss _baseBoss;

    [SerializeField] private ThrusterTweener _thrusterTweener;
    [SerializeField] private Vector3 _newScale;

    private Vector3 _oldScale;

    private void Awake() => _baseBoss = GetComponent<BaseBoss>();

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

    private void BaseBoss_OnDash() => _thrusterTweener.StartTweening(_newScale);
    private void BaseBoss_OnStopDash() => ResetThrusterScale();
    private void PlayerRespawn_OnPlayerRespawn() => ResetThrusterScale();
    private void ResetThrusterScale() => _thrusterTweener.StartTweening(_thrusterTweener.OriginalScale);
}
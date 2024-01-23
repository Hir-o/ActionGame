using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingBotThrusterTweener : MonoBehaviour
{
    private DashingEnemy _dashingEnemy;

    [SerializeField] private ThrusterTweener _thrusterTweener;
    [SerializeField] private Vector3 _newScale;

    private void Awake() => _dashingEnemy = GetComponent<DashingEnemy>();

    private void OnEnable()
    {
        _dashingEnemy.DashingBotOnIdle += DashingEnemy_DashingBotOnIdle;
        _dashingEnemy.DashingBotOnAttack += DashingEnemy_DashingBotOnAttack;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnDisable()
    {
        _dashingEnemy.DashingBotOnIdle -= DashingEnemy_DashingBotOnIdle;
        _dashingEnemy.DashingBotOnAttack -= DashingEnemy_DashingBotOnAttack;
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        _thrusterTweener.ResetTween();
    }

    private void PlayerRespawn_OnPlayerRespawn() => ResetThrusterScale();
    private void DashingEnemy_DashingBotOnAttack() => _thrusterTweener.StartTweening(_newScale);
    private void DashingEnemy_DashingBotOnIdle() => ResetThrusterScale();
    private void ResetThrusterScale() => _thrusterTweener.StartTweening(_thrusterTweener.OriginalScale);
}

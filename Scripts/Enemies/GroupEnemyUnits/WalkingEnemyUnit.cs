using System;
using UnityEngine;

public class WalkingEnemyUnit : MonoBehaviour
{
    private EnemyJumpPad _enemyJumpPad;
    private WalkingEnemy _walkingEnemy;

    private void Awake()
    {
        _enemyJumpPad = GetComponentInChildren<EnemyJumpPad>();
        _walkingEnemy = GetComponentInParent<WalkingEnemy>();
    }

    public void OnTriggerEnter(Collider other) => _walkingEnemy.OnEnemyTriggerEnter(other, _enemyJumpPad);
}
using NaughtyAttributes;
using UnityEngine;

public class BossDamageEffect : MonoBehaviour
{
    [SerializeField] private BossSmallExplosions _smallExplosions1;
    [SerializeField] private BossSmallExplosions _smallExplosions2;
    [SerializeField] private BossSmallExplosions _smallExplosions3;

    [SerializeField] private float _triggerExplosions1Percentage = 0.6f;
    [SerializeField] private float _triggerExplosions2Percentage = 0.3f;
    [SerializeField] private float _triggerExplosions3Percentage = 0f;

    private IDamageable _damageableBoss;

    private void Awake() => _damageableBoss = GetComponent<IDamageable>();

    private void OnEnable()
    {
        _damageableBoss.OnHealthChanged += DamageableBoss_OnHealthChanged;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnDisable()
    {
        _damageableBoss.OnHealthChanged -= DamageableBoss_OnHealthChanged;
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void DamageableBoss_OnHealthChanged(int currHealth, int maxHealth)
    {
        float damagePercentage = (float)currHealth / maxHealth;
        if (damagePercentage <= _triggerExplosions1Percentage) _smallExplosions1.TriggerExplosions();
        if (damagePercentage <= _triggerExplosions2Percentage) _smallExplosions2.TriggerExplosions();
        if (damagePercentage <= _triggerExplosions3Percentage) _smallExplosions3.TriggerExplosions();
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        _smallExplosions1.DisableExplosions();
        _smallExplosions2.DisableExplosions();
        _smallExplosions3.DisableExplosions();
    }
}
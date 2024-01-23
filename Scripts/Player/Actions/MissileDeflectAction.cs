
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class MissileDeflectAction : BaseAction
{
    [BoxGroup("Missle Deflection"), SerializeField]
    private float _missleDeflectDuration = 0.25f;

    [BoxGroup("Parry Particles"), SerializeField]
    private GameObject _parryVfx;
    
    [BoxGroup("Parry Particles"), SerializeField]
    private Transform _parryVfxSpawnTransform;

    private float _currMissleDeflectDuration;

    #region Properties

    public bool HasMissileInvincibility => _currMissleDeflectDuration > 0f;

    #endregion

    private void FixedUpdate()
    {
        if (_currMissleDeflectDuration > 0f)
            _currMissleDeflectDuration -= Time.fixedDeltaTime;
    }

    public void EnableMissileInvincibility() => _currMissleDeflectDuration = _missleDeflectDuration;

    public void SpawnParryVfx() =>
        LeanPool.Spawn(_parryVfx, _parryVfxSpawnTransform.position, _parryVfx.transform.rotation);
}
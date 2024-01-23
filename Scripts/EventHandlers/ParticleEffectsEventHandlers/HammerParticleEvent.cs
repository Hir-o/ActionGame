using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Factory;
using EmissionModule = UnityEngine.ParticleSystem.EmissionModule;

public class HammerParticleEvent : MonoBehaviour
{
    [SerializeField] private Transform _hammerBotTransform;
    private HammerBot _hammerBot;

    public enum HammerEffectType
    {
        HammerCrack,
        HammerDustCrack
    }

    [SerializeField] private HammerEffectType _selectedHammerEffect;

    [SerializeField] private float _setDealyVfx;

    private void Awake() => _hammerBot = GetComponent<HammerBot>();
    private void OnEnable() => _hammerBot.OnHammerHitGround += HammerBot_OnHammerHitGround;
    private void OnDisable() => _hammerBot.OnHammerHitGround -= HammerBot_OnHammerHitGround;
    private void HammerBot_OnHammerHitGround() => DOVirtual.DelayedCall(_setDealyVfx, PlayHammerVfx);

    public void PlayHammerVfx()
    {
        if (_selectedHammerEffect == HammerEffectType.HammerCrack)
        {
            if (TrapParticleFactory.Instance != null)
                TrapParticleFactory.Instance.GetNewHammerTrapVfxInstance(_hammerBotTransform.position);
        }
        else if (_selectedHammerEffect == HammerEffectType.HammerDustCrack)
        {
            if (TrapParticleFactory.Instance != null)
                TrapParticleFactory.Instance.GetNewHammerDustVfxInstance(_hammerBotTransform.position);
        }
    }
}
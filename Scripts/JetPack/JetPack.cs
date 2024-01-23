
using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    [SerializeField] private GameObject _gfx;

    [BoxGroup("Thruster Particles"), SerializeField]
    private ThrusterParticles[] _thrusterParticlesArray;

    [BoxGroup("Thruster Vfx Profiles"), SerializeField]
    private ThrusterVfxProfile[] _thrusterProfileArray;

    [BoxGroup("Vfx Parent Transform"), SerializeField]
    private Transform _vfxParentTransform;

    [BoxGroup("Particle Reset Timer"), SerializeField]
    private float _particleResetTimer = 1f;

    private Tween _resetParticlesTween;

    private void Start()
    {
        _gfx.SetActive(false);
        UpdateParticles(false);
    }

    private void OnEnable()
    {
        FlyAction.OnEquippFlyingEquipment += FlyAction_OnEquippFlyingEquipment;
        FlyAction.OnPlayerFly += FlyAction_OnPlayerFly;
        FlyAction.OnPlayerFlyCancel += FlyAction_OnPlayerFlyCancel;
        FlyAction.OnAnyFlyAscending += FlyAction_OnUpdateParticles;
        FlyAction.OnAnyFlyDescending += FlyAction_OnUpdateParticles;
        FlyAction.OnAnyStopFlyAscending += FlyAction_OnUpdateParticles;
        FlyAction.OnAnyStopFlyDescending += FlyAction_OnUpdateParticles;
    }

    private void OnDisable()
    {
        FlyAction.OnEquippFlyingEquipment -= FlyAction_OnEquippFlyingEquipment;
        FlyAction.OnPlayerFly -= FlyAction_OnPlayerFly;
        FlyAction.OnPlayerFlyCancel -= FlyAction_OnPlayerFlyCancel;
        FlyAction.OnAnyFlyAscending -= FlyAction_OnUpdateParticles;
        FlyAction.OnAnyFlyDescending -= FlyAction_OnUpdateParticles;
        FlyAction.OnAnyStopFlyAscending -= FlyAction_OnUpdateParticles;
        FlyAction.OnAnyStopFlyDescending -= FlyAction_OnUpdateParticles;
    }

    private void FlyAction_OnEquippFlyingEquipment(bool isPlayerFlying) => UpdateJetPackGraphic(isPlayerFlying);

    private void FlyAction_OnPlayerFly() => UpdateParticles(true);
    private void FlyAction_OnPlayerFlyCancel() => UpdateParticles(false, true);

    private void UpdateJetPackGraphic(bool isPlayerFlying) => _gfx.SetActive(isPlayerFlying);

    private void FlyAction_OnUpdateParticles(FloatActionDirection? flyActionDirection)
    {
        ThrusterVfxProfile selectedProfile = default;
        for (int i = 0; i < _thrusterProfileArray.Length; i++)
        {
            if (_thrusterProfileArray[i].FlyActionDirection != flyActionDirection) continue;
            selectedProfile = _thrusterProfileArray[i];
            break;
        }

        if (selectedProfile.Equals(default)) return;
        for (int i = 0; i < _thrusterParticlesArray.Length; i++)
        {
            selectedProfile.UpdateVfx(_thrusterParticlesArray[i].FireParticles,
                _thrusterParticlesArray[i].SmokeParticles, _thrusterParticlesArray[i].GlowParticles);
        }
    }

    private void UpdateParticles(bool active, bool disableWithDelay = false)
    {
        if (disableWithDelay)
        {
            for (var i = 0; i < _thrusterParticlesArray.Length; i++)
                _thrusterParticlesArray[i].MainParticleTransform.parent = null;

            if (_resetParticlesTween != null) _resetParticlesTween.Kill();
            _resetParticlesTween = DOVirtual.Float(_particleResetTimer, 0f, _particleResetTimer, value => { })
                .OnComplete(() => { ResetParticlePosition(active); });

            return;
        }

        for (var i = 0; i < _thrusterParticlesArray.Length; i++)
            _thrusterParticlesArray[i].MainParticleTransform.gameObject.SetActive(active);
    }

    private void ResetParticlePosition(bool active)
    {
        for (int i = 0; i < _thrusterParticlesArray.Length; i++)
        {
            if (_thrusterParticlesArray[i].MainParticleTransform == null) return;
            _thrusterParticlesArray[i].MainParticleTransform.gameObject.SetActive(active);
            _thrusterParticlesArray[i].MainParticleTransform.parent = _vfxParentTransform;
            _thrusterParticlesArray[i].MainParticleTransform.localPosition = _thrusterParticlesArray[i].SpawnPosition;
        }
    }
}
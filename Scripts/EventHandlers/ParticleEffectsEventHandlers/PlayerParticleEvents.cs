

using Factory;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using EmissionModule = UnityEngine.ParticleSystem.EmissionModule;

public class PlayerParticleEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem _slidingParticleSystem;
    [SerializeField] private ParticleSystem[] _wallSlideVfx;

    [BoxGroup("Footstep Vfx Spawn Position"), SerializeField]
    private Transform _footstepSpawnTransform;
    
    private List<ParticleSystem> _particleSystems;
    
    private void Awake()
    {
        ToggleSlidingVfx(false);
        InitializeParticleSystems();
    }

    private void OnEnable()
    {
        MovementController.OnPlayerSlide += MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel += MovementController_OnPlayerSlideCancel;
        MovementController.OnPlayerWallHang += MovementController_OnPlayerWallslide;
        MovementController.OnPlayerRun += MovementController_OffPlayerWallslide;
        MovementController.OnPlayerIdle += MovementController_OffPlayerWallslide;
        MovementController.OnPlayerJump += MovementController_StopPlayerWallslide;
        MovementController.OnPlayerSlide += MovementController_StopPlayerWallslide;
        MovementController.OnPlayerJumpFromJumpad += MovementController_StopPlayerWallslide;
        MovementController.OnPlayerDoubleJump += MovementController_StopPlayerWallslide;
    }

    private void OnDisable()
    {
        MovementController.OnPlayerSlide -= MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel -= MovementController_OnPlayerSlideCancel;
        MovementController.OnPlayerWallHang -= MovementController_OnPlayerWallslide;
        MovementController.OnPlayerRun -= MovementController_OffPlayerWallslide;
        MovementController.OnPlayerIdle -= MovementController_OffPlayerWallslide;
        MovementController.OnPlayerJump -= MovementController_StopPlayerWallslide;
        MovementController.OnPlayerSlide -= MovementController_StopPlayerWallslide;
        MovementController.OnPlayerJumpFromJumpad -= MovementController_StopPlayerWallslide;
        MovementController.OnPlayerDoubleJump -= MovementController_StopPlayerWallslide;
    }

    private void MovementController_OnPlayerSlide() => ToggleSlidingVfx(true);
    private void MovementController_OnPlayerSlideCancel(bool isMovmentPressed) => ToggleSlidingVfx(false);

    public void PlayFootstepVfx()
    {
        if (PlayerParticleFactory.Instance != null)
            PlayerParticleFactory.Instance.GetNewFootstepVfxInstance(_footstepSpawnTransform.position);
    }

    private void ToggleSlidingVfx(bool enableVfx)
    {
        EmissionModule emissionModule = _slidingParticleSystem.emission;
        emissionModule.enabled = enableVfx;
    }

    private void InitializeParticleSystems()
    {
        _particleSystems = new List<ParticleSystem>(_wallSlideVfx);
    }
    
    private void MovementController_OnPlayerWallslide()
    {
        ToggleAllEmissionModules(true);
    }

    private void MovementController_OffPlayerWallslide(bool a)
    {
        ToggleAllEmissionModules(false);
    }

    private void MovementController_StopPlayerWallslide()
    {
        ToggleAllEmissionModules(false);
    }

    private void ToggleAllEmissionModules(bool enable)
    {
        foreach (var particleSystem in _particleSystems)
        {
            var emissionModule = particleSystem.emission;
            emissionModule.enabled = enable;
        }
    }
}
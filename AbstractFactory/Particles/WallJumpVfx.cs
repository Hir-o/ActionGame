using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmissionModule = UnityEngine.ParticleSystem.EmissionModule;
public class WallJumpVfx : MonoBehaviour
{
    /*    [SerializeField] private ParticleSystem[] _slideVfx;

        *//*rivate WaitForSeconds _waitForSeconds;*/
    /*  [SerializeField] private float _waitDelay = 1.5f;*/
    /*    private EmissionModule _emissionModule;*//*
        private List<EmissionModule> _emissionModules;*/

    [SerializeField] private ParticleSystem[] _slideVfx;
    private List<ParticleSystem> _particleSystems;
    private void OnEnable()
    {

        MovementController.OnPlayerWallHang += MovementController_OnPlayerWallslide;
        MovementController.OnPlayerRun += MovementController_OffPlayerWallslide;
        MovementController.OnPlayerIdle += MovementController_OffPlayerWallslide;
        MovementController.OnPlayerJump += MovementController_StopPlayerWallslide;
        MovementController.OnPlayerSlide += MovementController_StopPlayerWallslide;
        MovementController.OnPlayerJumpFromJumpad += MovementController_StopPlayerWallslide;
        MovementController.OnPlayerDoubleJump += MovementController_StopPlayerWallslide;








    }


    private void OnDisable() {


        MovementController.OnPlayerWallHang -= MovementController_OnPlayerWallslide;
        MovementController.OnPlayerRun -= MovementController_OffPlayerWallslide;
        MovementController.OnPlayerIdle -= MovementController_OffPlayerWallslide;
        MovementController.OnPlayerJump -= MovementController_StopPlayerWallslide;
        MovementController.OnPlayerSlide -= MovementController_StopPlayerWallslide;
        MovementController.OnPlayerJumpFromJumpad -= MovementController_StopPlayerWallslide;
        MovementController.OnPlayerDoubleJump -= MovementController_StopPlayerWallslide;




    }



    private void Awake()
    {
        /*_emissionModule = _slideVfx.emission;
        _emissionModule.enabled = false;*/

        InitializeParticleSystems();
    }

    private void InitializeParticleSystems()
    {
        _particleSystems = new List<ParticleSystem>(_slideVfx);
    }
    private void MovementController_OnPlayerWallslide()
    {
        Debug.Log("slideVfxPlays");
        /*  _emissionModule.enabled = true;*/
        ToggleAllEmissionModules(true);

    }


    private void MovementController_OffPlayerWallslide(bool  a)
    {

        Debug.Log("OfffFlase");
        /*  _emissionModule.enabled = false;
  */
        ToggleAllEmissionModules(false);
    }


    private void MovementController_StopPlayerWallslide()
    {

        Debug.Log("JumpFlaseVfx");
        /*_emissionModule.enabled = false;*/
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

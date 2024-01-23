using System;
using System.Collections;
using System.Collections.Generic;
using Factory;
using UnityEngine;

public class DeathParticleEvent : MonoBehaviour
{
  


    private void OnEnable()
    {
        CharacterPlayerController.OnPlayerDiedVfx += PlayerRespawn_OnPlayerDeath;

      

    }
    private void OnDisable()
    {
        CharacterPlayerController.OnPlayerDiedVfx -= PlayerRespawn_OnPlayerDeath;

    }

    private void PlayerRespawn_OnPlayerDeath(DeathType deathType)
    {
        if (deathType == DeathType.HazardFogDeath)
        {
            PlayHazardDeathVfx();
        }
        else
        {
            PlayDeathVfx();
        }
        

     
    }


    private void PlayDeathVfx()
    {
        if (DeathParticleFactory.Instance != null)
            DeathParticleFactory.Instance.GetNewDeathVfxInstance(transform.position);
    }

    private void PlayHazardDeathVfx()
    {
        if (DeathParticleFactory.Instance != null)
            DeathParticleFactory.Instance.GetNewHazardDeathVfxInstance(transform.position);
    }


}

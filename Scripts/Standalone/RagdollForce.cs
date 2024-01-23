using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollForce : MonoBehaviour
{
    [SerializeField] private float _ragdollForceUp;
    [SerializeField] private float _ragdollForceSide;
   private Rigidbody[] _rigidbodies;
   private GameObject _player;
    private Renderer[] _renderers;
   



    private void Awake()
    {
        _rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        _player = MovementController.Instance.gameObject;
        _renderers = _player.GetComponentsInChildren<Renderer>();
       
       

    }

    private void OnEnable()
    { 
        PlayerRespawn.OnPlayerRespawn += EnablePlayer;
        AddForce();
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= EnablePlayer;
      
    }

    private void EnablePlayer()
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = true;
        }
        Destroy(gameObject);

    }

    private void AddForce()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.AddForce(transform.up * _ragdollForceUp);
            rigidbody.AddForce(transform.right * _ragdollForceSide);
        }
    }

   
}

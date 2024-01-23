using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballData : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystemArray;

    public ParticleSystem[] ParticleSystemArray => _particleSystemArray;
    public void ChangeGravityModifier(float newValue, float primaryValue = 0)
    {
        for (int i = 0; i < _particleSystemArray.Length; i++)
        {
            if (_particleSystemArray[i].gravityModifier != primaryValue)
                _particleSystemArray[i].gravityModifier = primaryValue;
            else
                _particleSystemArray[i].gravityModifier -= newValue;
        }   
    }

}

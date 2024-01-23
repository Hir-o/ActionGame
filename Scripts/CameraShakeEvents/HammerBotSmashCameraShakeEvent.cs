using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBotSmashCameraShakeEvent : MonoBehaviour
{
    private Renderer _renderer;
    void Start() => _renderer = GetComponent<Renderer>();
    
    private void OnEnable()
    {
        HammerBot.OnHammerSmash += HammerBot_OnHammerSmash;
    }

    private void OnDisable()
    {
        HammerBot.OnHammerSmash -= HammerBot_OnHammerSmash;
    }
    private void HammerBot_OnHammerSmash()
    {
        if (_renderer == null) return;
        if (_renderer.isVisible)
        {
            CameraShakeController.Instance.ShakeCamera(ShakeType.HammerEnemy);
        }
    }
}
    
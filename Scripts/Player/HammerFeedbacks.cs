using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class HammerFeedbacks : MonoBehaviour
{
    [SerializeField]private MMFeedbacks _hammerFeedback;

    private void OnEnable()
    { 
        HammerBot.OnHammerSmash += HammerBotOnOnHammerSmash;
    }
    private void OnDisable()
    {
        HammerBot.OnHammerSmash -= HammerBotOnOnHammerSmash;
    }
    private void HammerBotOnOnHammerSmash()
    {
        if (_hammerFeedback != null)
        {
            _hammerFeedback.PlayFeedbacks();
        }
        
    }
}

using NaughtyAttributes;
using UnityEngine;

public class BaseEnemyData : ScriptableObject
{
    [BoxGroup("Player Detection"), SerializeField]
    private float _triggerDistance = 20f;

    [BoxGroup("Player Detection"), SerializeField]
    private float _checkTriggerInterval = 0.5f;
    
    public float TriggerDistance => _triggerDistance;
    public float CheckTriggerInterval => _checkTriggerInterval;
}

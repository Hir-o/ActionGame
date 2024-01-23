using System;
using UnityEngine;

public class WarningTarget : MonoBehaviour
{
    public static Action<Transform> OnAnySendTransform;

    private void Start()
    {
        if (BaseBoss.Instance == null) return;
        if (BaseBoss.Instance is SecondBoss secondBoss)
            OnAnySendTransform?.Invoke(transform);
    }
}
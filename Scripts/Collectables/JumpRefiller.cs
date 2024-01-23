using System;
using UnityEngine;

public class JumpRefiller : BaseCollectable
{
    public static event Action OnAnyCollectJumpRefiller;

    private bool _isCollected;

    public bool IsCollected => _isCollected;

    protected override void OnEnable()
    {
        base.OnEnable();
        _isCollected = false;
    }
    
    public override void OnItemCollected()
    {
        base.OnItemCollected();
        _isCollected = true;
        OnAnyCollectJumpRefiller?.Invoke();
    }
}

using System;
using UnityEngine;

public class FlyingEquipmentCollectable : BaseCollectable
{
    public static event Action OnAnyCollectFlyingEquipment;
     
    private bool _isCollected;

    public bool IsCollected => _isCollected;

    protected override void OnEnable()
    {
        base.OnEnable();
        _isCollected = false;
        ObjectVisibilityCheck.OnAnyFlyingEquipmentCheck += CheckIfIsInCameraView;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ObjectVisibilityCheck.OnAnyFlyingEquipmentCheck -= CheckIfIsInCameraView;
    } 

    public override void OnItemCollected()
    {
        base.OnItemCollected();
        _isCollected = true;
        OnAnyCollectFlyingEquipment?.Invoke();
    }
}
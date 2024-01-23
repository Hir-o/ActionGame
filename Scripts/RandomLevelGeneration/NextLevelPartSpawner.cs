/using UnityEngine;

public class NextLevelPartSpawner : Invisibler, IUngrabbable
{
    private LevelPart _levelPart;

    private bool _isTriggered;

    protected override void Awake()
    {
        base.Awake();
        _levelPart = GetComponentInParent<LevelPart>();
    }
    
    private void OnDisable() => _isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (CharacterPlayerController.Instance.IsDead) return;
        if (_isTriggered) return;
        if (!other.TryGetComponent(out CharacterPlayerController player)) return;
        if (RandomLevelGenerator.Instance == null) return;
        RandomLevelGenerator.Instance.SpawnNewLevelPartAtPosition(_levelPart.transform.position.x);
        _isTriggered = true;
    }
}
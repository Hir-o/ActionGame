using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Stats/EnemyData", order = 3)]
public class EnemyData : BaseEnemyData, IMovingEnemy
{
    [BoxGroup("Stats"), SerializeField] private float _movementSpeed = 2f;
    [BoxGroup("Stats"), SerializeField] private float _waitDuration;
    
    [BoxGroup("Tweening"), SerializeField] private Ease _movementEase = Ease.OutQuad;

    public float MovementSpeed => _movementSpeed;
    public float WaitDuration => _waitDuration;
    public Ease MovementEase => _movementEase;
}

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Stats/LadderEnemyData", order = 5)]
public class LadderEnemyData : BaseEnemyData, ILadderEnemy
{

    [BoxGroup("Scale"), SerializeField] private float _movementSpeed = 5;
    [BoxGroup("Scale"), SerializeField] private float _waitDuration = 0.7f;
    [BoxGroup("Scale"), SerializeField] private Ease _movementEase = Ease.OutBounce;
    [BoxGroup("Scale"), SerializeField] private Ease _rotateEase = Ease.OutBounce;


    public float MovementSpeed { get => _movementSpeed; }
    public float WaitDuration { get => _waitDuration; }
    public Ease MovementEase { get => _movementEase; }
    public Ease RotateEase { get => _rotateEase; }
}
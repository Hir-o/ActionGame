using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Stats/DashingEnemyData", order = 6)]
public class DashingEnemyData : BaseEnemyData, IDashingEnemy
{

    [BoxGroup("Player Detection"), SerializeField] private float _dashingTriggerDistance = 20;

    [BoxGroup("Scale"), SerializeField] private float _movementSpeed = 5;
    [BoxGroup("Scale"), SerializeField] private float _dashingSpeed = 6f;
    [BoxGroup("Scale"), SerializeField] private Ease _movementEase = Ease.InOutCubic;
    [BoxGroup("Scale"), SerializeField] private Ease _dashingEase = Ease.OutExpo;
    
    public float DashingTriggerDistance { get => _dashingTriggerDistance; }

    public float MovementSpeed { get => _movementSpeed; }
    public float DashingSpeed { get => _dashingSpeed; }
    public Ease MovementEase { get => _movementEase; }
    public Ease DashingEase { get => _dashingEase; }
}
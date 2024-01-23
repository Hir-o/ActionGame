using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy Stats/ExpandingEnemyData", order = 4)]
public class ExpandingEnemyData : BaseEnemyData, IExpandingEnemy
{
    [BoxGroup("Scale"), SerializeField] private float _startScale = 0.5f;
    [BoxGroup("Scale"), SerializeField] private float _endScale = 3.5f;
    [BoxGroup("Scale"), SerializeField] private float _scaleSpeed = 5f;
    [BoxGroup("Scale"), SerializeField] private Ease _scaleEase = Ease.OutBounce;

    public float StartScale => _startScale;
    public float EndScale => _endScale;
    public float ScaleSpeed => _scaleSpeed;
    public Ease ScaleEase => _scaleEase;
}
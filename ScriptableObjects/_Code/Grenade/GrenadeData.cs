using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "PlayerProjectiles/Grenade", order = 1)]
public class GrenadeData : ScriptableObject
{
    [BoxGroup("Damage"), SerializeField] private int _damage = 10;

    [BoxGroup("Movement"), SerializeField] private float _moveSpeed = 3f;
    [BoxGroup("Movement"), SerializeField] private Ease _moveEase = Ease.Linear;

    [BoxGroup("Explosion Distance"), SerializeField]
    private float _minDistanceFromBoss = 0.5f;

    [BoxGroup("Explosion Vfx"), SerializeField]
    private GameObject _explosionVfx;

    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public Ease MoveEase => _moveEase;
    public float MinDistanceFromBoss => _minDistanceFromBoss;
    public GameObject ExplosionVfx => _explosionVfx;
}
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MissleData", menuName = "MissleData/BossMissle", order = 1)]
public class MissileData : ScriptableObject
{
    [BoxGroup("Damage"), SerializeField] private int _damage = 10;
    
    [BoxGroup("Movement"), SerializeField] private float _moveSpeed = 3f;
    [BoxGroup("Movement"), SerializeField] private Ease _moveEase = Ease.Linear;

    [BoxGroup("Movement Revert"), SerializeField] private float _moveSpeedReverse = 20f;
    [BoxGroup("Movement Revert"), SerializeField] private Ease _moveReverseEase = Ease.Linear;
    [BoxGroup("Movement Revert"), SerializeField] private float _revertDistance = 3f;

    [BoxGroup("Movement Launch"), SerializeField] private float _launchMoveSpeed = 75f;
    [BoxGroup("Movement Launch"), SerializeField] private float _minDistanceFromBoss = 0.5f;

    [BoxGroup("Spawn Vfx"), SerializeField] private GameObject _spawnVfx;
    
    [BoxGroup("Explosion Vfx"), SerializeField] private GameObject _explosionVfx;
    
    [BoxGroup("Rotation"), SerializeField] private Vector3 _fullRotation = new Vector3(0f, 180f, 809.999f);
    [BoxGroup("Rotation"), SerializeField] private float _fullRotateDuration = 1.8f;
    [BoxGroup("Rotation"), SerializeField] private Ease _fullRotateEase = Ease.OutBack;

    [BoxGroup("Thruster"), SerializeField] private Vector3 _thrusterChargingScale = new Vector3(1.2f, 1.2f, 1.2f);
    [BoxGroup("Thruster"), SerializeField] private Vector3 _thrusterReducedScale = new Vector3(.8f, .8f, .8f);
    [BoxGroup("Thruster"), SerializeField] private float _thrusterChargeDuration = .5f;
    [BoxGroup("Thruster"), SerializeField] private Ease _thrusterChargeEase = Ease.OutQuad;
    [BoxGroup("Thruster"), SerializeField] private float _thrusterChargeUpDelay = .25f;

    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public Ease MoveEase => _moveEase;
    public float MoveSpeedReverse => _moveSpeedReverse;
    public Ease MoveReverseEase => _moveReverseEase;
    public float RevertDistance => _revertDistance;
    public float LaunchMoveSpeed => _launchMoveSpeed;
    public float MinDistanceFromBoss => _minDistanceFromBoss;
    public GameObject SpawnVfx => _spawnVfx;
    public GameObject ExplosionVfx => _explosionVfx;
    public Vector3 FullRotation => _fullRotation;
    public float FullRotateDuration => _fullRotateDuration;
    public Ease FullRotateEase => _fullRotateEase;
    public Vector3 ThrusterChargingScale => _thrusterChargingScale;
    public Vector3 ThrusterReducedScale => _thrusterReducedScale;
    public float ThrusterChargeDuration => _thrusterChargeDuration;
    public Ease ThrusterChargeEase => _thrusterChargeEase;
    public float ThrusterChargeUpDelay => _thrusterChargeUpDelay;
}
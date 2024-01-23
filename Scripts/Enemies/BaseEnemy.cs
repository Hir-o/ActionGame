using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public event Action OnIsEnemyDestroyed;
    
    [BoxGroup("Graphic"), SerializeField] private Transform _enemyTransform;
    [BoxGroup("Stats"), SerializeField] private BaseEnemyData _baseEnemyData;

    [BoxGroup("Colliders"), SerializeField]
    private Collider[] _colliders;

    [BoxGroup("Waypoints"), SerializeField]
    private Transform[] _waypointTranformArray;

    [ShowIf("IsPatrolType"), SerializeField]
    private bool _patrol = true;

    public bool IsPatrolType
    {
        get { return this is IPatrol; }
    }

    private Tween _moveTween;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;
    private EnemyJumpPad _enemyJumpPad;
    private Vector3[] _waypointPositionArray;

    private bool _isDestroyed;

    #region Properties

    public bool CanPatrol => _patrol;

    public Transform EnemyTransform
    {
        get => _enemyTransform;
        set => _enemyTransform = value;
    }

    protected BaseEnemyData BaseEnemyData => _baseEnemyData;

    protected Tween MoveTween
    {
        get => _moveTween;
        set => _moveTween = value;
    }

    protected Coroutine Coroutine
    {
        get => _coroutine;
        set => _coroutine = value;
    }

    protected WaitForSeconds Wait => _wait;

    protected Vector3[] WaypointPositionArray => _waypointPositionArray;

    public bool IsDestroyed
    {
        get => _isDestroyed;
        set
        {
            _isDestroyed = value;
            ToggleColliders(!_isDestroyed);
            if (_isDestroyed) OnIsEnemyDestroyed?.Invoke();
        }
    }

    #endregion

    protected virtual void Awake()
    {
        _wait = new WaitForSeconds(_baseEnemyData.CheckTriggerInterval);
        if (_waypointTranformArray.Length == 0) return;
        _waypointPositionArray = new Vector3[_waypointTranformArray.Length];
        for (int i = 0; i < _waypointTranformArray.Length; i++)
            _waypointPositionArray[i] = _waypointTranformArray[i].position;

        _enemyJumpPad = GetComponentInChildren<EnemyJumpPad>();
    }

    protected virtual void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
        CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;
    }

    protected virtual void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
    }

    private void PlayerRespawn_OnPlayerRespawn()
    {
        UpdateCollider(true);
        Reset();
    }

    private void CharacterPlayerController_OnPlayerDied() => UpdateCollider(false);

    protected float GetXDistanceToPlayer()
    {
        Vector3 flatEnemyPosition = new Vector3(EnemyTransform.position.x, 0f, 0f);
        Vector3 flatPlayerPosition = new Vector3(MovementController.Instance.transform.position.x, 0f, 0f);
        return Vector3.Distance(flatEnemyPosition, flatPlayerPosition);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_isDestroyed) return;
        if (other.transform.root.gameObject.GetInstanceID() ==
            CharacterPlayerController.Instance.gameObject.GetInstanceID())
            CharacterPlayerController.Instance.Die();
    }

    public virtual void OnEnemyTriggerEnter(Collider other)
    {
        if (_isDestroyed) return;
        if (other.transform.root.gameObject.GetInstanceID() ==
            CharacterPlayerController.Instance.gameObject.GetInstanceID())
        {
            if (MovementController.Instance.IsSlidingOrSlowingFromSlide && _enemyJumpPad != null &&
                _enemyJumpPad.IsDestructable)
            {
                _enemyJumpPad.DestroyEnemy();
                return;
            }

            CharacterPlayerController.Instance.Die();
        }
    }

    private void UpdateCollider(bool isTrigger)
    {
        for (int i = 0; i < _colliders.Length; i++)
            _colliders[i].isTrigger = isTrigger;
    }

    private void ToggleColliders(bool enable)
    {
        for (int i = 0; i < _colliders.Length; i++)
            _colliders[i].enabled = enable;
    }

    protected abstract IEnumerator TryToTriggerTrap();

    protected abstract void Reset();
}
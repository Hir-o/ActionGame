using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEnemyGroup : BaseEnemy, IPatrol
{
    [SerializeField] private float _sequenceDelay = 0.33f;

    [SerializeField] private FireballData[] _fireBallDataArray;

    [SerializeField] private Transform[] _seqWaypointTransformArray;


    private Vector3[] _seqWaypointPositionArray;

    private Tween[] _moveTweenArray;

    protected override void Awake()
    {
        _fireBallDataArray = GetComponentsInChildren<FireballData>();

        if (_fireBallDataArray.Length == 0) return;
        _moveTweenArray = new Tween[_fireBallDataArray.Length];

        if (_seqWaypointTransformArray.Length == 0) return;
        _seqWaypointPositionArray = new Vector3[_seqWaypointTransformArray.Length];
        for (int i = 0; i < _seqWaypointTransformArray.Length; i++)
        {
            _seqWaypointPositionArray[i] = _seqWaypointTransformArray[i].position;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (CanPatrol)
            Coroutine = StartCoroutine(TryToTriggerTrap());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Coroutine != null) StopCoroutine(Coroutine);
        ResetPatrol();
    }

    protected override IEnumerator TryToTriggerTrap()
    {
        float distanceToPlayer;
        while (true)
        {
            yield return Wait;
            distanceToPlayer = GetXDistanceToPlayer();

            if (distanceToPlayer <= BaseEnemyData.TriggerDistance)
            {
                Patrol();
                if (Coroutine != null)
                    StopCoroutine(Coroutine);
                break;
            }
        }
    }

    public void Patrol()
    {
        if (_seqWaypointTransformArray == null) return;

        ResetPatrol();

        for (int i = 0; i < _fireBallDataArray.Length; i++)
        {
            _seqWaypointPositionArray[0] = new Vector3(_fireBallDataArray[i].transform.position.x, _seqWaypointTransformArray[0].position.y, _fireBallDataArray[i].transform.position.z);
            _seqWaypointPositionArray[1] = new Vector3(_fireBallDataArray[i].transform.position.x, _seqWaypointTransformArray[1].position.y, _fireBallDataArray[i].transform.position.z);

            if (BaseEnemyData is IMovingEnemy movingEnemy)
                _moveTweenArray[i] = _fireBallDataArray[i].transform.DOPath(_seqWaypointPositionArray, movingEnemy.MovementSpeed, PathType.Linear,
                        PathMode.Full3D, 10, Color.red)
                    .SetSpeedBased()
                    .SetDelay(_sequenceDelay * i)
                    .SetEase(movingEnemy.MovementEase)
                    .OnStepComplete(FireballDrop)
                    .SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void FireballDrop()
    {
        if (_fireBallDataArray.Length == 0) return;
        for (int i = 0; i < _fireBallDataArray.Length; i++)
        {
            _fireBallDataArray[i].ChangeGravityModifier(1.5f);
        }
    }

    public void ResetPatrol()
    {

        if (_fireBallDataArray.Length == 0) return;
        for (int i = 0; i < _fireBallDataArray.Length; i++)
        {
            if (_seqWaypointTransformArray == null) return;

            _seqWaypointPositionArray[0] = new Vector3(_fireBallDataArray[i].transform.position.x, _seqWaypointTransformArray[0].position.y, _fireBallDataArray[i].transform.position.z);
            _seqWaypointPositionArray[1] = new Vector3(_fireBallDataArray[i].transform.position.x, _seqWaypointTransformArray[1].position.y, _fireBallDataArray[i].transform.position.z);
            
            if (_moveTweenArray[i] != null) _moveTweenArray[i].Kill();
            _fireBallDataArray[i].transform.position = _seqWaypointPositionArray[0];

            _fireBallDataArray[i].ChangeGravityModifier(0);
        }
    }

    protected override void Reset()
    {
        ResetPatrol();
        if (CanPatrol)
            Coroutine = StartCoroutine(TryToTriggerTrap());
    }
}

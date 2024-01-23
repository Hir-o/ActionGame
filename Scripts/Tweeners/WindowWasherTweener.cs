using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowWasherTweener : MonoBehaviour
{
    [SerializeField] private bool _verticalMovement;
    [SerializeField] private bool _horizontalMovement;
    [SerializeField] private Transform _windowWasherRig;
    [SerializeField] private Transform _windowWasherCage;

    [BoxGroup("Vertical Movement"), SerializeField]
    private float _moveCageVerticalDistance;

    [BoxGroup("Vertical Movement"), SerializeField]
    private float _moveCageVerticalDuration;

    [BoxGroup("Vertical Movement"), SerializeField]
    private Ease _moveCageVerticalEase;

    [BoxGroup("Horizontal Movement"), SerializeField]
    private float _moveCageHorizontalDuration;

    [BoxGroup("Horizontal Movement"), SerializeField]
    private Ease _moveCageHorizontalEase;

    [BoxGroup("Waypoints"), SerializeField]
    private Transform[] _waypointTranformArray;

    private Vector3[] _waypointPositionArray;

    private Tween _cageTweener;
    private Tween _cageHorizontalTweener;

    private void Start()
    {
        if (_waypointTranformArray.Length == 0) return;
        _waypointPositionArray = new Vector3[_waypointTranformArray.Length];
        for (int i = 0; i < _waypointTranformArray.Length; i++)
            _waypointPositionArray[i] = _waypointTranformArray[i].position;

        _windowWasherRig.position = _waypointPositionArray[0];

        MoveCage();
    }

    private void MoveCage()
    {
        if (_verticalMovement)
        {
            if (_cageTweener != null) _cageTweener.Kill();
            _cageTweener = _windowWasherCage.DOLocalMoveY(_moveCageVerticalDistance, _moveCageVerticalDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(UpdateType.Fixed)
                .SetEase(_moveCageVerticalEase);
        }

        if (_waypointPositionArray == null) return;
        if (_horizontalMovement)
        {
            if (_cageHorizontalTweener != null) _cageHorizontalTweener.Kill();
            _cageHorizontalTweener = _windowWasherRig.DOPath(_waypointPositionArray, _moveCageHorizontalDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(UpdateType.Fixed)
                .SetEase(_moveCageHorizontalEase);
        }
    }
}
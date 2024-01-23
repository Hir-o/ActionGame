using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class FlyingHelicopter : MonoBehaviour
{
    [BoxGroup("Helicopter"), SerializeField]
    private Transform _helicopterTransform;

    [BoxGroup("Waypoints"), SerializeField]
    private Transform[] _waypointTransformArray;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 5f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease = Ease.OutSine;

    private Vector3[] _waypointPositionArray;

    private void Start()
    {
        _waypointPositionArray = new Vector3[_waypointTransformArray.Length];
        for (int i = 0; i < _waypointTransformArray.Length; i++)
            _waypointPositionArray[i] = _waypointTransformArray[i].localPosition;

        FollowWaypoints();
    }

    private void FollowWaypoints()
    {
        _helicopterTransform.DOLocalPath(_waypointPositionArray,
                _speed,
                PathType.CatmullRom,
                PathMode.Sidescroller2D,
                10,
                Color.green)
            .SetEase(_ease)
            .SetLoops(-1, LoopType.Restart)
            .SetSpeedBased()
            .SetUpdate(UpdateType.Fixed);
    }
}
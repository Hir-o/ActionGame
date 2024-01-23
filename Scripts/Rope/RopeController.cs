
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class RopeController : MonoBehaviour, IUngrabbable
{
    [SerializeField] private Transform _originTransform;
    [SerializeField] private Transform _targetTransform;

    [Range(2, 20)] [SerializeField] private int _segmentsCount = 10;

    [BoxGroup("Tweening"), SerializeField] private float _swingSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _swingEase;
    [BoxGroup("Tweening"), SerializeField] private Transform[] _waypointsArray;

    private Mesh _mesh;
    private MeshCollider _meshCollider;
    private LineRenderer _lineRenderer;
    private Vector3[] _waypointPositionArray;

    private void Awake()
    {
        _mesh = new Mesh();
        _meshCollider = GetComponent<MeshCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _segmentsCount;
    }

    private void Start()
    {
        _waypointPositionArray = new Vector3[_waypointsArray.Length];
        for (int i = 0; i < _waypointsArray.Length; i++)
            _waypointPositionArray[i] = _waypointsArray[i].localPosition;

        _targetTransform.localPosition = _waypointPositionArray[0];
        _lineRenderer.SetPosition(0, _originTransform.position);
        _lineRenderer.SetPosition(_segmentsCount - 1, _targetTransform.position);
        StartSwinging();
    }

    private void StartSwinging()
    {
        _targetTransform
            .DOLocalPath(_waypointPositionArray, _swingSpeed, PathType.CatmullRom, PathMode.Full3D, 10,
                Color.green)
            .SetSpeedBased()
            .SetEase(_swingEase)
            .SetLoops(-1, LoopType.Yoyo)
            .OnUpdate(UpdateLineRenderer);
    }

    /*private void Update()
    {
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            float distanceToThePlayerCharacter = Vector3.Distance(_lineRenderer.GetPosition(i),
                MovementController.Instance.transform.position);
            if (distanceToThePlayerCharacter < 1f) Debug.Log(i);
        }
    }*/

    private void UpdateLineRenderer()
    {
        /*float distance = Vector3.Distance(_originTransform.position, _targetTransform.position);

        for (int i = 1; i < _segmentsCount; i++)
        {
            _lineRenderer.SetPosition(i,
                new Vector3(_originTransform.position.x,
                    _originTransform.position.y - (distance / _segmentsCount) * i,
                    _originTransform.position.z));
        }*/
        _lineRenderer.SetPosition(_segmentsCount - 1, _targetTransform.position);
        GenerateMeshCollider();
    }

    private void GenerateMeshCollider()
    {
        _lineRenderer.BakeMesh(_mesh);
        _meshCollider.sharedMesh = _mesh;
    }
}
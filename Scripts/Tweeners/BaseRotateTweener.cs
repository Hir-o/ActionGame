
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class BaseRotateTweener : MonoBehaviour
{
    [SerializeField] public Transform _origin;
    [SerializeField] public float _rotationAngle = 20f;
    [SerializeField] public bool _rotateInOppositeDirection;
    [SerializeField] public float _speed = 5f;
    [SerializeField] public Ease _ease;
    [SerializeField] public float _delay;


    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    public int _rotationDirection = 1;

    public float RotationAngle => _rotationAngle;

    private void Awake() => _wait = new WaitForSeconds(_delay);

    private void OnDisable()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        DOTween.Kill(_origin);
    }

    private void Start()
    {
        if (_rotateInOppositeDirection) _rotationDirection *= -1;
        _coroutine = StartCoroutine(InitTweening());
    }

    private IEnumerator InitTweening()
    {
        yield return _wait;
        TweenRotation();
    }
    public abstract void TweenRotation();

}

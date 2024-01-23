using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CheckpointTweener : MonoBehaviour
{
    private Checkpoint _checkpoint;

    [BoxGroup("Hologram Tween"), SerializeField]
    private Transform _hologramTransform;

    [BoxGroup("Hologram Tween"), SerializeField]
    private float _duration;

    [BoxGroup("Hologram Tween"), SerializeField]
    private Ease _ease = Ease.OutQuad;

    [BoxGroup("Circle Tween"), SerializeField]
    private Transform _circleTransform;

    [BoxGroup("Circle Tween"), SerializeField]
    private Vector3 _circleDestination;

    [BoxGroup("Circle Tween"), SerializeField]
    private Vector3 _circleFinalScale;

    [BoxGroup("Color Tween"), SerializeField]
    private MeshRenderer _hologramOriginMeshRenderer;

    [BoxGroup("Color Tween"), SerializeField]
    private Color _startColor;

    [BoxGroup("Color Tween"), SerializeField]
    private Color _finalColor;

    private Tween _checkpointOpenTween;
    private Tween _circlePositionTween;
    private Tween _circleScaleTween;
    private Tween _hologramOriginColorTween;

    private void Awake()
    {
        _checkpoint = GetComponent<Checkpoint>();
        _hologramOriginMeshRenderer.material.DOColor(_startColor, 0f);
    }

    private void Start() => _hologramTransform.localScale = Vector3.zero;
    private void OnEnable() => _checkpoint.OnCheckpointReached += Checkpoint_OnCheckpointReached;
    private void OnDisable() => _checkpoint.OnCheckpointReached -= Checkpoint_OnCheckpointReached;

    private void Checkpoint_OnCheckpointReached()
    {
        KillTweens();
        _hologramTransform.localScale = new Vector3(1f, 1f, 0f);
        _checkpointOpenTween = _hologramTransform.DOScaleZ(1f, _duration).SetEase(_ease);
        _circlePositionTween = _circleTransform.DOLocalMove(_circleDestination, _duration).SetEase(_ease);
        _circleScaleTween = _circleTransform.DOScale(_circleFinalScale, _duration).SetEase(_ease);
        _hologramOriginColorTween = _hologramOriginMeshRenderer.material.DOColor(_finalColor, _duration).SetEase(_ease);
    }

    private void KillTweens()
    {
        if (_checkpointOpenTween != null && _checkpointOpenTween.IsPlaying()) _checkpointOpenTween.Kill();
        if (_circlePositionTween != null && _circlePositionTween.IsPlaying()) _circlePositionTween.Kill();
        if (_circleScaleTween != null && _circleScaleTween.IsPlaying()) _circleScaleTween.Kill();
        if (_hologramOriginColorTween != null && _hologramOriginColorTween.IsPlaying())
            _hologramOriginColorTween.Kill();
    }
}
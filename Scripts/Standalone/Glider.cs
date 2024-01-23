

using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

public class Glider : MonoBehaviour
{
    [SerializeField] private GameObject _gliderGameObject;

    [SerializeField] private Vector3 _startScale;
    private Vector3 _originalScale;

    [BoxGroup("Tweening"), SerializeField] private float _speed = 2f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease = Ease.OutCubic;

    private Tween _scaleTweener;

    private void Start()
    {
        Assert.IsNotNull(_gliderGameObject);
        _gliderGameObject.SetActive(false);
        _originalScale = _gliderGameObject.transform.localScale;
        _gliderGameObject.transform.localScale = _startScale;
    }

    private void OnEnable()
    {
        GlideAction.OnStartGliding += GlideAction_OnStartGliding;
        GlideAction.OnStopGliding += GlideAction_OnStopGliding;
    }

    private void OnDisable()
    {
        GlideAction.OnStartGliding -= GlideAction_OnStartGliding;
        GlideAction.OnStopGliding -= GlideAction_OnStopGliding;
    }

    private void GlideAction_OnStartGliding() => UpdateGlider(true);
    private void GlideAction_OnStopGliding() =>  UpdateGlider(false);

    private void UpdateGlider(bool isEnabled)
    {
        if (_scaleTweener != null) _scaleTweener.Kill();
        if (isEnabled)
        {
            _gliderGameObject.SetActive(true);
            _scaleTweener = _gliderGameObject.transform.DOScale(_originalScale, _speed).SetSpeedBased().SetEase(_ease);

        }
        else
        {
            _scaleTweener = _gliderGameObject.transform.DOScale(_startScale, _speed).SetSpeedBased().SetEase(_ease).OnComplete(() =>
            {
                _gliderGameObject.SetActive(false);
            });
        }

    }
}
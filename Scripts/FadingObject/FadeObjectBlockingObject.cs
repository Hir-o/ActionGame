
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

public class FadeObjectBlockingObject : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField, Range(0, 1f)] private float _fadedAlpha = 0.33f;
    [SerializeField] private bool _retainShadows = true;
    [SerializeField] private Vector3 _targetPositionOffset = Vector3.up;
    [SerializeField] private float _fadeSpeed = 1f;

    [BoxGroup("Read Only Data"), SerializeField]
    private List<FadingObject> _fadingObjectList = new List<FadingObject>();

    [BoxGroup("Read Only Data"), SerializeField]
    private Dictionary<FadingObject, Coroutine> _runningCoroutines = new Dictionary<FadingObject, Coroutine>();

    private Transform _mainCameraTransform;
    private RaycastHit[] _raycastHitArray = new RaycastHit[10];

    #region Shader Variables

    private readonly string _srcBlend = "_SrcBlend";
    private readonly string _dstBlend = "_DstBlend";
    private readonly string _zWrite = "_ZWrite";
    private readonly string _surface = "_Surface";
    private readonly string _depthOnly = "DepthOnly";
    private readonly string _shadowCaster = "SHADOWCASTER";
    private readonly string _renderType = "RenderType";
    private readonly string _transparent = "Transparent";
    private readonly string _opaque = "Opaque";
    private readonly string _surfaceTypeTransparent = "_SURFACE_TYPE_TRANSPARENT";
    private readonly string _alphaPremultiplyOn = "_ALPHAPREMULTIPLY_ON";
    private readonly string _color = "_Color";

    #endregion

    private void Awake() => _mainCameraTransform = Camera.main.transform;

    private void Start() => StartCoroutine(CheckForObjects());

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            Vector3 _targetPosition = transform.position + _targetPositionOffset;
            Vector3 _direction = (_targetPosition - _mainCameraTransform.position).normalized;
            float _distance = Vector3.Distance(_mainCameraTransform.position, _targetPosition);
            int hits = Physics.RaycastNonAlloc(
                _mainCameraTransform.position,
                _direction,
                _raycastHitArray,
                _distance,
                _layerMask);

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    FadingObject _fadingObject = GetFadingObjectFromHit(_raycastHitArray[i]);
                    if (_fadingObject != null && !_fadingObjectList.Contains(_fadingObject))
                    {
                        if (_runningCoroutines.ContainsKey(_fadingObject))
                        {
                            if (_runningCoroutines[_fadingObject] != null)
                            {
                                StopCoroutine(_runningCoroutines[_fadingObject]);
                            }

                            _runningCoroutines.Remove(_fadingObject);
                        }

                        _runningCoroutines.Add(_fadingObject, StartCoroutine(FadeObjectOut(_fadingObject)));
                        _fadingObjectList.Add(_fadingObject);
                    }
                }
            }

            FadeObjectsNoLongerBeingHit();
            ClearHits();
            yield return null;
        }
    }

    private void ClearHits() => Array.Clear(_raycastHitArray, 0, _raycastHitArray.Length);

    private IEnumerator FadeObjectOut(FadingObject fadingObject)
    {
        fadingObject.MaterialList.ForEach((material) =>
        {
            material.SetInt(_srcBlend, (int)BlendMode.SrcAlpha);
            material.SetInt(_dstBlend, (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt(_zWrite, 0);
            material.SetInt(_surface, 1);

            material.renderQueue = (int)RenderQueue.Transparent;

            material.SetShaderPassEnabled(_depthOnly, false);
            material.SetShaderPassEnabled(_shadowCaster, _retainShadows);

            material.SetOverrideTag(_renderType, _transparent);

            material.EnableKeyword(_surfaceTypeTransparent);
        });

        float time = 0f;

        while (fadingObject.MaterialList[0].color.a > _fadedAlpha)
        {
            fadingObject.MaterialList.ForEach((material) =>
            {
                if (!material.HasProperty(_color)) return;
                material.color = new Color(
                    material.color.r,
                    material.color.g,
                    material.color.b,
                    Mathf.Lerp(fadingObject.InitialAlpha, _fadedAlpha, time * _fadeSpeed)
                );
            });

            time += Time.deltaTime;
            yield return null;
        }

        if (_runningCoroutines.ContainsKey(fadingObject))
        {
            StopCoroutine(_runningCoroutines[fadingObject]);
            _runningCoroutines.Remove(fadingObject);
        }
    }

    private IEnumerator FadeObjectIn(FadingObject fadingObject)
    {
        float time = 0f;

        while (fadingObject.MaterialList[0].color.a < fadingObject.InitialAlpha)
        {
            fadingObject.MaterialList.ForEach((material) =>
            {
                if (!material.HasProperty(_color)) return;
                material.color = new Color(
                    material.color.r,
                    material.color.g,
                    material.color.b,
                    Mathf.Lerp(_fadedAlpha, fadingObject.InitialAlpha, time * _fadeSpeed)
                );
            });

            time += Time.deltaTime;
            yield return null;
        }

        fadingObject.MaterialList.ForEach((material) =>
        {
            material.SetInt(_srcBlend, (int)BlendMode.One);
            material.SetInt(_dstBlend, (int)BlendMode.Zero);
            material.SetInt(_zWrite, 1);
            material.SetInt(_surface, 0);

            material.renderQueue = (int)RenderQueue.Geometry;

            material.SetShaderPassEnabled(_depthOnly, true);
            material.SetShaderPassEnabled(_shadowCaster, true);

            material.SetOverrideTag(_renderType, _opaque);

            material.DisableKeyword(_surfaceTypeTransparent);
            material.DisableKeyword(_alphaPremultiplyOn);
        });

        if (_runningCoroutines.ContainsKey(fadingObject))
        {
            StopCoroutine(_runningCoroutines[fadingObject]);
            _runningCoroutines.Remove(fadingObject);
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<FadingObject> objectsToRemove = new List<FadingObject>(_fadingObjectList.Count);

        _fadingObjectList.ForEach((fadingObject) =>
        {
            bool isObjectBeingHit = false;
            for (int i = 0; i < _raycastHitArray.Length; i++)
            {
                FadingObject hitFadingObject = GetFadingObjectFromHit(_raycastHitArray[i]);
                if (hitFadingObject != null && fadingObject == hitFadingObject)
                {
                    isObjectBeingHit = true;
                    break;
                }
            }

            if (!isObjectBeingHit)
            {
                if (_runningCoroutines.ContainsKey(fadingObject))
                {
                    if (_runningCoroutines[fadingObject] != null)
                        StopCoroutine(_runningCoroutines[fadingObject]);
                    
                    _runningCoroutines.Remove(fadingObject);
                }
                
                _runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                objectsToRemove.Add(fadingObject);
            }
        });

        objectsToRemove.ForEach((fadingObjectToRemove) =>
        {
            _fadingObjectList.Remove(fadingObjectToRemove);
        });
    }

    private FadingObject GetFadingObjectFromHit(RaycastHit hit) =>
        hit.collider != null ? hit.collider.GetComponent<FadingObject>() : null;
}
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

public class CrumblingPlatformMaterialFader : MonoBehaviour
{
    [SerializeField] private List<Renderer> _rendererList = new List<Renderer>();
    [SerializeField] private List<Material> _materialList = new List<Material>();

    [BoxGroup("Fading"), SerializeField] private float _fadeSpeed = 3f;

    [BoxGroup("Enable"), SerializeField] private bool _isEnabled = true;

    private CrumblingPlatform _crumblingPlatform;
    private Coroutine _fadeCoroutine;

    private float _initialAlpha;

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

    private void Awake()
    {
        _crumblingPlatform = GetComponent<CrumblingPlatform>();
        
        if (!_isEnabled) return;
        
        if (_rendererList.Count == 0)
            _rendererList.AddRange(GetComponentsInChildren<Renderer>());

        _rendererList.ForEach((renderer) => { _materialList.AddRange(renderer.materials); });

        _initialAlpha = _materialList[0].color.a;
    }

    private void OnEnable() => _crumblingPlatform.OnCrumble += CrumblingPlatform_OnCrumble;
    private void OnDisable() => _crumblingPlatform.OnCrumble -= CrumblingPlatform_OnCrumble;

    private void CrumblingPlatform_OnCrumble() => _fadeCoroutine = StartCoroutine(FadeObject());

    private IEnumerator FadeObject()
    {
        if (!_isEnabled) yield break;
        for (int i = 0; i < _materialList.Count; i++)
        {
            _materialList[i].SetInt(_srcBlend, (int)BlendMode.SrcAlpha);
            _materialList[i].SetInt(_dstBlend, (int)BlendMode.OneMinusSrcAlpha);
            _materialList[i].SetInt(_zWrite, 0);
            _materialList[i].SetInt(_surface, 1);

            _materialList[i].renderQueue = (int)RenderQueue.Transparent;

            _materialList[i].SetShaderPassEnabled(_depthOnly, false);
            _materialList[i].SetShaderPassEnabled(_shadowCaster, true);

            _materialList[i].SetOverrideTag(_renderType, _transparent);

            _materialList[i].EnableKeyword(_surfaceTypeTransparent);
        }

        float time = 0f;

        while (_materialList[0].color.a > 0f)
        {
            _materialList.ForEach((material) =>
            {
                if (!material.HasProperty(_color)) return;
                material.color = new Color(
                    material.color.r,
                    material.color.g,
                    material.color.b,
                    Mathf.Lerp(_initialAlpha, 0f, time * _fadeSpeed)
                );
            });

            time += Time.deltaTime;
            yield return null;
        }
    }

    public void ResetMaterialProperties()
    {
        if (!_isEnabled) return;
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        while (_materialList[0].color.a < 1f)
        {
            _materialList.ForEach((material) =>
            {
                if (!material.HasProperty(_color)) return;
                material.color = new Color(
                    material.color.r,
                    material.color.g,
                    material.color.b,
                    1f
                );
            });
        }

        for (int i = 0; i < _materialList.Count; i++)
        {
            _materialList[i].SetInt(_srcBlend, (int)BlendMode.One);
            _materialList[i].SetInt(_dstBlend, (int)BlendMode.Zero);
            _materialList[i].SetInt(_zWrite, 1);
            _materialList[i].SetInt(_surface, 0);

            _materialList[i].renderQueue = (int)RenderQueue.Geometry;

            _materialList[i].SetShaderPassEnabled(_depthOnly, true);
            _materialList[i].SetShaderPassEnabled(_shadowCaster, true);

            _materialList[i].SetOverrideTag(_renderType, _opaque);

            _materialList[i].EnableKeyword(_surfaceTypeTransparent);
            _materialList[i].DisableKeyword(_alphaPremultiplyOn);
        }
    }
}
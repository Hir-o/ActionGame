using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class BossSmallExplosions : MonoBehaviour
{
    [BoxGroup("Delay"), SerializeField] private float _smallExplosionDelay = 0.4f;
    
    [SerializeField] private GameObject[] _explosionsArray;

    [SerializeField] private bool _detachFromGameObject;
    
    private WaitForSeconds _wait;
    private Transform _parentTransform;
    private Vector3 _startPosition;
    
    private bool _enabled;

    private void Awake()
    {
        _wait = new WaitForSeconds(_smallExplosionDelay);
        _parentTransform = transform.parent;
        _startPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        if (BaseBoss.Instance != null)
        {
            BaseBoss.Instance.OnHide += BaseBoss_OnHide;
            BaseBoss.Instance.OnTeleport += BaseBoss_OnTeleport;
        }
    }

    private void OnDisable()
    {
        if (BaseBoss.Instance != null)
        {
            BaseBoss.Instance.OnHide -= BaseBoss_OnHide;
            BaseBoss.Instance.OnTeleport -= BaseBoss_OnTeleport;
        } 
    }

    public void TriggerExplosions()
    {
        if (_enabled) return;
        _enabled = true;
        if (_detachFromGameObject)
            transform.parent = null;
        StartCoroutine(EnableExplosions());
    }

    private IEnumerator EnableExplosions()
    {
        for (int i = 0; i < _explosionsArray.Length; i++)
        {
            yield return _wait;
            _explosionsArray[i].SetActive(true);
        }
    }

    public void DisableExplosions()
    {
        for (int i = 0; i < _explosionsArray.Length; i++) _explosionsArray[i].SetActive(false);
        _enabled = false;
        transform.parent = _parentTransform;
        transform.localPosition = _startPosition;
    }

    public void BaseBoss_OnHide()
    {
        if (!_enabled) return;
        for (int i = 0; i < _explosionsArray.Length; i++) _explosionsArray[i].SetActive(false);
    }

    public void BaseBoss_OnTeleport()
    {
        if (!_enabled) return;
        for (int i = 0; i < _explosionsArray.Length; i++) _explosionsArray[i].SetActive(true);
    }
}
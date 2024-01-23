
using System.Collections;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class ObjectLeanDespawner : MonoBehaviour
{
    [SerializeField] private float _despawnTimer = 3f;

    [SerializeField] private bool _spawnNextPartice;

    [ShowIf("_spawnNextPartice"), SerializeField]
    private GameObject _nextParticle;

    private WaitForSeconds _wait;
    private Coroutine _despawnCoroutine;

    private void Awake() => _wait = new WaitForSeconds(_despawnTimer);
    private void OnEnable() => _despawnCoroutine = StartCoroutine(Despawn());

    private IEnumerator Despawn()
    {
        if (_despawnTimer == -1f) yield break;
        yield return _wait;
        _despawnCoroutine = null;
        LeanPool.Despawn(gameObject);
        SpawnNextParticle();
    }

    public void InstantDespawn()
    {
        if (_despawnCoroutine != null) StopCoroutine(_despawnCoroutine);
        LeanPool.Despawn(gameObject);
        SpawnNextParticle();
    }

    private void SpawnNextParticle()
    {
        if (!_spawnNextPartice || _nextParticle == null) return;
        LeanPool.Spawn(_nextParticle, transform.position, _nextParticle.transform.rotation);
    }
}
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class FlamethrowerTrap : BaseTrap
{
    [SerializeField] private GameObject _flamePrefab;

    protected override  void Awake()
    {
        base.Awake();
        _wait = new WaitForSeconds(_checkTriggerInterval);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _coroutine = StartCoroutine(TryToTriggerTrap());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_coroutine != null) StopCoroutine(_coroutine);
    }

    protected override IEnumerator TryToTriggerTrap()
    {
        float distanceXToPlayer;

        while (true)
        {
            yield return _wait;
            distanceXToPlayer = GetXDistanceToPlayer();

            if (distanceXToPlayer <= _triggerDistance)
            {
                StartTrap();

                break;
            }
        }
    }

    private void StartTrap()
    {
        _flamePrefab.SetActive(true);
    }

    private void RevertTrap()
    {
        _flamePrefab.SetActive(false);
    }

    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        RevertTrap();
    }
}
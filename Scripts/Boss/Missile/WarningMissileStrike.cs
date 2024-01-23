using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class WarningMissileStrike : MonoBehaviour
{
    [BoxGroup("Missile"), SerializeField] private GameObject _warningMissile;
    [BoxGroup("Points"), SerializeField] private List<Transform> targetList = new List<Transform>();
    [BoxGroup("Movement"), SerializeField] private float _missileSpeed;

    [BoxGroup("Firing Missiles"), SerializeField]
    private float _missileFiringDelay = 0.4f;

    private bool _firedWarningMissiles;

    private WaitForSeconds _waitMissileFiringDelay;

    private void Awake() => _waitMissileFiringDelay = new WaitForSeconds(_missileFiringDelay);

    private void OnEnable() => WarningTarget.OnAnySendTransform += WarningTarget_OnAnySendTransform;

    private void OnDisable() => WarningTarget.OnAnySendTransform -= WarningTarget_OnAnySendTransform;

    private void WarningTarget_OnAnySendTransform(Transform transform) => targetList.Add(transform);

    public void OnFireMissiles()
    {
        if (_firedWarningMissiles) return;
        if (targetList.Count == 0) return;
        StartCoroutine(FireWarningMissiles());  
    } 

    private IEnumerator FireWarningMissiles()
    {
        _firedWarningMissiles = true;
        foreach (Transform targetTransform in targetList)
        {
            GameObject missile =
                LeanPool.Spawn(_warningMissile, transform.position, _warningMissile.transform.rotation);

            Vector3 direction = (missile.transform.position - targetTransform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            missile.transform.rotation = lookRotation;

            missile.transform.DOMove(targetTransform.position, _missileSpeed)
                .SetSpeedBased()
                .SetEase(Ease.Linear)
                .OnComplete(() => { Debug.Log("Boom"); });

            yield return _waitMissileFiringDelay;
        }
    }
}
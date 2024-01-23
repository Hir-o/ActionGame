using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class LadderLaserTrap : BaseTrap
{
    [SerializeField] private Hovl_Laser _hovlLaser;

    [SerializeField] private float _startLaserLength = 0;
    [SerializeField] private float _maxLaserLength = 10;

    [SerializeField] private float _startLaserDelay = 5;

    [SerializeField] private float _laserProjectionDelay = 2;

    private Coroutine _laserProjectionCoroutine;

    protected virtual void Awake()
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
        float distanceYToPlayer;
        while (true)
        {
            yield return _wait;
            distanceXToPlayer = GetXDistanceToPlayer();
            distanceYToPlayer = GetYDistanceToPlayer();

            if (distanceXToPlayer <= _triggerDistance)
            {
                StartTrap();
                if (distanceYToPlayer <= _startLaserDelay)
                {
                    _laserProjectionCoroutine = StartCoroutine(LaserProjectionCR());
                    break;
                }
            }
        }
    }

    private void StartTrap()
    {
        _hovlLaser.gameObject.SetActive(true);
    }

    private void RevertTrap()
    {
        _hovlLaser.gameObject.SetActive(false);
    }


    private IEnumerator LaserProjectionCR()
    {
        while (true)
        {
            if (_hovlLaser.MaxLength == _startLaserLength)
                _hovlLaser.MaxLength = _maxLaserLength;
            else if (_hovlLaser.MaxLength == _maxLaserLength)
                _hovlLaser.MaxLength = _startLaserLength;
            else
                break;
            
            yield return new WaitForSeconds(_laserProjectionDelay);
        }
    }
    private float GetYDistanceToPlayer()
    {
        Vector3 flatEnemyPosition = new Vector3(0f, transform.position.y, 0f);
        Vector3 flatPlayerPosition = new Vector3(0f, MovementController.Instance.transform.position.y, 0f);
        return Vector3.Distance(flatEnemyPosition, flatPlayerPosition);
    }
    protected override void Reset()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(TryToTriggerTrap());
        if (_laserProjectionCoroutine != null) StopCoroutine(_laserProjectionCoroutine);
        RevertTrap();
    }
}

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class DestructablePlatform : MonoBehaviour
{
    [SerializeField] private Collider[] _colliderArray;

    [BoxGroup("Destruction"), SerializeField]
    private float _destructTime = .5f;

    [BoxGroup("Destruction"), SerializeField]
    private float _destructDelay = .25f;

    [BoxGroup("AnimationClipData"), SerializeField]
    private AnimationClipData _idleClipData;
    
    [BoxGroup("AnimationClipData"), SerializeField]
    private AnimationClipData _crumbleClipData;

    private Animator _animator;
    private Tween _destructTween;

    private bool _isDestructing;

    private void Awake()
    {
        _colliderArray = GetComponents<Collider>();
        _animator = GetComponent<Animator>();
    } 

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
    }

    private void PlayerRespawn_OnPlayerRespawn() => Reset();

    private void OnTriggerEnter(Collider other)
    {
        if (_isDestructing) return;
        if (other.TryGetComponent(out MovementController _))
        {
            StartDestructing();
            _isDestructing = true;
        }
    }

    private void StartDestructing()
    {
        if (_destructTween != null) _destructTween.Kill();
        /*_destructTween = DOVirtual.Float(1f, 0f, _destructTime, value => { })
            .OnComplete(() => Destruct())
            .SetDelay(_destructDelay);*/
        _animator.CrossFadeInFixedTime(_crumbleClipData.AnimId, 0f);
    }

    public void Destruct()
    {
        for (int i = 0; i < _colliderArray.Length; i++)
            _colliderArray[i].enabled = false;
    }

    private void Reset()
    {
        if (_destructTween != null) _destructTween.Kill();
        for (int i = 0; i < _colliderArray.Length; i++)
            _colliderArray[i].enabled = true;
        _isDestructing = false;
        _animator.CrossFadeInFixedTime(_idleClipData.AnimId, 0f);
    }
}
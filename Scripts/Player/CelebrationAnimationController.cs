

using System;
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class CelebrationAnimationController : SceneSingleton<CelebrationAnimationController>
{
    public event Action OnCelebrationFinished;

    [BoxGroup("Graphics"), SerializeField] private GameObject _graphics;

    [BoxGroup("Idle Animation"), SerializeField]
    private AnimationClipData _idleAnimationClipData;

    [BoxGroup("Celebrate Animations"), SerializeField]
    private CelebrationAnimationClipData[] _animationClipDataArray;

    [SerializeField] private float _delay = 0.15f;
    [SerializeField] private float _finishDelay = 0.15f;

    private Animator _animator;
    
    private int index = 0;
    private bool _playedAnimation;

    private void Awake()
    {
        _graphics.SetActive(false);
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() => MovementController.Instance.OnPlayerFinish += MovementController_OnPlayerFinish;

    private void MovementController_OnPlayerFinish()
    {
        if (_playedAnimation) return;
        _graphics.SetActive(true);
        CelebrationAnimationClipData celebrationClip = GetRandomCelebrateAnimation();
        PlayAnimation(celebrationClip);
        MovementController.Instance.OnPlayerFinish -= MovementController_OnPlayerFinish;
    }

    private CelebrationAnimationClipData GetRandomCelebrateAnimation()
    {
        int randomAnimationIndex = Random.Range(0, _animationClipDataArray.Length);
        return _animationClipDataArray[randomAnimationIndex];
    }

    private void PlayAnimation(CelebrationAnimationClipData clipData)
    {
        _playedAnimation = true;
        DOVirtual.Float(1f, 0f, _delay, value => { }).OnComplete(() =>
        {
            _animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
            float animationCompletionTime = clipData.Animation.length;
            DOVirtual.Float(animationCompletionTime, 0f, animationCompletionTime, value => { })
                .OnComplete(() =>
                {
                    if (clipData.AllowTransitionToOtherAnimationState)
                    {
                        _animator.CrossFadeInFixedTime(_idleAnimationClipData.AnimId,
                            _idleAnimationClipData.AdditionalTransitionTime);
                    }

                    OnCelebrationFinished?.Invoke();
                }).SetDelay(_finishDelay);
        });
    }

    public void PlayAnimation()
    {
        CelebrationAnimationClipData clipData = _animationClipDataArray[index];
        index++;
        if (index >= _animationClipDataArray.Length) index = 0;
        _animator.CrossFadeInFixedTime(clipData.AnimId, clipData.AdditionalTransitionTime);
        float animationCompletionTime = clipData.Animation.length;
        DOVirtual.Float(animationCompletionTime, 0f, animationCompletionTime, value => { })
            .OnComplete(() =>
            {
                if (clipData.AllowTransitionToOtherAnimationState)
                {
                    _animator.CrossFadeInFixedTime(_idleAnimationClipData.AnimId,
                        _idleAnimationClipData.AdditionalTransitionTime);
                }
            });
    }
}
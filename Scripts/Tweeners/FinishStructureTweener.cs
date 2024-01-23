
using System;
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;

public class FinishStructureTweener : SceneSingleton<FinishStructureTweener>
{
    public event Action OnWindowsOpen;

    [BoxGroup("Window Transform"), SerializeField]
    private Transform _leftWindowTransform;

    [BoxGroup("Window Transform"), SerializeField]
    private Transform _rightWindowTransform;

    [BoxGroup("Window X Start"), SerializeField]
    private float _leftWindowXInitPos = -0.2f;

    [BoxGroup("Window X Start"), SerializeField]
    private float _rightWindowXInitPos = 0.2f;

    [BoxGroup("Window X Destination"), SerializeField]
    private float _leftWindowXDestinationPos = -0.4f;

    [BoxGroup("Window X Destination"), SerializeField]
    private float _rightWindowXDestinationPos = 0.4f;

    [BoxGroup("Tweening"), SerializeField] private float _duration = 0.5f;
    [BoxGroup("Tweening"), SerializeField] private float _delay = 0.3f;
    [BoxGroup("Tweening"), SerializeField] private Ease _ease = Ease.OutSine;

    private void OnEnable() => LevelFinish.OnAnyLevelCompleted += LevelFinish_OnAnyLevelCompleted;
    private void OnDisable() => LevelFinish.OnAnyLevelCompleted -= LevelFinish_OnAnyLevelCompleted;

    private void Start()
    {
        Vector3 initLeftWindowPos = _leftWindowTransform.transform.localPosition;
        initLeftWindowPos.x = _leftWindowXInitPos;

        Vector3 initRightWindowPos = _rightWindowTransform.transform.localPosition;
        initRightWindowPos.x = _rightWindowXInitPos;

        _leftWindowTransform.localPosition = initLeftWindowPos;
        _rightWindowTransform.localPosition = initRightWindowPos;
    }

    private void LevelFinish_OnAnyLevelCompleted() => TweenWindows();

    private void TweenWindows()
    {
        _leftWindowTransform.DOLocalMoveX(_leftWindowXDestinationPos, _duration).SetEase(_ease).SetDelay(_delay);
        _rightWindowTransform.DOLocalMoveX(_rightWindowXDestinationPos, _duration).SetEase(_ease).SetDelay(_delay);
        OnWindowsOpen?.Invoke();
    }
}
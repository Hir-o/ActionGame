
using NaughtyAttributes;
using UnityEngine;

public class LevelButtonsTweener : MonoBehaviour
{
    [BoxGroup("Tween Data"), SerializeField]
    private LevelButtonTween[] _levelButtonTweenArray;

    private void Awake() => Reset();

    public void AnimateButtons()
    {
        for (int i = 0; i < _levelButtonTweenArray.Length; i++)
            _levelButtonTweenArray[i].PlayAnimation(i);
    }

    public void Reset()
    {
        for (int i = 0; i < _levelButtonTweenArray.Length; i++)
            _levelButtonTweenArray[i].Reset();
    }

    [Button("TestTweening")]
    public void Test()
    {
        Reset();
        AnimateButtons();
    }
}
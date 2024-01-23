using UnityEngine;
using MoreMountains.Feedbacks;

public class EnemyFeedbacks : MonoBehaviour
{
    [SerializeField] private MMFeedbacks _enemyfeedback;

    private void OnEnable() => EnemyJumpPad.OnAnyEnemyDestroyed += EnemyJumpPadOnOnAnyEnemyDestroyed;

    private void OnDisable() => EnemyJumpPad.OnAnyEnemyDestroyed -= EnemyJumpPadOnOnAnyEnemyDestroyed;

    private void EnemyJumpPadOnOnAnyEnemyDestroyed()
    {
        if (_enemyfeedback != null)
            _enemyfeedback.PlayFeedbacks();
    }
}
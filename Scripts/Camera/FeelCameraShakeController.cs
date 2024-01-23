using Leon;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using UnityEngine;

public class FeelCameraShakeController : SceneSingleton<FeelCameraShakeController>
{
    [BoxGroup("Camera Shake Feedbacks"), SerializeField] private MMFeedbacks _jumpFromJumpadCameraShakeFeedback;
    [BoxGroup("Camera Shake Feedbacks"), SerializeField] private MMFeedbacks _enemyDestroyCameraShakeFeedback;
    [BoxGroup("Camera Shake Feedbacks"), SerializeField] private MMFeedbacks _slidingCameraShakeFeedback;

    private bool _isPlayingSlidingCameraShake;
    
    private void OnEnable()
    {
        if (_jumpFromJumpadCameraShakeFeedback != null)
            MovementController.OnPlayerJumpFromJumpad += MovementController_OnPlayerJumpFromJumpad;
        
        if (_enemyDestroyCameraShakeFeedback != null)
            EnemyJumpPad.OnAnyEnemyDestroyed += EnemyJumpPad_OnAnyEnemyDestroyed;

        if (_slidingCameraShakeFeedback != null)
        {
            MovementController.OnPlayerSlide += MovementController_OnPlayerSlide;
            MovementController.OnPlayerSlideCancel += MovementController_OnPlayerSlideCancel;
        }
    }

    private void OnDisable()
    {
        if (_jumpFromJumpadCameraShakeFeedback != null)
            MovementController.OnPlayerJumpFromJumpad -= MovementController_OnPlayerJumpFromJumpad;
        
        if (_enemyDestroyCameraShakeFeedback != null)
            EnemyJumpPad.OnAnyEnemyDestroyed -= EnemyJumpPad_OnAnyEnemyDestroyed;
        
        if (_slidingCameraShakeFeedback != null)
        {
            MovementController.OnPlayerSlide -= MovementController_OnPlayerSlide;
            MovementController.OnPlayerSlideCancel -= MovementController_OnPlayerSlideCancel;
        }
    }
    
    private void MovementController_OnPlayerJumpFromJumpad() => _jumpFromJumpadCameraShakeFeedback.PlayFeedbacks();
    private void EnemyJumpPad_OnAnyEnemyDestroyed() => _enemyDestroyCameraShakeFeedback.PlayFeedbacks();

    private void MovementController_OnPlayerSlide()
    {
        if (_isPlayingSlidingCameraShake) return;
        _slidingCameraShakeFeedback.PlayFeedbacks();
        _isPlayingSlidingCameraShake = true;
    }

    private void MovementController_OnPlayerSlideCancel(bool isMovementPressed)
    {
        if (!_isPlayingSlidingCameraShake) return;
        _slidingCameraShakeFeedback.StopFeedbacks();
        _isPlayingSlidingCameraShake = false;
    }
}
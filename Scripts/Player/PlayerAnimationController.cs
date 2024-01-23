using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Leon
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Animations"), SerializeField] private AnimationClipData _clipIdle;
        [SerializeField] private AnimationClipData _clipRunning;
        [SerializeField] private AnimationClipData[] _clipsJumping;
        [SerializeField] private AnimationClipData _clipDoubleJump;
        [SerializeField] private AnimationClipData _clipJumpingFromJumpad;
        [SerializeField] private AnimationClipData _clipFalling;
        [SerializeField] private AnimationClipData _clipSliding;
        [SerializeField] private AnimationClipData _clipHanging;
        [SerializeField] private AnimationClipData _clipLegdeClimb;
        [SerializeField] private AnimationClipData _clipRopeHang;
        [SerializeField] private AnimationClipData _clipRopeHangBackward;
        [SerializeField] private AnimationClipData _clipDie;
        [SerializeField] private AnimationClipData _clipClimb;
        [SerializeField] private AnimationClipData _clipFly;
        [SerializeField] private AnimationClipData _clipFlyAscend;
        [SerializeField] private AnimationClipData _clipFlyDescend;
        [SerializeField] private AnimationClipData _clipSwimming;
        [SerializeField] private AnimationClipData _clipSwimmingAscend;
        [SerializeField] private AnimationClipData _clipSwimmingDescend;
        [SerializeField] private AnimationClipData _clipGliding;

        [Header("Transition Time"), SerializeField]
        private float _transitionTime = 0.3f;

        [Header("Double Jump to Gliding Transition Delay"), SerializeField]
        private float _doubleJumpToGlideTimer = 0.3f;

        private Animator _animator;
        private Coroutine _glidingAimationCoroutine;
        private WaitForSeconds _waitEndJump;
        private WaitForSeconds _waitGlide;
        private Tween _tweenEndJump;
        private Tween _tweenEndDoubleJump;

        private int _doubleJumpHash;
        private float _endJumpAnimationLength;
        private float _transitionBonusTime;
        private float _currAnimationSpeed;
        private float _endJumpTimer;

        private readonly float _jumpTimerFactor = 0.8f;

        private bool _isPlayingJumpAnimation;

        private int _currJumpIndex;

        private int _jumpIndex
        {
            get
            {
                if (_currJumpIndex >= _clipsJumping.Length)
                    _currJumpIndex = 0;
                return _currJumpIndex;
            }
            set => _currJumpIndex = value;
        }

        private float _totalTransitionTime => _transitionTime + _transitionBonusTime;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _waitGlide = new WaitForSeconds(_doubleJumpToGlideTimer);
            _doubleJumpHash = Animator.StringToHash(_clipDoubleJump.Animation.name);
        }

        private void OnEnable()
        {
            MovementController.OnPlayerIdle += PlayIdleAnimation;
            MovementController.OnPlayerRun += PlayRunningAnimation;
            MovementController.OnPlayerJump += PlayJumpAnimation;
            MovementController.OnPlayerDoubleJump += PlayDoubleJumpAnimation;
            MovementController.OnPlayerJumpCancel += EndJumpAnimation;
            MovementController.OnPlayerJumpFromJumpad += PlayJumpadJumpAnimation;
            MovementController.OnPlayerSlide += PlaySlideAnimation;
            MovementController.OnPlayerSlideCancel += EndSlideAnimation;
            MovementController.OnPlayerClimbLedge += PlayLedgeClimbAnimation;
            MovementController.OnPlayerFall += PlayFallAnimation;
            MovementController.OnPlayerFallCancel += EndFallAnimation;
            MovementController.OnPlayerWallHang += PlayHangingAnimation;

            CharacterPlayerController.OnPlayerDied += PlayDieAnimation;
            ClimbAction.OnPlayerClimb += PlayClimbingAnimation;
            ClimbAction.OnPlayerClimbCancel += EndClimbingAnimation;
            HaltAction.OnPlayerHalt += HaltAction_OnPlayerHalt;
            HaltAction.OnPlayerStopHalt += HaltAction_OnPlayerStopHalt;

            FlyAction.OnPlayerFly += FlyAction_OnPlayerFly;
            FlyAction.OnPlayerFlyCancel += FlyAction_OnPlayerFlyCancel;
            FlyAction.OnAnyFlyAscending += FlyAction_OnAnyFlyAscending;
            FlyAction.OnAnyStopFlyAscending += FlyAction_OnAnyStopFlyAscendingOrDescending;
            FlyAction.OnAnyFlyDescending += FlyAction_OnAnyFlyDescending;
            FlyAction.OnAnyStopFlyDescending += FlyAction_OnAnyStopFlyAscendingOrDescending;

            SwimAction.OnPlayerSwim += SwimAction_OnPlayerSwim;
            SwimAction.OnPlayerSwimCancel += SwimAction_OnPlayerSwimCancel;
            SwimAction.OnAnySwimAscending += SwimAction_OnAnySwimAscending;
            SwimAction.OnAnyStopSwimAscending += SwimAction_OnAnyStopSwimAscendingOrDescending;
            SwimAction.OnAnySwimDescending += SwimAction_OnAnySwimDescending;
            SwimAction.OnAnyStopSwimDescending += SwimAction_OnAnyStopSwimAscendingOrDescending;

            GlideAction.OnStartGliding += GlideAction_OnStartGliding;
            GlideAction.OnStopGliding += GlideAction_OnStopGliding;
            RopeHangAction.OnAnyGrabRope += RopeHangAction_OnAnyGrabRope;
            RopeHangAction.OnAnyReleaseRope += RopeHangAction_OnAnyReleaseRope;
            PlayerRespawn.OnPlayerRespawn += PlayerRespawn_OnPlayerRespawn;

            PlayerIdleState.OnPlayerIdle += PlayIdleAnimation;
            PlayerRunState.OnPlayerRun += PlayRunningAnimation;
            PlayerJumpState.OnPlayerJump += PlayJumpAnimation;
            PlayerJumpState.OnPlayerJumpCancel += EndJumpAnimation;
        }

        private void OnDisable()
        {
            PlayerIdleState.OnPlayerIdle -= PlayIdleAnimation;
            PlayerRunState.OnPlayerRun -= PlayRunningAnimation;
            PlayerJumpState.OnPlayerJump -= PlayJumpAnimation;
            PlayerJumpState.OnPlayerJumpCancel -= EndJumpAnimation;

            MovementController.OnPlayerIdle -= PlayIdleAnimation;
            MovementController.OnPlayerRun -= PlayRunningAnimation;
            MovementController.OnPlayerJump -= PlayJumpAnimation;
            MovementController.OnPlayerDoubleJump -= PlayDoubleJumpAnimation;
            MovementController.OnPlayerJumpCancel -= EndJumpAnimation;
            MovementController.OnPlayerJumpFromJumpad -= PlayJumpadJumpAnimation;
            MovementController.OnPlayerSlide -= PlaySlideAnimation;
            MovementController.OnPlayerSlideCancel -= EndSlideAnimation;
            MovementController.OnPlayerClimbLedge -= PlayLedgeClimbAnimation;
            MovementController.OnPlayerFall -= PlayFallAnimation;
            MovementController.OnPlayerFallCancel -= EndFallAnimation;
            MovementController.OnPlayerWallHang -= PlayHangingAnimation;

            CharacterPlayerController.OnPlayerDied -= PlayDieAnimation;
            ClimbAction.OnPlayerClimb -= PlayClimbingAnimation;
            ClimbAction.OnPlayerClimbCancel -= EndClimbingAnimation;
            HaltAction.OnPlayerHalt -= HaltAction_OnPlayerHalt;
            HaltAction.OnPlayerStopHalt -= HaltAction_OnPlayerStopHalt;

            FlyAction.OnPlayerFly -= FlyAction_OnPlayerFly;
            FlyAction.OnPlayerFlyCancel -= FlyAction_OnPlayerFlyCancel;
            FlyAction.OnAnyFlyAscending -= FlyAction_OnAnyFlyAscending;
            FlyAction.OnAnyStopFlyAscending -= FlyAction_OnAnyStopFlyAscendingOrDescending;
            FlyAction.OnAnyFlyDescending -= FlyAction_OnAnyFlyDescending;
            FlyAction.OnAnyStopFlyDescending -= FlyAction_OnAnyStopFlyAscendingOrDescending;

            SwimAction.OnPlayerSwim -= SwimAction_OnPlayerSwim;
            SwimAction.OnPlayerSwimCancel -= SwimAction_OnPlayerSwimCancel;
            SwimAction.OnAnySwimAscending -= SwimAction_OnAnySwimAscending;
            SwimAction.OnAnyStopSwimAscending -= SwimAction_OnAnyStopSwimAscendingOrDescending;
            SwimAction.OnAnySwimDescending -= SwimAction_OnAnySwimDescending;
            SwimAction.OnAnyStopSwimDescending -= SwimAction_OnAnyStopSwimAscendingOrDescending;

            GlideAction.OnStartGliding -= GlideAction_OnStartGliding;
            GlideAction.OnStopGliding -= GlideAction_OnStopGliding;
            RopeHangAction.OnAnyGrabRope -= RopeHangAction_OnAnyGrabRope;
            RopeHangAction.OnAnyReleaseRope -= RopeHangAction_OnAnyReleaseRope;
            PlayerRespawn.OnPlayerRespawn -= PlayerRespawn_OnPlayerRespawn;
        }

        private void PlayerRespawn_OnPlayerRespawn() => ResetAnPlayIdleAnimation();

        private void PlayIdleAnimation(bool isMovementPressed)
        {
            if (isMovementPressed) return;
            if (_clipSliding.IsPlayingAnimation) return;
            _clipIdle.IsPlayingAnimation = true;
            PlayAnimation(_clipIdle);
        }

        private void PlayRunningAnimation(bool isMovementPressed)
        {
            if (MovementController.Instance == null) return;
            if (MovementController.Instance.DisableFunctionality) return;
            if (!isMovementPressed) return;
            if (_clipSliding.IsPlayingAnimation) return;
            if (_clipRunning.IsPlayingAnimation) return;
            PlayAnimation(_clipRunning);
            _clipRunning.IsPlayingAnimation = true;
        }

        private void PlayJumpAnimation()
        {
            if (_isPlayingJumpAnimation) return;
            if (_tweenEndJump != null) _tweenEndJump.Kill();
            if (_tweenEndDoubleJump != null) _tweenEndDoubleJump.Kill();
            _isPlayingJumpAnimation = true;
            AnimationClipData clipData = GetRandomJumpAnimation();
            _jumpIndex++;
            PlayAnimation(clipData);
            _tweenEndJump = DOVirtual.Float(1f, 0f, _endJumpTimer, value => { }).OnComplete(() =>
            {
                EndJumpAnimation(true);
                PlayFallAnimation();
            });
        }

        private void PlayDoubleJumpAnimation()
        {
            if (_tweenEndJump != null) _tweenEndJump.Kill();
            if (_tweenEndDoubleJump != null) _tweenEndDoubleJump.Kill();
            _clipDoubleJump.IsPlayingAnimation = true;
            PlayAnimation(_clipDoubleJump);
            float _endDoubleJumpTimer = _clipDoubleJump.Animation.length * _jumpTimerFactor;
            _tweenEndDoubleJump = DOVirtual.Float(1f, 0f, _endDoubleJumpTimer, value => { }).OnComplete(() =>
            {
                EndJumpAnimation(true);
                PlayFallAnimation();
            });
        }

        private void PlayJumpadJumpAnimation()
        {
            if (_isPlayingJumpAnimation) return;
            if (_tweenEndJump != null) _tweenEndJump.Kill();
            if (_tweenEndDoubleJump != null) _tweenEndDoubleJump.Kill();
            _isPlayingJumpAnimation = true;
            _clipJumpingFromJumpad.IsPlayingAnimation = true;
            PlayAnimation(_clipJumpingFromJumpad);
            float endJumpTimer = _clipJumpingFromJumpad.Animation.length * _jumpTimerFactor;
            _tweenEndJump = DOVirtual.Float(1f, 0f, endJumpTimer, value => { }).OnComplete(() =>
            {
                EndJumpAnimation(true);
                PlayFallAnimation();
            });
        }

        private void EndJumpAnimation(bool isMovementPressed)
        {
            if (_clipDoubleJump.IsPlayingAnimation)
            {
                _transitionBonusTime = 0.1f;
                _clipDoubleJump.IsPlayingAnimation = false;
                PlayIdleAnimation(isMovementPressed);
                PlayRunningAnimation(isMovementPressed);
            }
            else
                _transitionBonusTime = 0.05f;

            if (!_isPlayingJumpAnimation) return;
            _isPlayingJumpAnimation = false;
            _clipDoubleJump.IsPlayingAnimation = false;
            _clipJumpingFromJumpad.IsPlayingAnimation = false;
            PlayIdleAnimation(isMovementPressed);
            PlayRunningAnimation(isMovementPressed);
            _transitionBonusTime = 0f;
        }

        private void PlaySlideAnimation()
        {
            if (_clipSliding.IsPlayingAnimation) return;
            if (_isPlayingJumpAnimation) return;
            PlayAnimation(_clipSliding);
            _clipSliding.IsPlayingAnimation = true;
        }

        private void EndSlideAnimation(bool isMovementPressed)
        {
            if (!_clipSliding.IsPlayingAnimation) return;
            _clipSliding.IsPlayingAnimation = false;
            if (_isPlayingJumpAnimation) return;
            _transitionBonusTime = 0.05f;
            PlayIdleAnimation(isMovementPressed);
            PlayRunningAnimation(isMovementPressed);
            _transitionBonusTime = 0f;
        }

        private void PlayLedgeClimbAnimation()
        {
            _animator.applyRootMotion = true;
            PlayAnimation(_clipLegdeClimb);
        }

        private void PlayFallAnimation()
        {
            if (_clipFalling.IsPlayingAnimation) return;
            if (_clipDoubleJump.IsPlayingAnimation) return;
            if (_clipJumpingFromJumpad.IsPlayingAnimation) return;
            for (int i = 0; i < _clipsJumping.Length; i++)
                if (_clipsJumping[i].IsPlayingAnimation)
                    return;

            PlayAnimation(_clipFalling);
            _clipFalling.IsPlayingAnimation = true;
        }

        private void EndFallAnimation(bool isMovementPressed)
        {
            if (!_clipFalling.IsPlayingAnimation) return;
            _clipFalling.IsPlayingAnimation = false;
            _transitionBonusTime = 0.05f;
            if (MovementController.Instance.DisableFunctionality)
                isMovementPressed = false;
            PlayIdleAnimation(isMovementPressed);
            PlayRunningAnimation(isMovementPressed);
            _transitionBonusTime = 0f;
        }

        private void PlayHangingAnimation()
        {
            if (_clipHanging.IsPlayingAnimation) return;
            PlayAnimation(_clipHanging);
            _clipHanging.IsPlayingAnimation = true;
        }

        private void RopeHangAction_OnAnyGrabRope(float swingDirection)
        {
            if (swingDirection >= 0f)
            {
                if (_clipRopeHang.IsPlayingAnimation) return;
                _clipRopeHangBackward.IsPlayingAnimation = false;
                PlayAnimation(_clipRopeHang);
                _clipRopeHang.IsPlayingAnimation = true;
            }
            else
            {
                if (_clipRopeHangBackward.IsPlayingAnimation) return;
                _clipRopeHang.IsPlayingAnimation = false;
                PlayAnimation(_clipRopeHangBackward);
                _clipRopeHangBackward.IsPlayingAnimation = true;
            }

            _isPlayingJumpAnimation = false;
        }

        private void RopeHangAction_OnAnyReleaseRope()
        {
            if (!_clipRopeHang.IsPlayingAnimation && !_clipRopeHangBackward.IsPlayingAnimation) return;
            _clipRopeHang.IsPlayingAnimation = false;
            _clipRopeHangBackward.IsPlayingAnimation = false;
            // For some reason it changes to true in the android build. I had to set it to false manually, else the animation wouldn't switch from hanging to jumping.
            _isPlayingJumpAnimation = false;
            PlayJumpAnimation();
        }

        private void PlayDieAnimation() => ResetAnPlayIdleAnimation();

        private void ResetAnPlayIdleAnimation()
        {
            ResetAnimationStates();
            PlayIdleAnimation(false);
        }

        private void PlayClimbingAnimation(float direction)
        {
            if (_clipClimb.IsPlayingAnimation) return;
            PlayAnimation(_clipClimb);
            _clipClimb.IsPlayingAnimation = true;
        }

        private void EndClimbingAnimation()
        {
            if (!_clipClimb.IsPlayingAnimation) return;
            _clipClimb.IsPlayingAnimation = false;
        }

        private void FlyAction_OnPlayerFly()
        {
            PlayAnimation(_clipFly);
            _clipFly.IsPlayingAnimation = true;
            _clipFlyAscend.IsPlayingAnimation = false;
            _clipFlyDescend.IsPlayingAnimation = false;
        }

        private void FlyAction_OnPlayerFlyCancel()
        {
            _clipFly.IsPlayingAnimation = false;
            _clipFlyAscend.IsPlayingAnimation = false;
            _clipFlyDescend.IsPlayingAnimation = false;
            _isPlayingJumpAnimation = false;
            _transitionBonusTime = 0.05f;
            PlayFallAnimation();
            _transitionBonusTime = 0f;
        }

        private void FlyAction_OnAnyFlyAscending(FloatActionDirection? flyDirection)
        {
            if (!_clipFly.IsPlayingAnimation) return;
            _clipFly.IsPlayingAnimation = false;
            _clipFlyDescend.IsPlayingAnimation = false;
            PlayAnimation(_clipFlyAscend);
            _clipFlyAscend.IsPlayingAnimation = true;
            _clipFly.IsPlayingAnimation = true;
        }

        private void FlyAction_OnAnyStopFlyAscendingOrDescending(FloatActionDirection? flyDirection)
        {
            if (!_clipFly.IsPlayingAnimation) return;
            _clipFly.IsPlayingAnimation = false;
            _clipFlyDescend.IsPlayingAnimation = false;
            _clipFlyAscend.IsPlayingAnimation = false;
            PlayAnimation(_clipFly);
            _clipFly.IsPlayingAnimation = true;
        }

        private void FlyAction_OnAnyFlyDescending(FloatActionDirection? flyDirection)
        {
            if (!_clipFly.IsPlayingAnimation) return;
            _clipFly.IsPlayingAnimation = false;
            _clipFlyAscend.IsPlayingAnimation = false;
            PlayAnimation(_clipFlyDescend);
            _clipFlyDescend.IsPlayingAnimation = true;
            _clipFly.IsPlayingAnimation = true;
        }

        private void SwimAction_OnPlayerSwim()
        {
            if (_clipSwimming.IsPlayingAnimation) return;
            PlayAnimation(_clipSwimming);
            _clipSwimming.IsPlayingAnimation = true;
            _clipSwimmingAscend.IsPlayingAnimation = false;
            _clipSwimmingDescend.IsPlayingAnimation = false;
        }

        private void SwimAction_OnPlayerSwimCancel()
        {
            _clipSwimming.IsPlayingAnimation = false;
            _clipSwimmingAscend.IsPlayingAnimation = false;
            _clipSwimmingDescend.IsPlayingAnimation = false;
            _transitionBonusTime = 0.05f;
            PlayRunningAnimation(true);
            _transitionBonusTime = 0f;
        }

        private void SwimAction_OnAnySwimAscending(FloatActionDirection? swimDirection)
        {
            if (_clipSwimmingAscend.IsPlayingAnimation) return;
            if (!_clipSwimming.IsPlayingAnimation) return;
            if (CharacterPlayerController.Instance.IsDead) return;
            _clipSwimming.IsPlayingAnimation = false;
            _clipSwimmingDescend.IsPlayingAnimation = false;
            PlayAnimation(_clipSwimmingAscend);
            _clipSwimmingAscend.IsPlayingAnimation = true;
            _clipSwimming.IsPlayingAnimation = true;
        }

        private void SwimAction_OnAnyStopSwimAscendingOrDescending(FloatActionDirection? swimDirection)
        {
            if (!_clipSwimming.IsPlayingAnimation) return;
            _clipSwimming.IsPlayingAnimation = false;
            _clipSwimmingDescend.IsPlayingAnimation = false;
            _clipSwimmingAscend.IsPlayingAnimation = false;
            PlayAnimation(_clipSwimming);
            _clipSwimming.IsPlayingAnimation = true;
        }

        private void SwimAction_OnAnySwimDescending(FloatActionDirection? swimDirection)
        {
            if (_clipSwimmingDescend.IsPlayingAnimation) return;
            if (!_clipSwimming.IsPlayingAnimation) return;
            if (CharacterPlayerController.Instance.IsDead) return;
            _clipSwimming.IsPlayingAnimation = false;
            _clipSwimmingAscend.IsPlayingAnimation = false;
            PlayAnimation(_clipSwimmingDescend);
            _clipSwimmingDescend.IsPlayingAnimation = true;
            _clipSwimming.IsPlayingAnimation = true;
        }

        private void GlideAction_OnStartGliding()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _doubleJumpHash)
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f <= 0.5f)
                {
                    if (_glidingAimationCoroutine != null) StopCoroutine(_glidingAimationCoroutine);
                    _glidingAimationCoroutine = StartCoroutine(PlayGlidingAnimationWithDelay());
                    return;
                }
            }

            _clipDoubleJump.IsPlayingAnimation = false;
            _clipJumpingFromJumpad.IsPlayingAnimation = false;
            PlayAnimation(_clipGliding);
            _clipGliding.IsPlayingAnimation = true;
        }

        private IEnumerator PlayGlidingAnimationWithDelay()
        {
            yield return _waitGlide;
            _clipDoubleJump.IsPlayingAnimation = false;
            _clipJumpingFromJumpad.IsPlayingAnimation = false;
            PlayAnimation(_clipGliding);
            _clipGliding.IsPlayingAnimation = true;
        }

        private void GlideAction_OnStopGliding()
        {
            if (_glidingAimationCoroutine != null) StopCoroutine(_glidingAimationCoroutine);
            _clipGliding.IsPlayingAnimation = false;
            PlayFallAnimation();
        }

        private AnimationClipData GetRandomJumpAnimation()
        {
            _endJumpAnimationLength = _clipsJumping[_jumpIndex].Animation.length;
            _endJumpTimer = _endJumpAnimationLength * _jumpTimerFactor;
            _waitEndJump = new WaitForSeconds(_endJumpTimer);
            return _clipsJumping[_jumpIndex];
        }

        private void PlayAnimation(AnimationClipData clipData, bool applyRootMotion = false)
        {
            if (_tweenEndJump != null) _tweenEndJump.Kill();
            if (_tweenEndDoubleJump != null) _tweenEndDoubleJump.Kill();

            if (_clipFly.IsPlayingAnimation ||
                _clipFlyAscend.IsPlayingAnimation ||
                _clipFlyDescend.IsPlayingAnimation)
                return;

            if (_clipSwimming.IsPlayingAnimation ||
                _clipSwimmingAscend.IsPlayingAnimation ||
                _clipSwimmingDescend.IsPlayingAnimation)
                return;

            if (_clipGliding.IsPlayingAnimation) return;
            if (_clipRopeHang.IsPlayingAnimation) return;
            if (_clipRopeHangBackward.IsPlayingAnimation) return;

            if (_animator == null) return;
            _animator.applyRootMotion = applyRootMotion;
            ResetAnimationStates();
            float totalTransitionDuration = _totalTransitionTime + clipData.AdditionalTransitionTime;
            _animator.CrossFadeInFixedTime(clipData.AnimId, totalTransitionDuration);
        }

        private void ResetAnimationStates()
        {
            _clipRunning.IsPlayingAnimation = false;
            _clipFalling.IsPlayingAnimation = false;
            _clipHanging.IsPlayingAnimation = false;
            _clipRopeHang.IsPlayingAnimation = false;
            _clipRopeHangBackward.IsPlayingAnimation = false;
            _clipDie.IsPlayingAnimation = false;
            _clipSliding.IsPlayingAnimation = false;
            _clipClimb.IsPlayingAnimation = false;
        }

        public void HaltAction_OnPlayerHalt()
        {
            if (!_clipClimb.IsPlayingAnimation) return;
            if (_animator.speed == 0f) return;
            _currAnimationSpeed = _animator.speed;
            _animator.speed = 0f;
        }

        public void HaltAction_OnPlayerStopHalt()
        {
            if (_animator.speed > 0) return;
            _animator.speed = _currAnimationSpeed;
        }
    }
}
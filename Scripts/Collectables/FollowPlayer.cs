using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Leon
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Vector3 _offset = Vector3.zero;

        [BoxGroup("Collect Distance"), SerializeField]
        private float _collectDistance = .5f;

        private BaseCollectable _collectable;
        private Tweener _moveTweener;
        private Vector3 _startPosition;
        private float _currSpeed;
        private float _currTimeScale;

        private void Awake()
        {
            _collectable = GetComponent<BaseCollectable>();
            _startPosition = transform.position;
            _currSpeed = _speed;
            _currTimeScale = 1f;
        }

        private void OnEnable() => CharacterPlayerController.OnPlayerDied += CharacterPlayerController_OnPlayerDied;

        private void OnDisable()
        {
            CharacterPlayerController.OnPlayerDied -= CharacterPlayerController_OnPlayerDied;
            TryKillTween();
        }

        public void Follow(Transform followTarget)
        {
            TryKillTween();
            _moveTweener = transform.DOMove(followTarget.position + _offset, _currSpeed)
                .SetSpeedBased()
                .SetEase(Ease.OutQuad);

            _moveTweener.OnUpdate(() =>
            {
                _currTimeScale += Time.deltaTime;
                _moveTweener.DOTimeScale(_currTimeScale, 0f);
                _moveTweener.ChangeEndValue(followTarget.position + _offset, true);

                float distanceToPlayer = Vector3.Distance(transform.position,
                    CharacterPlayerController.Instance.transform.position);
                if (distanceToPlayer <= _collectDistance) _collectable.OnItemCollected();
            });
        }

        private void CharacterPlayerController_OnPlayerDied() => Reset();

        private bool TryKillTween()
        {
            if (_moveTweener != null)
            {
                _moveTweener.Kill();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            TryKillTween();
            transform.position = _startPosition;
            _currSpeed = _speed;
            _currTimeScale = 1f;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using Leon;

namespace Leon
{
    public class PlayerController : SceneSingleton<PlayerController>
    {
        public static Action<bool> OnRespawn;

        private PlayerMovement _playerMovement;
        [Header("Respawn"), SerializeField] private float _respawnDelay = 1f;
        [SerializeField] private float _moveAfterRespawnDelay;

        private WaitForSeconds _waitRespawn;
        private WaitForSeconds _waitAfterRespawnDelay;

        private bool _isRespawning;

        public bool IsRespawning => _isRespawning;

        protected override void Awake()
        {
            base.Awake();
            _playerMovement = GetComponent<PlayerMovement>();
            _waitRespawn = new WaitForSeconds(_respawnDelay);
            _waitAfterRespawnDelay = new WaitForSeconds(_moveAfterRespawnDelay);
        }

        private void OnEnable()
        {
            PlayerMovement.OnPlayerRespawn += RespawnPlayer;
        }

        private void OnDisable()
        {
            PlayerMovement.OnPlayerRespawn -= RespawnPlayer;
        }

        private void Update()
        {
            if (_isRespawning) return;
            _playerMovement.HandleMovementUpdate();
        }

        public void RespawnPlayer(Vector3 respawnPoint) => StartCoroutine(HandleRespawn(respawnPoint));

        private IEnumerator HandleRespawn(Vector3 respawnPoint)
        {
            _isRespawning = true;
            yield return _waitRespawn;
            transform.position = respawnPoint;
            yield return _waitAfterRespawnDelay;
            _isRespawning = false;
        }
    }
}
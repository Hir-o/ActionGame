using Lean.Pool;
using UnityEngine;
using Leon;

public class PlayerParticleSpawner : Singleton<PlayerParticleSpawner>
{
    [SerializeField] private GameObject[] _particles;

    private GameObject player;
    private GameObject trail;
    private bool idle;

    private void Start() => player = MovementController.Instance.gameObject;

    private void OnEnable()
    {
        MovementController.OnPlayerJump += MovementController_OnPlayerJump;
        MovementController.OnPlayerDoubleJump += MovementController_OnPlayerDoubleJump;
        MovementController.OnPlayerSlide += MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel += MovementController_OnPlayerSlideCancel;
    }

    private void OnDisable()
    {
        MovementController.OnPlayerJump -= MovementController_OnPlayerJump;
        MovementController.OnPlayerDoubleJump -= MovementController_OnPlayerDoubleJump;
        MovementController.OnPlayerSlide -= MovementController_OnPlayerSlide;
        MovementController.OnPlayerSlideCancel -= MovementController_OnPlayerSlideCancel;
    }

    private void MovementController_OnPlayerJump()
    {
        FactorySpawner("jump", transform);
    }

    private void MovementController_OnPlayerDoubleJump()
    {
        FactorySpawner("doubleJump", transform);
    }

    private void MovementController_OnPlayerSlide()
    {
        FactorySpawner("sliding", player.transform);
    }

    private void MovementController_OnPlayerSlideCancel(bool obj)
    {
        if (obj)
            if (trail != null)
            {
                trail.transform.SetParent(null);
                trail.SetActive(false);
            }
    }

    private void FactorySpawner(string particleName, Transform parent)
    {
        if (player == null) player = CharacterPlayerController.Instance.gameObject;
        ParticleFactory.GetParticle(particleName).SpawnParticle(player.transform, parent);
    }

    public void TrailInstantiator(int particleIndex, Transform player, Transform parent)
    {
        if (player == null) player = CharacterPlayerController.Instance.transform;
        if (trail == null)
            trail = LeanPool.Spawn(_particles[particleIndex], player.position, player.rotation, parent);

        TrailActivator(parent);
    }

    private void TrailActivator(Transform parent)
    {
        trail.transform.SetParent(parent);
        trail.transform.localPosition = Vector3.zero;
        trail.SetActive(true);
    }

    public void ParticleInstantiator(int particleIndex, Transform player, Transform parent)
    {
        if (player == null) player = CharacterPlayerController.Instance.transform;
        LeanPool.Spawn(_particles[particleIndex], player.position, player.rotation, parent);
    }
}
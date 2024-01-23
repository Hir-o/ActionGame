using UnityEngine;

public class RocketThruster : MonoBehaviour
{
    [SerializeField] private ParticleSystem _rocketMissleFireVfx;
    [SerializeField] private ParticleSystem _whiteGlobeVfx;

    public void EnableThrusterFires()
    {
        _rocketMissleFireVfx.Play();
        _whiteGlobeVfx.Play();
    }
    
    public void DisableThrusterFires()
    {
        _rocketMissleFireVfx.Stop();
        _whiteGlobeVfx.Stop();
    }
}

using System.Collections;

public interface ISoundEffectCooldown
{
    public float CooldownDuration { get; }
    public bool IsInCooldown { get; set; }

    public IEnumerator CooldownSfx();
}
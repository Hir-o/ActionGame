using System;
using Factory;

public class RareCoin : Coin
{
    public static event Action OnCollectRareCoin;

    private void Start() => CoinsManager.Instance.AddToTotalSpecialCoinAmount();

    public override void OnItemCollected()
    {
        if (IsCollected) return;
        IsCollected = true;
        Gfx.SetActive(false);
        SpawnCollectedVfx();
        if (FollowPlayer != null) FollowPlayer.Reset();
        if (SoundEffectsManager.Instance != null) SoundEffectsManager.Instance.PlayGemCollectSfx();
        OnCollectRareCoin?.Invoke();
    }

    protected override void PlayerRespawn_OnPlayerRespawn()
    {
        if (IsCollected) return;
        Gfx.SetActive(true);
    }

    public virtual void SpawnCollectedVfx() =>
        CollectableParticleFactory.Instance.GetNewGemVfxInstance(transform.position);
}
using UnityEngine;

[CreateAssetMenu(fileName = "LevelStarReward", menuName = "Level Star Reward/LevelStarReward", order = 1)]
public class LevelStarRewardData : ScriptableObject
{
    [SerializeField] private Sprite _starOffSprite;
    [SerializeField] private Sprite _starOnSprite;

    public Sprite StarOffSprite => _starOffSprite;
    public Sprite StarOnSprite => _starOnSprite;
}
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableCardData", menuName = "CollectableCard/CollectableCard", order = 0)]
public class CollectableCardData : ScriptableObject
{
    [SerializeField] private string _name = "Hammer";
    [SerializeField] private string _rarity = "Normal";
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _cardSprite;
    [SerializeField] private Sprite _rarityLabelSprite;
    [SerializeField] private Color _glowColor;
    [SerializeField] private int _starsRequiredToUnlock = 6;

    #region Properties

    public string Name => _name;
    public string Rarity => _rarity;
    public Sprite Sprite => _sprite;
    public Sprite CardSprite => _cardSprite;
    public Sprite RarityLabelSprite => _rarityLabelSprite;
    public Color GlowColor => _glowColor;
    public int StarsRequired => _starsRequiredToUnlock;

    #endregion
}
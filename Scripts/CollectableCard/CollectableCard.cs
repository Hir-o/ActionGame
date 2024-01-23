using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CollectableCard : MonoBehaviour
{
    [BoxGroup("Card Data"), SerializeField]
    private CollectableCardData _cardData;

    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _cardNameText;
    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _rarityText;
    [BoxGroup("UI"), SerializeField] private Image _iconImage;
    [BoxGroup("UI"), SerializeField] private Image _cardImage;
    [BoxGroup("UI"), SerializeField] private Image _rarityLabelImage;
    [BoxGroup("UI"), SerializeField] private Image _glowImage;
    [BoxGroup("UI"), SerializeField] private Image _overlayImage;
    [BoxGroup("UI"), SerializeField] private Image _frameImage;
    [BoxGroup("UI"), SerializeField] private TextMeshProUGUI _requiredStarsAmountText;
    [BoxGroup("UI"), SerializeField] private GameObject _starsPanel;

    [BoxGroup("Color"), SerializeField] private Color _lockedColor;

    private const string LOCKED = "Locked";

    private void Awake() =>
        Assert.IsNotNull(_cardData, $"CollectableCardData is null in {gameObject.name}. Assign a CollectableCardData!");

    private void Start() => UpdateCard();

    private void UpdateCard()
    {
        if (LevelDataManager.Instance == null) return;
        bool isUnlocked = LevelDataManager.Instance.CollectedStarsAmount >= _cardData.StarsRequired;
        
        _frameImage.gameObject.SetActive(isUnlocked);
        _overlayImage.gameObject.SetActive(!isUnlocked);
        _starsPanel.gameObject.SetActive(!isUnlocked);
        _iconImage.color = isUnlocked ? Color.white : _lockedColor;
        _requiredStarsAmountText.text = _cardData.StarsRequired.ToString();

        _cardNameText.text = isUnlocked ? _cardData.Name : LOCKED;
        _rarityText.text = _cardData.Rarity;
        _iconImage.sprite = _cardData.Sprite;
        _cardImage.sprite = _cardData.CardSprite;
        _rarityLabelImage.sprite = _cardData.RarityLabelSprite;
        _glowImage.color = _cardData.GlowColor;
    }
}
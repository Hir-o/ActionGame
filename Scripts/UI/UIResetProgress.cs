
using System.Collections.Generic;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIResetProgress : SceneSingleton<UIResetProgress>
{
    [SerializeField] private GameObject _resetProgressPopup;
    [BoxGroup("Buttons"), SerializeField] private Button _resetProgressButton;
    [BoxGroup("Buttons"), SerializeField] private Button _closeButton;
    [BoxGroup("Buttons"), SerializeField] private Button _resetButton;

    [BoxGroup("Tweener"), SerializeField] private PopUpTweener _popUpTweener;

    private Dictionary<string, int> _keysToSaveInt = new Dictionary<string, int>();
    private Dictionary<string, float> _keysToSaveFloat = new Dictionary<string, float>();

    protected override void Awake()
    {
        base.Awake();
        _resetProgressPopup.SetActive(false);
        _resetProgressButton.onClick.AddListener(OnClickResetProgressButton);
        _closeButton.onClick.AddListener(OnClickCloseButton);
        _resetButton.onClick.AddListener(OnClickResetButton);
    }

    private void OnClickResetProgressButton()
    {
        _popUpTweener.Reset();
        _resetProgressPopup.SetActive(true);
        _popUpTweener.AnimateOpening();
    }

    private void OnClickCloseButton() => _popUpTweener.AnimateClosing(DeactivatePopupGameObject);

    private void DeactivatePopupGameObject() => _resetProgressPopup.SetActive(false);

    private void OnClickResetButton()
    {
        StoreKeys();
        _resetProgressPopup.SetActive(false);
        PlayerPrefs.DeleteAll();
        LevelDataManager.Instance.ResetAllLevelData();
        UILevelSelection.Instance.LevelSelectButtonList.ForEach(levelSelectButton => levelSelectButton.UpdateStars());

        foreach (var keyValuePair in _keysToSaveInt)
        {
            string key = keyValuePair.Key;
            int value = keyValuePair.Value;
            PlayerPrefs.SetInt(key, value);
        }

        foreach (var keyValuePair in _keysToSaveFloat)
        {
            string key = keyValuePair.Key;
            float value = keyValuePair.Value;
            PlayerPrefs.SetFloat(key, value);
        }

        PlayerPrefs.Save();
    }

    private void StoreKeys()
    {
         _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementSkyline,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementSkyline));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementCoinCollector,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementCoinCollector));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementJumpKing,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementJumpKing));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementEvilFighter,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementEvilFighter));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementGrandCanyon,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementGrandCanyon));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementIDontNeedRoads,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementIDontNeedRoads));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementPirateCove,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementPirateCove));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementStargazer,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementStargazer));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementItsHotInHere,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementItsHotInHere));
        _keysToSaveInt.Add(PlayerPrefsConstants.LocalAchievementYoloHero,
            PlayerPrefs.GetInt(PlayerPrefsConstants.LocalAchievementYoloHero));
        _keysToSaveInt.Add(PlayerPrefsConstants.BatterySaving, PlayerPrefs.GetInt(PlayerPrefsConstants.BatterySaving));
        _keysToSaveInt.Add(PlayerPrefsConstants.Vibration, PlayerPrefs.GetInt(PlayerPrefsConstants.Vibration));
        _keysToSaveFloat.Add(PlayerPrefsConstants.SoundFxVolume,
            PlayerPrefs.GetFloat(PlayerPrefsConstants.SoundFxVolume));
        _keysToSaveFloat.Add(PlayerPrefsConstants.MusicVolume, PlayerPrefs.GetFloat(PlayerPrefsConstants.MusicVolume));
    }
}
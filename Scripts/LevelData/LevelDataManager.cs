
using System.Collections.Generic;
using Leon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataManager : Singleton<LevelDataManager>
{
    [SerializeField] private List<LevelData> _levelDataList;

    public List<LevelData> LevelDataList => _levelDataList;

    private int _collectedStarsAmount;

    #region Properties

    public int CollectedStarsAmount => _collectedStarsAmount;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        LoadData();
        _collectedStarsAmount = CalculateTotalStarsCollected();
    }

    private void LoadData()
    {
        for (int i = 0; i < _levelDataList.Count; i++)
        {
            if (PlayerPrefs.HasKey($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.CoinsReward}{i}"))
            {
                _levelDataList[i].CoinCollectedRewardGained =
                    PlayerPrefs.GetInt($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.CoinsReward}{i}") == 1;
            }

            if (PlayerPrefs.HasKey($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.TimerReward}{i}"))
            {
                _levelDataList[i].TimerRewardGained =
                    PlayerPrefs.GetInt($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.TimerReward}{i}") == 1;
            }

            if (PlayerPrefs.HasKey($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.SpecialCoinsReward}{i}"))
            {
                _levelDataList[i].SpecialCoinRewardGained =
                    PlayerPrefs.GetInt(
                        $"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.SpecialCoinsReward}{i}") == 1;
            }
        }
    }

    public void SaveData()
    {
        bool gainedCoinsReward = false;
        bool completedTimer = false;
        bool gainedSpecialCoinsReward = false;

        int currSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (currSceneIndex < 0) return;
        if (_levelDataList.Count < currSceneIndex) return;
        if (LevelStats.Instance == null) return;

        if (PlayerPrefs.HasKey($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.CoinsReward}{currSceneIndex}"))
        {
            gainedCoinsReward =
                PlayerPrefs.GetInt(
                    $"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.CoinsReward}{currSceneIndex}") == 1;
        }

        if (PlayerPrefs.HasKey($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.TimerReward}{currSceneIndex}"))
        {
            completedTimer =
                PlayerPrefs.GetInt(
                    $"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.TimerReward}{currSceneIndex}") == 1;
        }

        if (PlayerPrefs.HasKey(
                $"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.SpecialCoinsReward}{currSceneIndex}"))
        {
            gainedSpecialCoinsReward =
                PlayerPrefs.GetInt(
                    $"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.SpecialCoinsReward}{currSceneIndex}") == 1;
        }

        bool changedLevelData = false;
        if (!gainedCoinsReward)
        {
            gainedCoinsReward = LevelStats.Instance.GetCoinsCollectedCompletion();
            if (gainedCoinsReward)
            {
                _levelDataList[currSceneIndex].CoinCollectedRewardGained = true;
                changedLevelData = true;
            }

            PlayerPrefs.SetInt($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.CoinsReward}{currSceneIndex}",
                gainedCoinsReward ? 1 : 0);
        }

        if (!completedTimer)
        {
            completedTimer = LevelStats.Instance.GetTimerCompletion();
            if (completedTimer)
            {
                _levelDataList[currSceneIndex].TimerRewardGained = true;
                changedLevelData = true;
            }

            PlayerPrefs.SetInt($"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.TimerReward}{currSceneIndex}",
                completedTimer ? 1 : 0);
        }

        if (!gainedSpecialCoinsReward)
        {
            gainedSpecialCoinsReward = LevelStats.Instance.GetSpecialCoinsCollectedCompletion();
            if (gainedSpecialCoinsReward)
            {
                _levelDataList[currSceneIndex].SpecialCoinRewardGained = true;
                changedLevelData = true;
            }

            PlayerPrefs.SetInt(
                $"{PlayerPrefsConstants.LevelData}{PlayerPrefsConstants.SpecialCoinsReward}{currSceneIndex}",
                gainedSpecialCoinsReward ? 1 : 0);
        }

        if (changedLevelData) PlayerPrefs.Save();
    }

    private int CalculateTotalStarsCollected()
    {
        int amount = 0;
        for (int i = 0; i < LevelDataList.Count; i++)
        {
            LevelData _levelData = LevelDataList[i];
            if (_levelData.CoinCollectedRewardGained) amount++;
            if (_levelData.TimerRewardGained) amount++;
            if (_levelData.SpecialCoinRewardGained) amount++;
        }

        return amount;
    }

    public void ResetAllLevelData()
    {
        for (int i = 0; i < _levelDataList.Count; i++)
        {
            _levelDataList[i].CoinCollectedRewardGained = false;
            _levelDataList[i].TimerRewardGained = false;
            _levelDataList[i].SpecialCoinRewardGained = false;
        }
    }
}
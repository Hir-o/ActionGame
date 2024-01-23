public static class PlayerPrefsConstants
{
    #region SaveVersion

    //When updating the game, if saves need to be reset, then increase this number by 1
    public static readonly string SaveVersion = "V1_";

    #endregion

    #region Levels and Stages

    public static readonly string UnlockedStages = SaveVersion + "UnlockedStages";
    public static readonly string NewStage = SaveVersion + "NewStage";
    public static readonly string LevelData = SaveVersion + "LevelData";

    #endregion

    #region Level Rewards

    public static readonly string CoinsReward =  "_CoinsReward";
    public static readonly string TimerReward = "_TimerReward";
    public static readonly string SpecialCoinsReward = "_SpecialCoinsReward";

    #endregion

    #region Settings

    public static readonly string BatterySaving = "BatterySaving";
    public static readonly string Vibration = "Vibration";
    public static readonly string SoundFxVolume = "SoundFxVolume";
    public static readonly string MusicVolume = "MusicVolume";

    #endregion
    
    #region LocalAchievements

    public static readonly string LocalAchievementSkyline = "LocalAchievementSkyline";
    public static readonly string LocalAchievementCoinCollector = "LocalAchievementCoinCollector";
    public static readonly string LocalAchievementJumpKing = "LocalAchievementJumpKing";
    public static readonly string LocalAchievementEvilFighter = "LocalAchievementEvilFighter";
    public static readonly string LocalAchievementGrandCanyon = "LocalAchievementGrandCanyon";
    public static readonly string LocalAchievementIDontNeedRoads = "LocalAchievementIDontNeedRoads";
    public static readonly string LocalAchievementPirateCove = "LocalAchievementPirateCove";
    public static readonly string LocalAchievementStargazer = "LocalAchievementStargazer";
    public static readonly string LocalAchievementItsHotInHere = "LocalAchievementItsHotInHere";
    public static readonly string LocalAchievementYoloHero = "LocalAchievementYoloHero";

    #endregion
}
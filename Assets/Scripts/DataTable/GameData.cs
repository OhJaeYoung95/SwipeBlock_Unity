using SaveDataVC = SaveDataV1;

public enum Scene
{
    Intro,
    MainMenu,
    Select,
    Game
}

public static class GameData
{
    public static int Gold { get; set; } = 10000;
    public static int ItemCount { get; set; }
    public static int[] Slots { get; set; } = new int[3];
    public static float BestScore { get; set; }
    public static float MasterVolume { get; set; }
    public static float BGMVolune { get; set; }
    public static float SEVolume { get; set; }
    public static bool IsOffMasterMute { get; set; } = false;
    public static bool IsOffBGMMute { get; set; } = false;
    public static bool IsOffSEMute { get; set; } = false;

    public static int CurrentStage { get; set; } = 0;

    public static void SaveGameData()
    {
        var saveData = new SaveDataVC();
        saveData.Gold = Gold;
        saveData.ItemCount = ItemCount;
        saveData.Slots = Slots;
        saveData.BestScore = BestScore;
        saveData.MasterVolume = MasterVolume;
        saveData.BGMVolume = BGMVolune;
        saveData.SEVolume = SEVolume;
        saveData.IsOffMasterMute = IsOffMasterMute;
        saveData.IsOffBGMMute = IsOffBGMMute;
        saveData.IsOffSEMute = IsOffSEMute;
        SaveLoadSystem.Save(saveData, SaveLoadSystem.SaveDataPath);
    }

    public static void LoadGameData()
    {
        var saveData = SaveLoadSystem.Load(SaveLoadSystem.SaveDataPath) as SaveDataVC;
        if (saveData == null)
            SaveGameData();
        else
        {
            Gold = saveData.Gold;
            ItemCount = saveData.ItemCount;
            Slots = saveData.Slots;
            BestScore = saveData.BestScore;
            MasterVolume = saveData.MasterVolume;
            BGMVolune = saveData.BGMVolume;
            SEVolume = saveData.SEVolume;
            IsOffMasterMute = saveData.IsOffMasterMute;
            IsOffBGMMute = saveData.IsOffBGMMute;
            IsOffSEMute = saveData.IsOffSEMute;
        }
    }

}

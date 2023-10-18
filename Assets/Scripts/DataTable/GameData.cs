using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public static class GameData
{
    public static int Gold { get; set; }
    public static int[] Slots { get; set; } = new int[3];
    public static float BestScore { get; set; }

    public static void SaveGameData()
    {
        var saveData = new SaveDataVC();
        saveData.Gold = Gold;
        saveData.Slots = Slots;
        saveData.BestScore = BestScore;
        SaveLoadSystem.Save(saveData, SaveLoadSystem.SaveDataPath);
    }

    public static void LoadGameData()
    {
        var saveData = SaveLoadSystem.Load(SaveLoadSystem.SaveDataPath) as SaveDataVC;
        if (saveData == null)
            SaveGameData();

        Gold = saveData.Gold;
        Slots = saveData.Slots;
        BestScore = saveData.BestScore;
    }

}

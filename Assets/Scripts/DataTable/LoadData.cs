using GooglePlayGames;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
        });

        //Application.targetFrameRate = 60;
        GameData.LoadGameData();
    }
}

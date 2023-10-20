using UnityEngine;

public class LoadData : MonoBehaviour
{
    private void Awake()
    {
        //Application.targetFrameRate = 60;
        GameData.LoadGameData();
    }
}

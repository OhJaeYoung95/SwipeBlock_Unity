using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    private void Awake()
    {
        GameData.LoadGameData();
    }
}

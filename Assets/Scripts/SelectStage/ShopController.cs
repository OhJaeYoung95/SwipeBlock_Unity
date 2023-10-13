using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Score,
    Timer,
    Destroy
}

public struct Item
{
    int id;
    ItemType type;
    int value;
    int duration;
    int cost;
}
public class ShopController : MonoBehaviour
{
    public static ShopController Instance { get; private set; }
    private int itemCount = 0;

    private List<Item> slots = new List<Item>();
    private List<Toggle> toggles = new List<Toggle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Instance.ShopInit();
    }
    public void ShopInit()
    {
        Instance.itemCount = 0;
    }

    public void OnClickBuyButton()
    {
        itemCount++;
    }
}

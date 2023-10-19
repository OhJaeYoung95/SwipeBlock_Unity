using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SaveDataVC = SaveDataV1;

public enum ItemType
{
    Score = 1,
    Timer,
    Bomb
}

public enum ItemID
{
    None = 0,
    Score1 = 1,
    Score2,
    Score3,
    Timer1,
    Timer2,
    Timer3,
    Bomb1,
    Bomb2
}

public struct ItemInfo
{
    public string path;
    public ItemID id;
    public ItemType type;
    public float value;
    public float duration;
    public int price;

    public ItemInfo(string path = "", int id = -1, int type = -1, float value = 0, float duration = 0, int price = 0)
    {
        this.path = path;
        this.id = (ItemID)id;
        this.type = (ItemType)type;
        this.value = value;
        this.duration = duration;
        this.price = price;
    }
}
public class ShopController : MonoBehaviour
{
    private int itemCount = 0;

    private List<Item> slots = new List<Item>();

    private ItemTable itemTable;

    //private List<Toggle> toggles = new List<Toggle>();
    private Dictionary<Toggle, ItemID> toggleToItemMapping = new Dictionary<Toggle, ItemID>();
    private List<ItemID> itemInfos = new List<ItemID>();

    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
    private GameObject slotFrame;
    [SerializeField] 
    private GameObject[] shopItemUISlots;
    [SerializeField] 
    private GameObject[] stageItemUISlots;

    [SerializeField]
    private GameObject canvas;
    private Transform slotContent;

    [SerializeField]
    private StageController stageController;


    private void Awake()
    {
        itemTable = DataTableManager.GetTable<ItemTable>();
        ShopInit();
    }

    public void ShopInit()
    {
        itemCount = 0;
        slotContent = canvas.transform.GetChild(1).transform.GetChild(8).transform.GetChild(0).transform.GetChild(0);

        int itemSlotCount = itemTable.GetTableSize();

        for (int i = 1; i < itemSlotCount + 1; ++i)
        {
            GameObject slot = Instantiate(slotFrame, slotContent);
            string itemImagePath = itemTable.GetItemInfo((ItemID)i).path;
            int price = itemTable.GetItemInfo((ItemID)i).price;
            slot.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Arts/{itemImagePath}");
            slot.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{price} Gold";
            slot.GetComponent<Toggle>().group = toggleGroup;
        }

        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach( Toggle toggle in toggles )
        {
            toggle.onValueChanged.AddListener(OnClickItemToggle);
        }
        for (int i = 1; i <= itemSlotCount; ++i)
        {
            toggleToItemMapping[toggles[i - 1]] = (ItemID)i;
        }

        for(int i = 0; i < shopItemUISlots.Length; ++i)
        {
            if (GameData.Slots[i] == 0)
                continue;
            ApplyItemSlotImage(shopItemUISlots[i], (ItemID)GameData.Slots[i]);
            ApplyItemSlotImage(stageItemUISlots[i], (ItemID)(ItemID)GameData.Slots[i]);
        }
    }

    public void OnClickItemToggle(bool value)
    {
        SoundManager.Instance.PlayShopItemToggleSound();
    }

    public void OnClickBuyButton()
    {
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedToggle == null)
        {
            SoundManager.Instance.PlayFailedBuySound();
            return;
        }

        if (GameData.ItemCount == 3)
        {
            // 실패음
            SoundManager.Instance.PlayFailedBuySound();
            return;
        }
        if(selectedToggle != null)
        {
            ItemID selectedItemID = toggleToItemMapping[selectedToggle];

            int price = itemTable.GetItemInfo((ItemID)selectedItemID).price;
            // 구입 실패
            if(GameData.Gold < price)
            {
                // 실패음
                SoundManager.Instance.PlayFailedBuySound();
                return;
            }

            SoundManager.Instance.PlayBuyButtonClickSound();
            GameData.Gold -= price;
            //itemInfos.Add(selectedItemID);
            int index = 0;
            for (int i = 0; i < shopItemUISlots.Length; ++i)
            {
                if (GameData.Slots[i] == 0)
                {
                    index = i;
                    GameData.Slots[i] = (int)selectedItemID;
                    break;
                }
            }
            ApplyItemSlotImage(shopItemUISlots[index], (ItemID)selectedItemID);
            ApplyItemSlotImage(stageItemUISlots[index], (ItemID)selectedItemID);
            GameData.ItemCount++;
        }
        stageController.DisplayGold();
        GameData.SaveGameData();
    }

    //public List<ItemID> GetItemSlotInfo()
    //{
    //    return itemInfos;
    //}

    public void ApplyItemSlotImage(GameObject slot, ItemID selectedItemID)
    {
        string itemImagePath = itemTable.GetItemInfo((ItemID)selectedItemID).path;
        Image itemSlotUIImage = slot.transform.GetChild(1).GetComponent<Image>();
        itemSlotUIImage.sprite = Resources.Load<Sprite>($"Arts/{itemImagePath}");
        Color iamgeColor = itemSlotUIImage.color;
        iamgeColor.a = 255;
        itemSlotUIImage.color = iamgeColor;
    }
}

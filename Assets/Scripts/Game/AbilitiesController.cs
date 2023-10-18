using UnityEngine;
using UnityEngine.UI;

public class AbilitiesController : MonoBehaviour
{
    [SerializeField]
    private GameObject itemSlot1;
    [SerializeField]
    private GameObject itemSlot2;
    [SerializeField]
    private GameObject itemSlot3;

    private bool isUsedSlot1 = false;
    private bool isUsedSlot2 = false;
    private bool isUsedSlot3 = false;
    private bool isUsedSkill = false;

    private ItemTable itemTable;

    private void Awake()
    {
        itemTable = DataTableManager.GetTable<ItemTable>();
    }
    public void OnButtonHoverEnter()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnButtonHoverEnter();
    }

    public void OnButtonHoverExit()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnButtonHoverExit();
    }

    public void UseItemSlot1()
    {
        if (GameManager.Instance.IsMove)
            return;
        if(!isUsedSlot1)
        {
            isUsedSlot1 = true;
            UseItem(itemSlot1, (ItemID)GameData.Slots[0]);
        }
    }
    public void UseItemSlot2()
    {
        if (GameManager.Instance.IsMove)
            return;
        if(!isUsedSlot2)
        {
            isUsedSlot2 = true;
            UseItem(itemSlot2, (ItemID)GameData.Slots[1]);
        }
    }
    public void UseItemSlot3()
    {
        if (GameManager.Instance.IsMove)
            return;
        if(!isUsedSlot3)
        {
            isUsedSlot3 = true;
            UseItem(itemSlot3, (ItemID)GameData.Slots[2]);
        }

    }
    public void UseSkillSlot()
    {
        if (GameManager.Instance.IsMove)
            return;
        if(!isUsedSkill)
        {
            isUsedSkill = true;
        }
    }

    public void UseItem(GameObject itemSlot, ItemID id)
    {
        // ������ ��� �κ�
        ItemType type = itemTable.GetItemInfo(id).type;
        float value = itemTable.GetItemInfo(id).value;
        float duration = itemTable.GetItemInfo(id).duration;
        int price = itemTable.GetItemInfo(id).price;


        if (id != ItemID.None)
        {
            switch (type)
            {
                case ItemType.Score:
                    ScoreManager.Instance.IsScoreIncreaseByItem = true;
                    ScoreManager.Instance.itemValue = value;
                    UIManager.Instance.scoreItemDuration = duration;
                    break;
                case ItemType.Timer:
                    UIManager.Instance.isStopTimer = true;
                    UIManager.Instance.stopDuration = duration;
                    UIManager.Instance.ApplyStopTimerImage();
                    break;
                case ItemType.Bomb:

                    if (value == 0)
                    {
                        BlockManager blockManager = BlockManager.Instance;
                        foreach (Block obs in blockManager.obsList)
                        {
                            if (obs != null)
                            {
                                blockManager.RemoveBlock(obs);
                                PlayBombObsEffect(obs);
                            }
                        }
                        blockManager.obsList.Clear();
                    }
                    else
                    {
                        int count = Mathf.Min((int)value, BlockManager.Instance.obsList.Count);

                        for (int i = 0; i < count; ++i)
                        {
                            Block obs = BlockManager.Instance.obsList[i];
                            if (obs != null)
                            {
                                BlockManager.Instance.RemoveBlock(obs);
                                PlayBombObsEffect(obs);
                            }
                        }
                        BlockManager.Instance.obsList.RemoveRange(0, count);
                    }
                    break;
            }
        }
        Image slotImage = itemSlot.transform.GetChild(1).GetComponent<Image>();
        slotImage.sprite = null;
        Color imageColor = slotImage.color;
        imageColor.a = 0f;
        slotImage.color = imageColor;
    }

    public void PlayBombObsEffect(Block obs)
    {
        ParticleSystem effect = Instantiate(obs.mergeEffect2, obs.transform.position, Quaternion.identity);
        effect.gameObject.SetActive(true);
        effect.Play();
    }
}

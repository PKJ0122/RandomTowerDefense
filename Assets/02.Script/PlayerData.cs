using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PlayerData : SingletonMonoBase<PlayerData>
{
    private const string FILENAME = "PlayerData.dat";

    #region User Data Dictionary
    static Dictionary<string, ItemLevelData> s_itemLevels;
    static Dictionary<string, QuestSaveData> s_questDatas;
    static Dictionary<UnitKind, BeyondCraftingData> s_beyondCraftingDatas;
    static Dictionary<string, MailSaveData> s_mailSaveDatas;

    public static Dictionary<string, ItemLevelData> ItemLevels
    {
        get
        {
            if (s_itemLevels == null)
            {
                s_itemLevels = new Dictionary<string, ItemLevelData>();
                foreach (ItemLevelData itemLevelData in PlayerDataContainer.itemLevelData)
                {
                    s_itemLevels.Add(itemLevelData.itemName, itemLevelData);
                }
            }
            return s_itemLevels;
        }
    }
    public static Dictionary<string, QuestSaveData> QuestDatas
    {
        get
        {
            if (s_questDatas == null)
            {
                s_questDatas = new Dictionary<string, QuestSaveData>();
                foreach (QuestSaveData questSaveData in PlayerDataContainer.questSaveDatas)
                {
                    s_questDatas.Add(questSaveData.QuestName, questSaveData);
                }
            }
            return s_questDatas;
        }
    }
    public static Dictionary<UnitKind, BeyondCraftingData> BeyondCraftingDatas
    {
        get
        {
            if (s_beyondCraftingDatas == null)
            {
                s_beyondCraftingDatas = new Dictionary<UnitKind, BeyondCraftingData>();
                foreach (BeyondCraftingData beyondCraftingData in PlayerDataContainer.beyondCraftingDatas)
                {
                    s_beyondCraftingDatas.Add(beyondCraftingData.unitKind, beyondCraftingData);
                }
            }
            return s_beyondCraftingDatas;
        }
    }
    public static Dictionary<string, MailSaveData> MailSaveDatas
    {
        get
        {
            if (s_mailSaveDatas == null)
            {
                s_mailSaveDatas = new Dictionary<string, MailSaveData>();

                foreach (MailSaveData mailSaveData in PlayerDataContainer.mailSaveDatas)
                {
                    s_mailSaveDatas.Add(mailSaveData.mailName, mailSaveData);
                }
            }
            return s_mailSaveDatas;
        }
    }
    #endregion

    static PlayerDataContainer s_playerDataContainer;
    public static PlayerDataContainer PlayerDataContainer
    {
        get
        {
            if (s_playerDataContainer == null)
            {
                s_playerDataContainer = new PlayerDataContainer();
            }
            return s_playerDataContainer;
        }
        set
        {
            s_playerDataContainer = value;
        }
    }

    public int Gold
    {
        get => PlayerDataContainer.gold;
        set
        {
            PlayerDataContainer.gold = value;
            OnGoldChange?.Invoke(value);
            SaveData();
        }
    }
    public int Diamond
    {
        get => PlayerDataContainer.diamond;
        set
        {
            PlayerDataContainer.diamond = value;
            OnDiamondChange?.Invoke(value);
            SaveData();
        }
    }
    public int ItemSummons
    {
        get => PlayerDataContainer.itemSummons;
        set
        {
            PlayerDataContainer.itemSummons = value;
            OnItemSummonsChange?.Invoke(value);
            SaveData();
        }
    }

    public DateTime LastShopChange
    {
        get => DateTime.Parse(PlayerDataContainer.lastShopChange);
        set
        {
            PlayerDataContainer.lastShopChange = value.ToString();
            PlayerDataContainer.freeDiamondBuy = false;
            PlayerDataContainer.adItemBuy = false;
            PlayerDataContainer.adDiamondBuy = false;
            OnLastShopChangeChange?.Invoke();
            SaveData();
        }
    }

    public event Action<string> OnPlayerNameChange;
    public event Action<int> OnGoldChange;
    public event Action<int> OnDiamondChange;
    public event Action<int> OnItemSummonsChange;
    public event Action OnLastShopChangeChange;
    public event Action<UnitLevelData> OnUnitDataChange;
    public event Action<ItemLevelData> OnItemDataChange;
    public event Action<QuestSaveData> OnQuestSaveDataChange;

    /// <summary>
    /// ���� ���̺� ������ ���� �Լ�
    /// </summary>
    /// <param name="index">������ �ε��� �ѹ�</param>
    /// <param name="shopSaveData">������ ���� ������</param>
    public void SetShopSaveData(int index, ShopSaveData shopSaveData)
    {
        if (index > PlayerDataContainer.shopSaveDatas.Count - 1)
        {
            for (int i = PlayerDataContainer.shopSaveDatas.Count - 1; i < index; i++)
            {
                PlayerDataContainer.shopSaveDatas.Add(null);
            }
        }

        PlayerDataContainer.shopSaveDatas[index] = shopSaveData;
        SaveData();
    }

    /// <summary>
    /// ����Ʈ ���� �����Լ�
    /// </summary>
    /// <param name="qusetName"></param>
    /// <param name="amount"></param>
    public void SetQuestSaveData(string qusetName, int amount)
    {
        if (QuestDatas.TryGetValue(qusetName, out QuestSaveData questSaveData))
        {
            questSaveData.Amount += amount;
            OnQuestSaveDataChange?.Invoke(questSaveData);
            SaveData();
            return;
        }

        QuestSaveData newQuestSaveData = new QuestSaveData()
        {
            QuestName = qusetName,
            Amount = amount
        };
        QuestDatas.Add(qusetName, newQuestSaveData);
        PlayerDataContainer.questSaveDatas.Add(newQuestSaveData);
        OnQuestSaveDataChange?.Invoke(newQuestSaveData);
        SaveData();
    }

    /// <summary>
    /// ����Ʈ ���̺� ������ ��ȯ�Լ�
    /// </summary>
    /// <param name="qusetName">��ȯ �� ����Ʈ �̸�</param>
    public QuestSaveData GetQuestData(string qusetName)
    {
        if (QuestDatas.TryGetValue(qusetName, out QuestSaveData questSaveData))
        {
            return questSaveData;
        }

        QuestSaveData newQuestSaveData = new QuestSaveData()
        {
            QuestName = qusetName,
        };

        QuestDatas.Add(qusetName, newQuestSaveData);
        PlayerDataContainer.questSaveDatas.Add(newQuestSaveData);
        return newQuestSaveData;
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="itemName">������ �̸�</param>
    /// <param name="itemAmount">���� ����</param>
    public void SetItemAmount(string itemName, int itemAmount)
    {
        if (ItemLevels.TryGetValue(itemName, out ItemLevelData itemLevelData))
        {
            itemLevelData.Amount += itemAmount;
            OnItemDataChange?.Invoke(itemLevelData);
            SaveData();
            return;
        }

        ItemLevelData newItemLevelData = new ItemLevelData()
        {
            itemName = itemName,
            Amount = itemAmount
        };
        ItemLevels.Add(itemName, newItemLevelData);
        PlayerDataContainer.itemLevelData.Add(newItemLevelData);
        OnItemDataChange?.Invoke(newItemLevelData);
        SaveData();
    }

    /// <summary>
    /// ������ ���� ���� �Լ�
    /// </summary>
    /// <param name="itemName">������ �̸�</param>
    /// <param name="itemAmount">������ �ʿ����� ���� (����� �Է�)</param>
    public void SetItemLevel(string itemName, int itemAmount)
    {
        ItemLevels[itemName].level++;
        SetItemAmount(itemName, -itemAmount);
    }

    public bool IsItemPossess(string itemName)
    {
        return ItemLevels.ContainsKey(itemName);
    }

    /// <summary>
    /// ���� ���� Ȯ�� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="mailName">���� �̸�</param>
    public void SetMailData(string mailName)
    {
        MailSaveData mailSaveData = new MailSaveData() { mailName = mailName };

        MailSaveDatas.Add(mailName, mailSaveData);
        PlayerDataContainer.mailSaveDatas.Add(mailSaveData);
        SaveData();
    }

    void SaveData()
    {
#if UNITY_EDITOR
        return;
#endif

        OpenSaveGame();
    }

    void OpenSaveGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(FILENAME,
                                                            DataSource.ReadCacheOrNetwork,
                                                            ConflictResolutionStrategy.UseLastKnownGood,
                                                            OnsavedGameOpened);
    }

    void OnsavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();

            string json = JsonUtility.ToJson(PlayerDataContainer);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            savedGameClient.CommitUpdate(game, update, bytes, OnSavedGameWritten);
        }
    }

    void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("���� ����");
        }
    }
}
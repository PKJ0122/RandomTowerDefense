using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerData : SingletonMonoBase<PlayerData>
{
    private const string FILENAME = "PlayerData.dat";

    public static Dictionary<UnitKind, UnitLevelData> unitLevels = new Dictionary<UnitKind, UnitLevelData>(8);
    public static Dictionary<string, ItemLevelData> itemLevels = new Dictionary<string, ItemLevelData>(10);
    public static Dictionary<string, QuestSaveData> questDatas = new Dictionary<string, QuestSaveData>(9);
    public static Dictionary<UnitKind, BeyondCraftingData> beyondCraftingDatas = new Dictionary<UnitKind, BeyondCraftingData>(8);
    public static Dictionary<string, MailSaveData> mailSaveDatas = new Dictionary<string, MailSaveData>(5);

    static PlayerDataContainer s_playerDataContainer;
    public PlayerDataContainer PlayerDataContainer
    {
        get
        {
            if (s_playerDataContainer == null)
                s_playerDataContainer = new PlayerDataContainer();

            return s_playerDataContainer;
        }
        set
        {
            s_playerDataContainer = value;
            PlayerDataSetting();
        }
    }

    public string PlayerName
    {
        get => PlayerDataContainer.playerName;
        set
        {
            PlayerDataContainer.playerName = value;
            OnPlayerNameChange?.Invoke(value);
            SaveData();
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



    public void PlayerDataSetting()
    {
        foreach (UnitLevelData unitLevelData in PlayerDataContainer.unitLevelDatas)
        {
            unitLevels.Add(unitLevelData.unitKind, unitLevelData);
        }
        foreach (ItemLevelData itemLevelData in PlayerDataContainer.itemLevelData)
        {
            itemLevels.Add(itemLevelData.itemName, itemLevelData);
        }
        foreach (QuestSaveData questSaveData in PlayerDataContainer.questSaveDatas)
        {
            questDatas.Add(questSaveData.QuestName, questSaveData);
        }
        foreach (BeyondCraftingData beyondCraftingData in PlayerDataContainer.beyondCraftingDatas)
        {
            beyondCraftingDatas.Add(beyondCraftingData.unitKind, beyondCraftingData);
        }
        foreach (MailSaveData mailSaveData in PlayerDataContainer.mailSaveDatas)
        {
            mailSaveDatas.Add(mailSaveData.mailName, mailSaveData);
        }
    }

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
    /// ��ǥ ������ ����ġ �÷��ִ� �Լ�
    /// </summary>
    /// <param name="unitKind">���� ����</param>
    /// <param name="experience">���� ����ġ</param>
    public void SetUnitLevel(UnitKind unitKind, int pulsExperience)
    {
        if (unitLevels.TryGetValue(unitKind, out UnitLevelData unitLevelData))
        {
            unitLevelData.experience += pulsExperience;
            OnUnitDataChange?.Invoke(unitLevelData);
            SaveData();
            return;
        }

        UnitLevelData newUnitLevelData = new UnitLevelData()
        {
            unitKind = unitKind,
            experience = pulsExperience
        };
        unitLevels.Add(unitKind, newUnitLevelData);
        PlayerDataContainer.unitLevelDatas.Add(newUnitLevelData);
        OnUnitDataChange?.Invoke(newUnitLevelData);
        SaveData();
    }

    /// <summary>
    /// ����Ʈ ���� �����Լ�
    /// </summary>
    /// <param name="qusetName"></param>
    /// <param name="amount"></param>
    public void SetQuestSaveData(string qusetName, int amount)
    {
        if (questDatas.TryGetValue(qusetName, out QuestSaveData questSaveData))
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
        questDatas.Add(qusetName, newQuestSaveData);
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
        if (questDatas.TryGetValue(qusetName, out QuestSaveData questSaveData))
        {
            return questSaveData;
        }

        QuestSaveData newQuestSaveData = new QuestSaveData()
        {
            QuestName = qusetName,
        };

        questDatas.Add(qusetName, newQuestSaveData);
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
        if (itemLevels.TryGetValue(itemName, out ItemLevelData itemLevelData))
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
        itemLevels.Add(itemName, newItemLevelData);
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
        itemLevels[itemName].level++;
        SetItemAmount(itemName, -itemAmount);
    }

    public bool IsItemPossess(string itemName)
    {
        return itemLevels.ContainsKey(itemName);
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
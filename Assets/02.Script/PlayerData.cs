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
        }
    }
    public UnitKind PlayerCharacter
    {
        get => PlayerDataContainer.playerCharacter;
        set
        {
            PlayerDataContainer.playerCharacter = value;
            OnUnitCharacterChange?.Invoke(PlayerCharacter, PlayerCharacterRank);
        }
    }
    public UnitRank PlayerCharacterRank
    {
        get => PlayerDataContainer.playerCharacterRank;
        set
        {
            PlayerDataContainer.playerCharacterRank = value;
            OnUnitCharacterChange?.Invoke(PlayerCharacter, PlayerCharacterRank);
        }
    }
    public int Gold
    {
        get => PlayerDataContainer.gold;
        set
        {
            PlayerDataContainer.gold = value;
            OnGoldChange?.Invoke(value);
        }
    }
    public int Diamond
    {
        get => PlayerDataContainer.diamond;
        set
        {
            PlayerDataContainer.diamond = value;
            OnDiamondChange?.Invoke(value);
        }
    }

    public static event Action<string> OnPlayerNameChange;
    public static event Action<UnitKind, UnitRank> OnUnitCharacterChange;
    public static event Action<int> OnGoldChange;
    public static event Action<int> OnDiamondChange;
    public static event Action<UnitLevelData> OnUnitDataChange;
    public static event Action<ItemLevelData> OnItemDataChange;


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
        OnPlayerNameChange += value => SaveData();
        OnUnitCharacterChange += (value1,value2) => SaveData();
        OnGoldChange += value => SaveData();
        OnDiamondChange += value => SaveData();
        OnUnitDataChange += value => SaveData();
        OnItemDataChange += value => SaveData();
    }

    /// <summary>
    /// ��ǥ ������ ����ġ �÷��ִ� �Լ�
    /// </summary>
    /// <param name="unitKind">���� ����</param>
    /// <param name="experience">���� ����ġ</param>
    public void SetUnitLevel(UnitKind unitKind,int pulsExperience)
    {
        if (unitLevels.TryGetValue(unitKind, out UnitLevelData unitLevelData))
        {
            unitLevelData.experience += pulsExperience;
            OnUnitDataChange?.Invoke(unitLevelData);
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
    }

    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="itemName">������ �̸�</param>
    /// <param name="itemAmount">���� ����</param>
    public void SetItemAmount(string itemName,int itemAmount)
    {
        if (itemLevels.TryGetValue(itemName, out ItemLevelData itemLevelData))
        {
            itemLevelData.Amount += itemAmount;
            OnItemDataChange?.Invoke(itemLevelData);
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
    }

    /// <summary>
    /// ������ ���� ���� �Լ�
    /// </summary>
    /// <param name="itemName">������ �̸�</param>
    /// <param name="itemAmount">������ �ʿ����� ����</param>
    public void SetItemLevel(string itemName, int itemAmount)
    {
        itemLevels[itemName].Amount -= itemAmount;
        itemLevels[itemName].level++;
    }

    void SaveData()
    {
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
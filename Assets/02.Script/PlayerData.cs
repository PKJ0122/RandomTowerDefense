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
    public int ItemSummons
    {
        get => PlayerDataContainer.itemSummons;
        set
        {
            PlayerDataContainer.itemSummons = value;
            OnItemSummonsChange?.Invoke(value);
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
        }
    }

    public static event Action<string> OnPlayerNameChange;
    public static event Action<UnitKind, UnitRank> OnUnitCharacterChange;
    public static event Action<int> OnGoldChange;
    public static event Action<int> OnDiamondChange;
    public static event Action<int> OnItemSummonsChange;
    public static event Action OnLastShopChangeChange;
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
        OnUnitCharacterChange += (value1, value2) => SaveData();
        OnGoldChange += value => SaveData();
        OnDiamondChange += value => SaveData();
        OnUnitDataChange += value => SaveData();
        OnItemDataChange += value => SaveData();
        OnLastShopChangeChange += SaveData;
    }

    /// <summary>
    /// 상점 세이브 데이터 설정 함수
    /// </summary>
    /// <param name="index">수정할 인덱스 넘버</param>
    /// <param name="shopSaveData">수정할 상점 데이터</param>
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
    /// 대표 유닛의 경험치 올려주는 함수
    /// </summary>
    /// <param name="unitKind">유닛 종류</param>
    /// <param name="experience">유닛 경험치</param>
    public void SetUnitLevel(UnitKind unitKind, int pulsExperience)
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
    /// 아이템 갯수 함수
    /// </summary>
    /// <param name="itemName">아이템 이름</param>
    /// <param name="itemAmount">변경 개수</param>
    public void SetItemAmount(string itemName, int itemAmount)
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
    /// 아이템 레벨 증가 함수
    /// </summary>
    /// <param name="itemName">아이템 이름</param>
    /// <param name="itemAmount">레벨업 필요조건 개수 (양수로 입력)</param>
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
            Debug.Log("저장 성공");
        }
    }

}
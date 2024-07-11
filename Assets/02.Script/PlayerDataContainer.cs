using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDataContainer
{
    public string playerName = "이름없는";
    public UnitKind playerCharacter = UnitKind.Spike;
    public UnitRank playerCharacterRank = UnitRank.Nomal;
    public int gold;
    public int diamond;
    public int itemSummons;
    public string lastShopChange = "1999-01-22";

    public bool freeDiamondBuy;
    public bool adItemBuy;
    public bool adDiamondBuy;

    public List<ShopSaveData> shopSaveDatas = new List<ShopSaveData>(5);
    public List<UnitLevelData> unitLevelDatas = new List<UnitLevelData>(8);
    public List<ItemLevelData> itemLevelData = new List<ItemLevelData>(10);
}
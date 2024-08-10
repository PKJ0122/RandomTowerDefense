using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDataContainer
{
    public int gold;
    public int diamond;
    public int itemSummons;
    public string lastShopChange = "1999-01-22";

    public bool freeDiamondBuy;
    public bool adItemBuy;
    public bool adDiamondBuy;

    public List<ShopSaveData> shopSaveDatas = new List<ShopSaveData>(5);
    public List<ItemLevelData> itemLevelData = new List<ItemLevelData>(10);
    public List<QuestSaveData> questSaveDatas = new List<QuestSaveData>(8);
    public List<BeyondCraftingData> beyondCraftingDatas = new List<BeyondCraftingData>(8);
    public List<MailSaveData> mailSaveDatas = new List<MailSaveData>(5);
}
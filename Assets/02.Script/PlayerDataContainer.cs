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

    public List<UnitLevelData> unitLevelDatas = new List<UnitLevelData>(8);
    public List<ItemLevelData> itemLevelData = new List<ItemLevelData>(10);
}
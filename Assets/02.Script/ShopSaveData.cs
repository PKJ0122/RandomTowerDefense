using System;

[Serializable]
public class ShopSaveData
{
    public string itemName;
    public int price;
    public int priceKind;
    public bool isBuy;

    /// <summary>
    /// 상점 아이템 세이브 데이터 생성자
    /// </summary>
    /// <param name="itemName">아이템 이름</param>
    /// <param name="price">아이템 가격</param>
    /// <param name="priceKind">아이템 구매 재화</param>
    public ShopSaveData(string itemName, int price, int priceKind)
    {
        this.itemName = itemName;
        this.price = price;
        this.priceKind = priceKind;
    }
}
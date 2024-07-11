using System;

[Serializable]
public class ShopSaveData
{
    public string itemName;
    public int price;
    public int priceKind;
    public bool isBuy;

    /// <summary>
    /// ���� ������ ���̺� ������ ������
    /// </summary>
    /// <param name="itemName">������ �̸�</param>
    /// <param name="price">������ ����</param>
    /// <param name="priceKind">������ ���� ��ȭ</param>
    public ShopSaveData(string itemName, int price, int priceKind)
    {
        this.itemName = itemName;
        this.price = price;
        this.priceKind = priceKind;
    }
}
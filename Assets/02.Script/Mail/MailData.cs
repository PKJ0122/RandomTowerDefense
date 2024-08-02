using System;

[Serializable]
public class MailData
{
    public string mailName;
    public string mailDetail;
    public string startReceive;
    public string endReceive;
    public int diamondAmount;

    public MailData(string mailName, string mailDetail, string startReceive, string endReceive, int diamondAmount)
    {
        this.mailName = mailName;
        this.mailDetail = mailDetail;
        this.startReceive = startReceive;
        this.endReceive = endReceive;
        this.diamondAmount = diamondAmount;
    }
}
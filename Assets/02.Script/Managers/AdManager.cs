using GoogleMobileAds.Api;

public class AdManager : SingletonMonoBase<AdManager>
{
    RewardedAd _rewardedAd;
    public RewardedAd RewardedAd
    {
        get
        {
            if (_rewardedAd == null)
            {
                MobileAds.Initialize(initStatus => { });
                _rewardedAd = new RewardedAd(adUnitId);
            }
            return _rewardedAd;
        }
    }
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
    string adUnitId = "adUnitId";
#endif

    //_rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
    //_rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
    //_rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ������ ǥ�õ� �� ȣ��(��� ȭ���� ����)
    //_rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
    //_rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ������ ��û�� �� ������ �޾ƾ��� �� ȣ��
    //_rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ������ ���� �� ȣ��

    public void ShowAds()
    {
        AdRequest request = new AdRequest.Builder().Build();
        RewardedAd.LoadAd(request);
        if (RewardedAd.IsLoaded())
        {
            RewardedAd.Show();
        }
    }
}
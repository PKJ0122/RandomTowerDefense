using GoogleMobileAds.Api;
using UnityEngine;

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
    string adUnitId = "ca-app-pub-5639813524802030/4478427050";
#else
    string adUnitId = "adUnitId";
#endif

    protected override void Awake()
    {
        base.Awake();
        if (_rewardedAd == null)
        {
            MobileAds.Initialize(initStatus => { });
            _rewardedAd = new RewardedAd(adUnitId);
        }
        AdRequest request = new AdRequest.Builder().Build();
        RewardedAd.LoadAd(request);
    }

    //_rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
    //_rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
    //_rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
    //_rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
    //_rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
    //_rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��

    public void ShowAds()
    {
        if (RewardedAd.IsLoaded())
        {
            RewardedAd.Show();
        }
        else
        {
            UIManager.Instance.Get<PopUpUI>().Show("���� �ҷ����� ���߽��ϴ�.");
        }
    }
}

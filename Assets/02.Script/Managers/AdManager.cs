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
    string adUnitId = "ca-app-pub-5639813524802030/4478427050";
#else
    string adUnitId = "adUnitId";
#endif

    //_rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
    //_rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
    //_rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
    //_rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
    //_rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
    //_rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출

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

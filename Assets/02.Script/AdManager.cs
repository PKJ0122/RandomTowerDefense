using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdManager : SingletonMonoBase<AdManager>
{
    RewardedAd _rewardedAd;
    public RewardedAd RewardedAd => _rewardedAd;

    string adUnitId;


    public void Start()
    {
        // 모바일 광고 SDK를 초기화함.
        MobileAds.Initialize(initStatus => { });

        //adUnitId 설정
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
        adUnitId = "adUnitId";
#endif

        //광고 로드 : RewardedAd 객체의 loadAd메서드에 AdRequest 인스턴스를 넣음
        AdRequest request = new AdRequest.Builder().Build();
        _rewardedAd = new RewardedAd(adUnitId);
        _rewardedAd.LoadAd(request);

        //_rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
        //_rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
        //_rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
        //_rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
        //_rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
        //_rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출
    }

    public void ShowAds()
    {
        if (_rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
            AdRequest request = new AdRequest.Builder().Build();
            _rewardedAd.LoadAd(request);
        }
    }
}

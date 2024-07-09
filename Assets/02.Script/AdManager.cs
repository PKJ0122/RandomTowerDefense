using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    RewardedAd rewardedAd;

    string adUnitId;

    public void Start()
    {
        // ����� ���� SDK�� �ʱ�ȭ��.
        MobileAds.Initialize(initStatus => { });

        //adUnitId ����
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
        adUnitId = "adUnitId";
#endif

        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);



        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) { }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) { }

    public void HandleRewardedAdFailedToShow(object sender, EventArgs args) { }

    public void HandleRewardedAdClosed(object sender, EventArgs args) { }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
    }

    public void ShowAds()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
            AdRequest request = new AdRequest.Builder().Build();
            this.rewardedAd.LoadAd(request);
        }
    }
}

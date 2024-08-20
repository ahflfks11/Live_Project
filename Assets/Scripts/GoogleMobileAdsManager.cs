using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleMobileAdsManager : MonoBehaviour
{

    string _adUnitId;
    InterstitialAd _interstitialAd;

    private void Awake()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {
        // 이전에 로드된 광고가 있는지 확인하고 있다면 제거하고 해제한다.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // 새로 광고를 로드하기위한 요청을 생성한다.
        var adRequest = new AdRequest();

        // 광고단위 ID _adUnitId와 adRequest 객체를 전달받아 광고를 로드한다.
        InterstitialAd.Load(_adUnitId, adRequest,
        (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : "
                    + ad.GetResponseInfo());

            _interstitialAd = ad;
        // 성공한 경우 로드된 광고에 대한 이벤트 핸들러를 등록한다.
        RegisterEventHandlers(_interstitialAd);
        });
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // 광고 지급 관련 이벤트
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // 
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // 광고가 클릭되었을때 이벤트
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // 전면 광고가 열렸을때 호출
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // 전면 광고가 닫혔을때 호출
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("close Scene");
        };
        // 전면 광고가 열리지 못했을때 호출
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    public void ShowAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            LoadInterstitialAd(); //광고 재로드
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }
}

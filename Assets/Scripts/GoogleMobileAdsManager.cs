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
        // ������ �ε�� ���� �ִ��� Ȯ���ϰ� �ִٸ� �����ϰ� �����Ѵ�.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // ���� ���� �ε��ϱ����� ��û�� �����Ѵ�.
        var adRequest = new AdRequest();

        // ������� ID _adUnitId�� adRequest ��ü�� ���޹޾� ���� �ε��Ѵ�.
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
        // ������ ��� �ε�� ���� ���� �̺�Ʈ �ڵ鷯�� ����Ѵ�.
        RegisterEventHandlers(_interstitialAd);
        });
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // ���� ���� ���� �̺�Ʈ
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
        // ���� Ŭ���Ǿ����� �̺�Ʈ
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // ���� ���� �������� ȣ��
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // ���� ���� �������� ȣ��
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("close Scene");
        };
        // ���� ���� ������ �������� ȣ��
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
            LoadInterstitialAd(); //���� ��ε�
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }
}

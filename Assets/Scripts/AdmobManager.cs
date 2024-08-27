using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;

public class AdmobManager : MonoBehaviour
{
    private static AdmobManager instance;
    private InterstitialAd _interstitialAd;
    public string _adUnitId;

    public static AdmobManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    public void Start()
    {
        LoadInterstitialAd();
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

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // ���� ���� ���� �̺�Ʈ
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            GPGSManager.Instance.GaveCrystal(100, LobbyManager.Instance._lobbyUIManager._CashText);
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));

            Debug.Log(adValue.Value + ", " + adValue.CurrencyCode);
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
}

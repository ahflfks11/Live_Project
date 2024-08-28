using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    private RewardedAd rewardedAd;
    
    // Start is called before the first frame update
    void Awake()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    private void Start()
    {
        LoadRewardedAd();
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        if(rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad");

        var adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
         {
             if (error != null || ad == null)
             {
                 Debug.LogError("Reward ad  failed to load an ad " +
                     "with error : " + error);

                 return;
             }

             Debug.Log("Rewarded ad loaded with response : "
                 + ad.GetResponseInfo());

             rewardedAd = ad;
         });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));

                GPGSManager.Instance.GaveCrystal(100, LobbyManager.Instance._lobbyUIManager._CashText);
            });
        }
    }
}

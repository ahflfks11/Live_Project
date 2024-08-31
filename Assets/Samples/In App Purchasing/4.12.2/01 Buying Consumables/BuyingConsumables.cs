using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Samples.Purchasing.Core.BuyingConsumables
{
    public class BuyingConsumables : MonoBehaviour, IDetailedStoreListener
    {
        IStoreController m_StoreController; // The Unity Purchasing system.
        private const string Environment = "production";
        // Your products IDs. They should match the ids of your products in your store.
        string goldProductIdLarge = "gold_buy_50000";
        string diamondProductIdLarge = "crystal_buy_1000";
        string goldProductIdSmall = "gold_buy_5000";
        string diamondProductIdSmall = "crystal_buy_500";

        void Start()
        {
            InitializePurchasing();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add products that will be purchasable and indicate its type.
            builder.AddProduct(goldProductIdLarge, ProductType.Consumable);
            builder.AddProduct(diamondProductIdLarge, ProductType.Consumable);
            builder.AddProduct(goldProductIdSmall, ProductType.Consumable);
            builder.AddProduct(diamondProductIdSmall, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }

        public void BuyGoldLarge()
        {
            m_StoreController.InitiatePurchase(goldProductIdLarge);
        }

        public void BuyDiamondLarge()
        {
            m_StoreController.InitiatePurchase(diamondProductIdLarge);
        }

        public void BuyGoldSmall()
        {
            m_StoreController.InitiatePurchase(goldProductIdSmall);
        }

        public void BuyDiamondSmall()
        {
            m_StoreController.InitiatePurchase(diamondProductIdSmall);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            m_StoreController = controller;

            // 앱 재시작 시 보상 누락을 체크하여 처리
            CheckPendingRewards();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // Retrieve the purchased product
            var product = args.purchasedProduct;

            // Add the purchased product to the player's inventory and save the transaction
            if (product.definition.id == goldProductIdLarge)
            {
                GrantReward(goldProductIdLarge, () => AddGoldLarge());
            }
            else if (product.definition.id == diamondProductIdLarge)
            {
                GrantReward(diamondProductIdLarge, () => AddDiamondLarge());
            }
            else if (product.definition.id == goldProductIdSmall)
            {
                GrantReward(goldProductIdSmall, () => AddGoldSmall());
            }
            else if (product.definition.id == diamondProductIdSmall)
            {
                GrantReward(diamondProductIdSmall, () => AddDiamondSmall());
            }

            Debug.Log($"Purchase Complete - Product: {product.definition.id}");

            // We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                $" Purchase failure reason: {failureDescription.reason}," +
                $" Purchase failure details: {failureDescription.message}");
        }

        // 보상 지급 함수들
        void AddGoldLarge()
        {
            GPGSManager.Instance.GaveGold(50000, LobbyManager.Instance._lobbyUIManager._CoinText);
        }

        void AddDiamondLarge()
        {
            GPGSManager.Instance.GaveCrystal(1600, LobbyManager.Instance._lobbyUIManager._CashText);
        }

        void AddGoldSmall()
        {
            GPGSManager.Instance.GaveGold(10000, LobbyManager.Instance._lobbyUIManager._CoinText);
        }

        void AddDiamondSmall()
        {
            GPGSManager.Instance.GaveCrystal(500, LobbyManager.Instance._lobbyUIManager._CashText);
        }

        // 보상 지급을 확인하고 처리하는 함수
        void GrantReward(string productId, Action rewardAction)
        {
            if (!PlayerPrefs.HasKey(productId))
            {
                // 보상 지급 상태 저장 (중복 방지)
                PlayerPrefs.SetInt(productId, 1);

                // 보상 지급 실행
                rewardAction.Invoke();

                // 보상 지급 완료 후 상태 제거 (보상이 1회만 지급되도록)
                PlayerPrefs.DeleteKey(productId);
            }
        }

        // 앱 재시작 시 보상 누락 확인 및 처리 함수
        void CheckPendingRewards()
        {
            if (PlayerPrefs.GetInt(goldProductIdLarge, 0) == 1)
            {
                GrantReward(goldProductIdLarge, () => AddGoldLarge());
            }

            if (PlayerPrefs.GetInt(diamondProductIdLarge, 0) == 1)
            {
                GrantReward(diamondProductIdLarge, () => AddDiamondLarge());
            }

            if (PlayerPrefs.GetInt(goldProductIdSmall, 0) == 1)
            {
                GrantReward(goldProductIdSmall, () => AddGoldSmall());
            }

            if (PlayerPrefs.GetInt(diamondProductIdSmall, 0) == 1)
            {
                GrantReward(diamondProductIdSmall, () => AddDiamondSmall());
            }
        }
    }
}
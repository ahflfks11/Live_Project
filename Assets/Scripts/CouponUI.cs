using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CouponUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _couponText;
    DOTweenVisualManager _visualManager;
    public DOTweenVisualManager VisualManager { get => _visualManager; set => _visualManager = value; }
    private void Start()
    {
        VisualManager = GetComponent<DOTweenVisualManager>();
    }

    public void SendCoupon()
    {
        if (GPGSManager.Instance.UseCoupons(_couponText.text))
        {
            Debug.Log("success");
        }
    }
}

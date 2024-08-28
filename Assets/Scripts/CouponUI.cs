using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CouponUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _couponText;
    [SerializeField] private TMP_Text _successText;
    [SerializeField] private Transform _CouponTransform;

    private string _values;

    DOTweenVisualManager _visualManager;
    public DOTweenVisualManager VisualManager { get => _visualManager; set => _visualManager = value; }

    private void Start()
    {
        VisualManager = GetComponent<DOTweenVisualManager>();
    }

    public void SetValues(string _text)
    {
        _values = _text;
    }

    public void SendCoupon()
    {
        TMP_Text _success = null;
        if (GPGSManager.Instance.UseCoupons(_values.ToUpper()))
        {
            _success = Instantiate(_successText, Vector2.zero, Quaternion.identity);
            _success.text = "���� �Է¿� �����Ͽ����ϴ�!";
        }
        else
        {
            _success = Instantiate(_successText, Vector2.zero, Quaternion.identity);
            _success.text = "<color=#FF0000>������ �̹� ���Ǿ��ų� ���� �����Դϴ�.</color>";
        }

        _success.transform.SetParent(_CouponTransform);
        _success.transform.position = Vector3.zero;
        _success.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    LobbyManager _lobbyManager;
    GPGSManager _gpgsManager;
    public TMP_InputField _NickLabel;
    public TMP_Text _name;
    public GameObject _popup;
    public TMP_Text _CoinText;
    public TMP_Text _CashText;
    public GameObject _shopUI;
    public GameObject _gachaShopUI;

    private void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _gpgsManager = FindObjectOfType<GPGSManager>();

        if (_gpgsManager != null)
        {
            _name.text = _gpgsManager.TakeNick();
        }
    }

    public void ShopPanel()
    {
        if (_shopUI.activeSelf)
        {
            _shopUI.SetActive(false);
        }
        else
        {
            _shopUI.SetActive(true);
        }
    }

    public void GachaShopPanel()
    {
        if (_gachaShopUI.activeSelf)
        {
            _gachaShopUI.SetActive(false);
        }
        else
        {
            _gachaShopUI.SetActive(true);
        }
    }

    public void SetCoinText(string _coin)
    {
        _CoinText.text = _coin;
    }

    public void SetCashText(string _cash)
    {
        _CashText.text = _cash;
    }

    public void NickDecision()
    {
        if (_gpgsManager != null)
        {
            if (_gpgsManager.CreateNick(_NickLabel.text))
            {
                _lobbyManager.CharacterAttack();
                _name.text = _gpgsManager.TakeNick();
                _popup.SetActive(false);
            }
        }
    }

    private void Update()
    {
    }
}

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
    public DG.Tweening.DOTweenVisualManager _multiGachaUIPanel;
    public DG.Tweening.DOTweenVisualManager _singleGachaUIPanel;
    public DG.Tweening.DOTweenVisualManager _gachaShopUI;

    public Gacha_Cotents _multiGachaContent;
    public Gacha_Cotents _singleGachaContent;

    public Gacha_Cotents MultiGachaContent { get => _multiGachaContent; set => _multiGachaContent = value; }
    public Gacha_Cotents SingleGachaContent { get => _singleGachaContent; set => _singleGachaContent = value; }

    private void Start()
    {
        _lobbyManager = FindObjectOfType<LobbyManager>();
        _gpgsManager = FindObjectOfType<GPGSManager>();
        MultiGachaContent = _multiGachaUIPanel.GetComponent<Gacha_Cotents>();
        SingleGachaContent = _singleGachaUIPanel.GetComponent<Gacha_Cotents>();

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
        if (_gachaShopUI.enabled)
        {
            _gachaShopUI.enabled =false;
        }
        else
        {
            _gachaShopUI.enabled =true;
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

    public void CloseLootbox()
    {
        Destroy(GameObject.Find("LootBox"));
    }

    public void MultiGachaCleanSpr()
    {
        _multiGachaContent.CleanGachaImage();
    }

    public void SingleGachaCleanSpr()
    {
        _singleGachaContent.CleanGachaImage();
    }

    public void MultiGachaUI()
    {
        SingleGachaCleanSpr();

        if (_multiGachaUIPanel.enabled)
        {
            CloseLootbox();
            _multiGachaUIPanel.enabled = false;
        }
        else
        {
            _multiGachaUIPanel.enabled = true;
        }
    }

    public void SingleGachaUI()
    {
        MultiGachaCleanSpr();

        if (_singleGachaUIPanel.enabled)
        {
            CloseLootbox();
            _singleGachaUIPanel.enabled = false;
        }
        else
        {
            _singleGachaUIPanel.enabled = true;
        }
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

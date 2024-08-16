using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;

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
    public Transform _rankTransform;
    public Sprite[] _rankImg;
    public DOTweenVisualManager _multiGachaUIPanel;
    public DOTweenVisualManager _singleGachaUIPanel;
    public DOTweenVisualManager _gachaShopUI;
    [SerializeField] private DOTweenVisualManager _character_InvenPanel;
    [SerializeField] private DOTweenVisualManager _settingUI;
    [SerializeField] private DOTweenVisualManager _rankingPanel;
    [SerializeField] private Transform _Inven_Contents;
    [SerializeField] private ErrorGame _errorPanel;
    [SerializeField] private RankingUI _rankingUI;
    Gacha_Cotents _multiGachaContent;
    Gacha_Cotents _singleGachaContent;

    [SerializeField] CharacterStatus _status;

    [SerializeField] private Character_Icon _icon;

    public Gacha_Cotents MultiGachaContent { get => _multiGachaContent; set => _multiGachaContent = value; }
    public Gacha_Cotents SingleGachaContent { get => _singleGachaContent; set => _singleGachaContent = value; }
    public DOTweenVisualManager Character_InvenPanel { get => _character_InvenPanel; set => _character_InvenPanel = value; }
    public CharacterStatus Status { get => _status; set => _status = value; }
    public LobbyManager LobbyManager { get => _lobbyManager; set => _lobbyManager = value; }
    public RankingUI RankingUI { get => _rankingUI; set => _rankingUI = value; }

    private void Start()
    {
        LobbyManager = FindObjectOfType<LobbyManager>();
        _gpgsManager = FindObjectOfType<GPGSManager>();
        MultiGachaContent = _multiGachaUIPanel.GetComponent<Gacha_Cotents>();
        SingleGachaContent = _singleGachaUIPanel.GetComponent<Gacha_Cotents>();

        if (_gpgsManager != null)
        {
            _name.text = _gpgsManager.TakeNick();
        }
    }

    public void RankUIPanel()
    {
        if (!_rankingPanel.enabled)
            _rankingPanel.enabled = true;
        else
            _rankingPanel.enabled = false;
    }

    public void FeedBackURL()
    {
        Application.OpenURL("https://cafe.naver.com/legenddefence");
    }

    public void SettingUI()
    {
        if (_settingUI.enabled)
            _settingUI.enabled = false;
        else
            _settingUI.enabled = true;
    }

    public void ErrorPanel()
    {
        if (_errorPanel.gameObject.activeSelf)
            _errorPanel.gameObject.SetActive(false);
        else
            _errorPanel.gameObject.SetActive(true);
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

    public void ChacterInvenPanel()
    {
        if (_character_InvenPanel.enabled)
        {
            _character_InvenPanel.enabled = false;
        }
        else
        {
            _character_InvenPanel.enabled = true;
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
        if (_NickLabel.text == "")
            return;

        if (_gpgsManager != null)
        {
            if (_gpgsManager.CreateNick(_NickLabel.text))
            {
                //_lobbyManager.CharacterAttack();
                _name.text = _gpgsManager.TakeNick();
            }
        }
        if (JsonParseManager.Instance._txtNumber != 7)
            DialogueManager.Instance.SignPanelUI();
        else
            Transitioner.Instance.TransitionToScene(1);
    }

    public void CreateIcon(DataManager.Data _data, int _number)
    {
        Character_Icon Inven_Icon = Instantiate(_icon, Vector3.zero, Quaternion.identity);
        Inven_Icon.gameObject.name = _data._rarelity.ToString();
        Inven_Icon.transform.SetParent(_Inven_Contents);
        Inven_Icon.transform.localScale = _icon.transform.localScale;
        Inven_Icon.MyData = _data;
        Inven_Icon.Number = _number;
        Inven_Icon.SetImage(_data);

        IconPosChange();
    }

    public void IconPosChange()
    {
        GameObject[] _icons = GameObject.FindGameObjectsWithTag("Inven_Icon");
        //OrderBy <==> OrderByDescending
        _icons = _icons.OrderByDescending(go => go.name).ToArray();
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].transform.SetSiblingIndex(i);
        }
    }
}

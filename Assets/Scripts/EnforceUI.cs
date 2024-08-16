using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class EnforceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelUpLabel;

    [SerializeField] private TMP_Text _dmgUpNormalCoinText;
    [SerializeField] private TMP_Text _dmgUpRareCoinText;
    [SerializeField] private TMP_Text _dmgUpLegendCoinText;
    [SerializeField] private TMP_Text _dmgUpHiddenCoinText;
    [SerializeField] private TMP_Text _levelUpCoinText;
    [SerializeField] private TMP_Text _levelUpRateText;

    [SerializeField] Color[] _color;

    [SerializeField] private List<float> _normalEnforce_Coin;
    [SerializeField] private List<float> _rareEnforce_Coin;
    [SerializeField] private List<float> _legendEnforce_Coin;
    [SerializeField] private List<float> _hiddenEnforce_Coin;
    [SerializeField] private GameObject _vfx;
    [SerializeField] private Transform _vfxPos;

    [SerializeField] private GameObject _arrow;

    int _normalLevel = 0;
    int _rareLevel = 0;
    int _legendLevel = 0;
    int _hiddenLevel = 0;
    int[] _levelGold = { 100, 150, 200, -1 };
    string[] _levelText = { "희귀함", "전설적인", "전설적인" };
    string[] _rateText = { "확률이 대량 증가한다.", "확률이 소량 증가한다.", "확률이 소량 증가한다." };
    int _levelNumber = 0;

    public void OpenVFX()
    {
        GameObject openfx = Instantiate(_vfx, _vfxPos.position, Quaternion.identity);
        Destroy(openfx, 5f);
    }

    public void Start()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            _arrow.SetActive(true);
        }
        else
        {
            _arrow.SetActive(false);
        }
    }

    public bool SetDmgUpNormalCoin()
    {
        if (_normalEnforce_Coin[_normalLevel] > GameManager.Instance.Gold)
            return false;

        GameManager.Instance.Gold -= (int)_normalEnforce_Coin[_normalLevel];

        if (_normalLevel + 1 < _normalEnforce_Coin.Count)
            _normalLevel++;

        _dmgUpNormalCoinText.text = _normalEnforce_Coin[_normalLevel].ToString();

        return true;
    }

    public bool SetDmgUpRareCoin()
    {
        if (_rareEnforce_Coin[_rareLevel] > GameManager.Instance.Gold)
            return false;

        GameManager.Instance.Gold -= (int)_rareEnforce_Coin[_rareLevel];

        if (_rareLevel + 1 < _rareEnforce_Coin.Count)
            _rareLevel++;

        _dmgUpRareCoinText.text = _rareEnforce_Coin[_rareLevel].ToString();

        return true;
    }

    public bool SetDmgUpLegendCoin()
    {
        if (_legendEnforce_Coin[_legendLevel] > GameManager.Instance.Gold)
            return false;
        
        GameManager.Instance.Gold -= (int)_legendEnforce_Coin[_legendLevel];

        if (_legendLevel + 1 < _legendEnforce_Coin.Count)
            _legendLevel++;

        _dmgUpLegendCoinText.text = _legendEnforce_Coin[_legendLevel].ToString();


        return true;
    }

    public bool SetDmgUpHiddenCoin()
    {
        if (_hiddenEnforce_Coin[_hiddenLevel] > GameManager.Instance.Gold)
            return false;
        
        GameManager.Instance.Gold -= (int)_hiddenEnforce_Coin[_hiddenLevel];

        if (_hiddenLevel + 1 < _hiddenEnforce_Coin.Count)
            _hiddenLevel++;

        _dmgUpHiddenCoinText.text = _hiddenEnforce_Coin[_hiddenLevel].ToString();


        return true;
    }

    public void SpawnScroll(int _scrollNumber)
    {
        if (GameManager.Instance.RarilitySpawnCount[_scrollNumber] < 1 || GameManager.Instance.RarilitySpawnCount.Length <= _scrollNumber)
            return;

        GameManager.Instance.UiManager.EnforceShopPanel();
        GameManager.Instance.UnitManager.SpecialRalitySpawn(_scrollNumber, GameManager.Instance.myArea.position);
        GameManager.Instance.RarilitySpawnCount[_scrollNumber]--;
    }

    public bool SetLevelText()
    {
        if (GameManager.Instance.Gold < _levelGold[_levelNumber] || _levelGold[_levelNumber] == -1)
            return false;

        GameManager.Instance.Gold -= _levelGold[_levelNumber];

        _levelNumber++;

        if (_levelGold[_levelNumber] == -1)
        {
            _levelUpCoinText.text = "MAX";
        }
        else
        {
            _levelUpCoinText.text = _levelGold[_levelNumber].ToString();
        }

        if (_levelText[_levelNumber - 1].Contains("희귀함"))
        {
            _levelUpLabel.color = _color[1];
        }
        else
        {
            _levelUpLabel.color = _color[2];
        }

        _levelUpLabel.text = _levelText[_levelNumber - 1];
        _levelUpRateText.text = _rateText[_levelNumber - 1];

        return true;
    }
}

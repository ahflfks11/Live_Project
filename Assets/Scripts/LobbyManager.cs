using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    LobbyCharacter _lobbyCharacter;
    LobbyMonster _lobbyMonster;
    GPGSManager _gpgsManager;
    LobbyUIManager _lobbyUIManager;
    DataManager _dataManager;

    public bool _tutorial;

    public GameObject[] _LootBox;

    public List<UnitData> _GachaList;

    // Start is called before the first frame update
    void Start()
    {
        _lobbyCharacter = FindObjectOfType<LobbyCharacter>();
        _lobbyMonster = FindObjectOfType<LobbyMonster>();
        _lobbyUIManager = FindObjectOfType<LobbyUIManager>();

        if (!_tutorial)
        {
            _lobbyCharacter.Attack();
        }
    }

    private void Update()
    {
        if (_gpgsManager == null && FindObjectOfType<GPGSManager>())
        {
            _gpgsManager = FindObjectOfType<GPGSManager>();
            _gpgsManager.SetValue(_lobbyUIManager._CoinText, _lobbyUIManager._CashText);
        }

        if (_dataManager == null && FindObjectOfType<DataManager>())
        {
            _dataManager = FindObjectOfType<DataManager>();
        }
    }

    public int SetupGachaList(int _GachaCount)
    {
        int _boxlevel = 0;
        _GachaList = new List<UnitData>();

        while (_GachaList.Count < _GachaCount)
        {
            double sum = 0f;

            for (int i = 0; i < _dataManager._data.Length; i++)
            {
                sum += _dataManager._data[i]._weight;
            }

            sum *= Random.value;

            int result_idx = 0;

            for (int i = 0; i < _dataManager._data.Length; i++)
            {
                sum -= _dataManager._data[i]._weight;
                if (sum <= 0)
                {
                    result_idx = i;

                    break;
                }
            }

            if (_GachaCount > 1)
                _lobbyUIManager.MultiGachaContent.SetupGachaImage(_dataManager._data[result_idx]._unit, _GachaList.Count, _dataManager._data[result_idx]._rarelity);
            else
                _lobbyUIManager.SingleGachaContent.SetupGachaImage(_dataManager._data[result_idx]._unit, _GachaList.Count, _dataManager._data[result_idx]._rarelity);

            _GachaList.Add(_dataManager._data[result_idx]._unit);

            if (_boxlevel < _dataManager._data[result_idx]._rarelity)
            {
                _boxlevel = _dataManager._data[result_idx]._rarelity;
            }
        }

        return _boxlevel;
        
    }

    public void SetCoin(string _coin)
    {
        _lobbyUIManager.SetCoinText(_coin);
    }

    public void SetCash(string _cash)
    {
        _lobbyUIManager.SetCashText(_cash);
    }

    public void CharacterAttack()
    {
        _lobbyCharacter.Attack();
    }

    public void MonsterHit()
    {
        _lobbyMonster.Hit();
    }

    public void ChangeData()
    {
        _gpgsManager.ChangeGoldCrystal(100, 100, _lobbyUIManager._CoinText, _lobbyUIManager._CashText);
    }

    public void GaveGold(int _gold)
    {
        _gpgsManager.GaveGold(_gold, _lobbyUIManager._CoinText);
    }

    public void GaveCrystal(int _crystal)
    {
        _gpgsManager.GaveCrystal(_crystal, _lobbyUIManager._CashText);
    }

    public void MultiGacha()
    {
        Destroy(GameObject.Find("LootBox"));
        int _boxlevel = SetupGachaList(8);
        GameObject _GachaboxObject = Instantiate(_LootBox[_boxlevel].gameObject, _LootBox[_boxlevel].transform.position, Quaternion.identity);
        _GachaboxObject.GetComponent<LootBoxController>()._isMuiti = true;
        _GachaboxObject.gameObject.name = "LootBox";

        if (_gpgsManager == null)
            return;

        if (_gpgsManager.LeastCrystal(1600, _lobbyUIManager._CashText))
        {

        }
    }

    public void SingleGacha()
    {
        Destroy(GameObject.Find("LootBox"));
        int _boxlevel = SetupGachaList(1);
        GameObject _GachaboxObject = Instantiate(_LootBox[_boxlevel].gameObject, _LootBox[_boxlevel].transform.position, Quaternion.identity);
        _GachaboxObject.GetComponent<LootBoxController>()._isMuiti = false;
        _GachaboxObject.gameObject.name = "LootBox";

        if (_gpgsManager == null)
            return;


        if (_gpgsManager.LeastCrystal(160, _lobbyUIManager._CashText))
        {

        }
    }

    public void EnterDungeon()
    {
        Transitioner.Instance.TransitionToScene(3);
    }
}

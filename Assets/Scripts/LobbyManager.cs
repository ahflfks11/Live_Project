using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager instance;

    LobbyCharacter _lobbyCharacter;
    LobbyMonster _lobbyMonster;
    GPGSManager _gpgsManager;
    public LobbyUIManager _lobbyUIManager;
    DataManager _dataManager;

    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private DG.Tweening.DOTweenVisualManager _dungeonArrowObject;
    [SerializeField] private GameObject _arrow;
    public bool _tutorial;

    private bool _rankState;

    public GameObject[] _LootBox;

    public List<UnitData> _GachaList;

    public static LobbyManager Instance { get => instance; set => instance = value; }
    public LobbyMonster LobbyMonster { get => _lobbyMonster; set => _lobbyMonster = value; }
    public GameObject SpawnEffect { get => spawnEffect; set => spawnEffect = value; }
    public DOTweenVisualManager DungeonArrowObject { get => _dungeonArrowObject; set => _dungeonArrowObject = value; }
    public bool RankState { get => _rankState; set => _rankState = value; }

    private void Awake()
    {
        Instance = this;
        //GPGSManager.Instance.GetNoticeList();
    }

    // Start is called before the first frame update
    void Start()
    {
        _lobbyCharacter = FindObjectOfType<LobbyCharacter>();
        LobbyMonster = FindObjectOfType<LobbyMonster>();
        _lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        Time.timeScale = 1f;

        if (!_tutorial)
        {
            if (FindObjectOfType<DialogueManager>())
                Destroy(FindObjectOfType<DialogueManager>().gameObject);

            if (JsonParseManager.Instance.Tutorial)
            {
                JsonParseManager.Instance.Tutorial = false;
            }
            _lobbyCharacter.Attack();
        }
        else
        {
            JsonParseManager.Instance.Tutorial = true;
            LobbyMonster.gameObject.SetActive(false);
        }

        AudioManager.instance.LobbyBgm(true);
    }

    private void Update()
    {
        if (_dataManager == null && FindObjectOfType<DataManager>())
        {
            _dataManager = FindObjectOfType<DataManager>();
        }

        if (_gpgsManager == null && FindObjectOfType<GPGSManager>())
        {
            _gpgsManager = FindObjectOfType<GPGSManager>();
            _gpgsManager.SetValue(_lobbyUIManager._CoinText, _lobbyUIManager._CashText);
            if (!_tutorial)
            {

            }
        }
    }

    //튜토리얼 arrow on / off
    public void SetArrow()
    {
        _arrow.SetActive(true);
    }

    public void LoadAd()
    {
        AdmobManager.Instance.ShowAd();
    }

    public int SetupGachaList(int _GachaCount)
    {
        int _boxlevel = 0;
        _GachaList = new List<UnitData>();

        int _duplicationCount = 0;

        while (_GachaList.Count < _GachaCount)
        {
            double sum = 0f;

            for (int i = 0; i < _dataManager._data.Length; i++)
            {
                sum += _dataManager._data[i]._unit._data.weight;
            }

            sum *= Random.value;

            int result_idx = 0;
            bool duplicationState = false;
            for (int i = 0; i < _dataManager._data.Length; i++)
            {
                sum -= _dataManager._data[i]._unit._data.weight;
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

            bool _result_Hero_Duplication = false;

            for (int i = 0; i < _dataManager.MyHeroList.Count; i++)
            {
                if (_dataManager.MyHeroList[i] == _dataManager._data[result_idx]._unit)
                {
                    if (_dataManager.MyHeroLevel[i] + 1 < 3)
                    {
                        _dataManager.MyHeroLevel[i] = _dataManager.MyHeroLevel[i] + 1;
                    }
                    else
                    {
                        _duplicationCount++;
                        duplicationState = true;
                    }
                    _result_Hero_Duplication = true;
                    break;
                }
            }

            if (_GachaCount > 1)
            {
                _lobbyUIManager.MultiGachaContent.Duplication(duplicationState, _GachaList.Count);
            }
            else
            {
                _lobbyUIManager.SingleGachaContent.Duplication(duplicationState, _GachaList.Count);
            }

            if (!_result_Hero_Duplication)
            {
                _dataManager.MyHeroList.Add(_dataManager._data[result_idx]._unit);
                _dataManager.MyHeroLevel.Add(0);
                _dataManager.NowLevel.Add(0);
                _lobbyUIManager.CreateIcon(_dataManager._data[result_idx], _dataManager.MyHeroList.Count - 1);
            }

            _GachaList.Add(_dataManager._data[result_idx]._unit);

            if (_boxlevel < _dataManager._data[result_idx]._rarelity)
            {
                _boxlevel = _dataManager._data[result_idx]._rarelity;
            }
        }

        string _tempHeroList = null;
        string _tempHeroLevel = null;
        string _tempNowLevel = null;

        for (int i = 0; i < _dataManager.MyHeroList.Count; i++)
        {
            for (int j = 0; j < _dataManager._data.Length; j++)
            {
                if (_dataManager.MyHeroList[i] == _dataManager._data[j]._unit)
                {
                    _tempHeroList += j;
                    break;
                }
            }

            _tempHeroLevel += _dataManager.MyHeroLevel[i];
            _tempNowLevel += _dataManager.NowLevel[i];

            if (i < _dataManager.MyHeroList.Count - 1)
            {
                _tempHeroList += ",";
                _tempHeroLevel += ",";
                _tempNowLevel += ",";
            }
        }

        if (_gpgsManager != null)
            _gpgsManager.WriteHeroInfo(_tempHeroList, _tempHeroLevel, _tempNowLevel);

        if (_duplicationCount > 0)
            GaveGold(_duplicationCount * 300);


        return _boxlevel;
        
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
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
        LobbyMonster.Hit();
    }

    public void ChangeData()
    {
        _gpgsManager.ChangeGoldCrystal(100, 100, _lobbyUIManager._CoinText, _lobbyUIManager._CashText);
    }

    public void GaveGold(int _gold)
    {
        _gpgsManager.Purchase_Game("Gold", _gold, BackEnd.Backend.UserNickName);
        _gpgsManager.GaveGold(_gold, _lobbyUIManager._CoinText);
    }

    public void GaveCrystal(int _crystal)
    {
        _gpgsManager.Purchase_Game("Crystal", _crystal, BackEnd.Backend.UserNickName);
        _gpgsManager.GaveCrystal(_crystal, _lobbyUIManager._CashText);
    }

    public void MultiGacha()
    {
        if (LobbyManager.instance._tutorial && JsonParseManager.Instance._txtNumber < 6)
            return;

        if (GameObject.Find("LootBox"))
            return;

        if (_gpgsManager.LeastCrystal(1280, _lobbyUIManager._CashText))
        {
            Destroy(GameObject.Find("LootBox"));
            int _boxlevel = SetupGachaList(8);
            GameObject _GachaboxObject = Instantiate(_LootBox[_boxlevel].gameObject, _LootBox[_boxlevel].transform.position, Quaternion.identity);
            _GachaboxObject.GetComponent<LootBoxController>()._isMuiti = true;
            _GachaboxObject.gameObject.name = "LootBox";
        }
    }

    public void SingleGacha()
    {
        if (LobbyManager.instance._tutorial && JsonParseManager.Instance._txtNumber < 6)
            return;

        if (GameObject.Find("LootBox"))
            return;


        if (_gpgsManager.LeastCrystal(160, _lobbyUIManager._CashText))
        {
            Destroy(GameObject.Find("LootBox"));
            int _boxlevel = SetupGachaList(1);
            GameObject _GachaboxObject = Instantiate(_LootBox[_boxlevel].gameObject, _LootBox[_boxlevel].transform.position, Quaternion.identity);
            _GachaboxObject.GetComponent<LootBoxController>()._isMuiti = false;
            _GachaboxObject.gameObject.name = "LootBox";
        }
    }

    public void EnterDungeon()
    {
        if(LobbyManager.instance._tutorial && JsonParseManager.Instance._txtNumber < 6)
            return;

        if (GameObject.Find("WiggleDiamondTransitioner"))
            Transitioner.Instance.TransitionToScene(3);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}

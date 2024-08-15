using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameData
{
    public Param UserInfo(string _nickName, int _level, int _cash, int _gold, int _stamina, int _stageClear, int _maxStage, string _values, int legendCount, int RareCount)
    {
        Param result = new Param();
        result.Add("NickName", _nickName);
        result.Add("Level", _level);
        result.Add("Cash", _cash);
        result.Add("Gold", _gold);
        result.Add("Stamina", _stamina);
        result.Add("StageClear", _stageClear);
        result.Add("High_Stage", _maxStage);
        result.Add("Values", _values);
        result.Add("LegendCount", legendCount);
        result.Add("RareCount", RareCount);
        return result;
    }

    public Param UserHeroInfo(string _heroList, string _herolevel,string _nowLevel)
    {
        Param result = new Param();
        result.Add("HeroList", _heroList);
        result.Add("HeroLevel", _herolevel);
        result.Add("NowLevel", _nowLevel);
        return result;
    }
}

// �ڳ��� �⺻ ���� ��Ʈ�� �̿��ϸ� ���� �������Դϴ�.  
// ���ε��Ͻ� ��Ʈ�� �÷��� �°� ������ �������ֽñ� �ٶ��ϴ�
public class ProbabilityItem
{
    public string itemid;
    public string itemname;
    public int num;
    public string itemtype;
    public string percent;

    public override string ToString()
    {
        return $"itemID : {itemid}\n" +
        $"itemName : {itemname}\n" +
        $"num : {num}\n" +
        $"itemtype : {itemtype}\n";
    }
}

public class GPGSManager : MonoBehaviour
{
    public GameData gameTable = new GameData();
    public Text _logText;
    bool _login;

    [SerializeField] string _fieldid = "12285";

    private static GPGSManager instance = null;

    public static GPGSManager Instance { get => instance; set => instance = value; }
    public bool Login { get => _login; set => _login = value; }

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
        }

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        BackendReturnObject _loginAcess = Backend.BMember.LoginWithTheBackendToken();
        if (_loginAcess.IsSuccess())
        {
            Debug.Log("�ڵ� �α��ο� �����߽��ϴ�");
        }
    }

    void Update()
    {
        Backend.AsyncPoll();

        if(_logText == null && GameObject.Find("LogText"))
        {
            _logText = GameObject.Find("LogText").GetComponent<Text>();
        }
    }

    void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            GetAccessCode();
            // Continue with Play Games Services
        }
        else
        {
            Login = true;
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    public void SetValue(TMPro.TMP_Text _coinText, TMPro.TMP_Text _cashText)
    {
        BackendReturnObject bro = null;

        // tableName���� �ִ� 10���� �ڱ� �ڽ��� ����� row �ҷ�����
        bro = Backend.PlayerData.GetMyData("UserInfo");

        // �ҷ����⿡ ������ ���
        if (bro.IsSuccess() == false)
        {
            Debug.Log("������ �б� �߿� ������ �߻��߽��ϴ� : " + bro.ToString());
            LobbyManager.Instance._lobbyUIManager.ErrorPanel();
            return;
        }

        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            _coinText.text = "0";
            _cashText.text = "0";
            return;
        }
        // 1�� �̻� �����͸� �ҷ��� ���
        if (bro.FlattenRows().Count > 0)
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            int level = int.Parse(bro.FlattenRows()[0]["Cash"].ToString());
            _coinText.text = bro.FlattenRows()[0]["Gold"].ToString();
            _cashText.text = bro.FlattenRows()[0]["Cash"].ToString();
        }
        // tableName���� �ִ� 1���� �ڱ� �ڽ��� ����� row �ҷ�����
        bro = Backend.PlayerData.GetMyData("UserInfo", 1);
    }

    public bool loginAcess()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();

        if (bro.IsSuccess())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GuestLogin()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin(Backend.BMember.GetGuestID());

        if (bro.IsSuccess())
        {
            if (CheckUser())
            {
                Transitioner.Instance.TransitionToScene(1);
            }
            else
            {
                Transitioner.Instance.TransitionToScene(2);
            }
        }
        else
        {
            Backend.BMember.DeleteGuestInfo();
            GuestLogin();
        }
    }

    public void GetAccessCode()
    {
        PlayGamesPlatform.Instance.RequestServerSideAccess(
          /* forceRefreshToken= */ false,
          code => {
              Backend.BMember.GetGPGS2AccessToken(code, googleCallback =>
              {
                  string accessToken = "";

                  if (googleCallback.IsSuccess())
                  {
                      accessToken = googleCallback.GetReturnValuetoJSON()["access_token"].ToString();
                  }

                  Backend.BMember.AuthorizeFederation(accessToken, FederationType.GPGS2, callback =>
                  {
                      Login = true;
                  });
              });
          });
    }

    public void ChangeUserInfo()
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");
        // �ҷ����⿡ ������ ���
        if (bro.IsSuccess() == false)
        {
            Debug.Log("������ �б� �߿� ������ �߻��߽��ϴ� : " + bro.ToString());
        }
        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�");
        }

        Debug.Log("������ : " + bro);
    }

    public bool CheckUser()
    {
        string nick = Backend.UserNickName;

        //���� �������� ������ false
        if(nick == string.Empty)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CreateNick(string _nick)
    {
        if (_nick != string.Empty)
        {
            Backend.BMember.CreateNickname(_nick);
            CreateUserData();
            SetValue(FindObjectOfType<LobbyUIManager>()._CoinText, FindObjectOfType<LobbyUIManager>()._CashText);
            return true;
        }
        else
        {
            return false;
        }
    }

    public string TakeNick()
    {
        return Backend.UserNickName;
    }

    public void DeleteUserData()
    {
        Backend.BMember.DeleteGuestInfo();
        Backend.BMember.WithdrawAccount();
    }

    public void TakeData()
    {
        // select[]�� �̿��Ͽ� ���� ��, owner_inDate�� score�� ��µǵ��� ����
        string[] select = { "owner_inDate", "level", "Cash" };
        TakeData("UserInfo", select);
    }

    public void TakeData(string _tableName, string[] _tableData)
    {

        // ���̺� �� �ش� rowIndate�� ���� row�� ��ȸ
        var bro = Backend.GameData.GetMyData(_tableName, "rowIndate");

        // ���̺� �� �ش� rowIndate�� ���� row�� ��ȸ
        // select�� �����ϴ� �÷��� ����
        bro = Backend.GameData.GetMyData(_tableName, "rowIndate", _tableData);
        Debug.Log(bro);
    }


    public void UpdateData(string _tableName, string _inDate, Param _data)
    {
        var bro = Backend.GameData.UpdateV2(_tableName, _inDate, Backend.UserInDate, _data);
    }

    public void CreateUserData()
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Backend.BMember.GetUserInfo((callback) =>
            {
                var bro = Backend.GameData.Insert("UserInfo", gameTable.UserInfo(Backend.UserNickName, 0, 0, 0, 100, 0, 0, "", 0, 0));
                CreateHeroInfo();
            });
        }
    }

    public void CreateHeroInfo()
    {
        var bro = Backend.PlayerData.GetMyData("UserHeroInfo");

        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Backend.BMember.GetUserInfo((callback) =>
            {
                var bro = Backend.GameData.Insert("UserHeroInfo", gameTable.UserHeroInfo("0,1,2,3", "0,0,0,0", "0,0,0,0"));

                if (bro.IsSuccess())
                {
                    ReadHeroInfo();
                }
            });
        }
    }

    //
    public void ChangeGoldCrystal(int _gold, int _Crystal, TMPro.TMP_Text _goldText, TMPro.TMP_Text _crystalText)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        bool _valueChange = false;

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();
            _updateParam.AddCalculation("Gold", GameInfoOperator.addition, _gold);
            _updateParam.AddCalculation("Cash", GameInfoOperator.addition, _Crystal);

            //������ ����
            var _result =  Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);

            if (_result.IsSuccess())
            {
                _valueChange = true;
            }
        }

        if (_valueChange)
        {
            var _data = Backend.PlayerData.GetMyData("UserInfo");

            if (_data.FlattenRows().Count > 0 && _data.IsSuccess())
            {
                _goldText.text = _data.FlattenRows()[0]["Gold"].ToString();
                _crystalText.text = _data.FlattenRows()[0]["Cash"].ToString();
            }
        }
    }

    public void WriteHeroInfo(string _heroList, string _heroLevel, string _nowLevel)
    {
        var bro = Backend.PlayerData.GetMyData("UserHeroInfo");

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            Backend.PlayerData.UpdateMyLatestData("UserHeroInfo", gameTable.UserHeroInfo(_heroList, _heroLevel, _nowLevel));
        }
    }

    public void SaveLevel(List<int> _nowlevelList)
    {
        string _nowLevel = null;
        string _HeroList = null;
        string _HeroLevel = null;

        for (int i = 0; i < _nowlevelList.Count; i++)
        {
            for (int j = 0; j < DataManager.Instance._data.Length; j++)
            {
                if (DataManager.Instance._data[j]._unit == DataManager.Instance.MyHeroList[i])
                {
                    _HeroList += j;
                    break;
                }
            }

            _nowLevel += _nowlevelList[i].ToString();
            _HeroLevel += DataManager.Instance.MyHeroLevel[i].ToString();

            if (i < _nowlevelList.Count - 1)
            {
                _nowLevel += ",";
                _HeroLevel += ",";
                _HeroList += ",";
            }
        }

        WriteHeroInfo(_HeroList, _HeroLevel, _nowLevel);
    }

    public void ClearStage(int _stage, int _dropCash, int _dropMoney, int _legendCount, int _rareCount)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        //�����Ͱ� ������ ���
        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();
            _updateParam.AddCalculation("Gold", GameInfoOperator.addition, _dropMoney);
            _updateParam.AddCalculation("Cash", GameInfoOperator.addition, _dropCash);
            _updateParam.AddCalculation("StageClear", GameInfoOperator.addition, 1);

            //������ ����
            var _result = Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);

            Param _updateStage = new Param();
            _updateStage.Add("High_Stage", _stage);

            if (_stage > int.Parse(bro.FlattenRows()[0]["High_Stage"].ToString()))
            {
                Backend.GameData.UpdateV2("UserInfo", inDate, Backend.UserInDate, _updateStage);
            }

            Param _legendParam = new Param();
            _legendParam.Add("LegendCount", _legendCount);

            if (_legendCount > int.Parse(bro.FlattenRows()[0]["LegendCount"].ToString()))
            {
                Backend.GameData.UpdateV2("UserInfo", inDate, Backend.UserInDate, _legendParam);
            }

            Param _rareParam = new Param();
            _rareParam.Add("RareCount", _rareCount);

            if (_legendCount > int.Parse(bro.FlattenRows()[0]["RareCount"].ToString()))
            {
                Backend.GameData.UpdateV2("UserInfo", inDate, Backend.UserInDate, _rareParam);
            }
        }
    }

    public void ReadHeroInfo()
    {
        var bro = Backend.PlayerData.GetMyData("UserHeroInfo");

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            LobbyUIManager _lobbyManager = GameObject.Find("LobbyUIManager").GetComponent<LobbyUIManager>();
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            string _heroInfo = bro.FlattenRows()[0]["HeroList"].ToString();
            string _heroInfo_Level = bro.FlattenRows()[0]["HeroLevel"].ToString();
            string _heroInfo_NowLevel = bro.FlattenRows()[0]["NowLevel"].ToString();
            string[] _myHeroList = _heroInfo.Split(',');
            string[] _myHeroLevel = _heroInfo_Level.Split(',');
            string[] _myNowLevel = _heroInfo_NowLevel.Split(',');
            for (int i = 0; i < _myHeroList.Length; i++)
            {
                DataManager.Instance.MyHeroList.Add(DataManager.Instance._data[int.Parse(_myHeroList[i].ToString())]._unit);
                DataManager.Instance.MyHeroLevel.Add(int.Parse(_myHeroLevel[i]));
                DataManager.Instance.NowLevel.Add(int.Parse(_myNowLevel[i]));
                _lobbyManager.CreateIcon(DataManager.Instance._data[int.Parse(_myHeroList[i].ToString())], i);
            }
        }
    }

    public void LeastGold(int _gold, TMPro.TMP_Text _goldText)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        bool _valueChange = false;

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();

            _updateParam.AddCalculation("Gold", GameInfoOperator.subtraction, _gold);

            //������ ����
            var _result = Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);

            if (_result.IsSuccess())
            {
                _valueChange = true;
            }
        }

        if (_valueChange)
        {
            var _data = Backend.PlayerData.GetMyData("UserInfo");

            if (_data.FlattenRows().Count > 0 && _data.IsSuccess())
            {
                _goldText.text = _data.FlattenRows()[0]["Gold"].ToString();
            }
        }
    }

    public bool LeastCrystal(int _crystal, TMPro.TMP_Text _crystalText)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        bool _valueChange = false;

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            if (int.Parse(bro.FlattenRows()[0]["Cash"].ToString()) < _crystal)
                return false;

            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();
            _updateParam.AddCalculation("Cash", GameInfoOperator.subtraction, _crystal);

            //������ ����
            var _result = Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);

            if (_result.IsSuccess())
            {
                _valueChange = true;
            }
        }

        if (_valueChange)
        {
            var _data = Backend.PlayerData.GetMyData("UserInfo");

            if (_data.FlattenRows().Count > 0 && _data.IsSuccess())
            {
                _crystalText.text = _data.FlattenRows()[0]["Cash"].ToString();
            }
        }

        return _valueChange;
    }

    public void GaveGold(int _gold, TMPro.TMP_Text _goldText)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        bool _valueChange = false;

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();
            _updateParam.AddCalculation("Gold", GameInfoOperator.addition, _gold);

            //������ ����
            var _result = Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);

            if (_result.IsSuccess())
            {
                _valueChange = true;
            }
        }

        if (_valueChange)
        {
            var _data = Backend.PlayerData.GetMyData("UserInfo");

            if (_data.FlattenRows().Count > 0 && _data.IsSuccess())
            {
                _goldText.text = _data.FlattenRows()[0]["Gold"].ToString();
            }
        }
    }

    public void GaveCrystal(int _Crystal, TMPro.TMP_Text _crystalText)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        bool _valueChange = false;

        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();
            _updateParam.AddCalculation("Cash", GameInfoOperator.addition, _Crystal);

            //������ ����
            var _result = Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);

            if (_result.IsSuccess())
            {
                _valueChange = true;
            }
        }

        if (_valueChange)
        {
            var _data = Backend.PlayerData.GetMyData("UserInfo");

            if (_data.FlattenRows().Count > 0 && _data.IsSuccess())
            {
                _crystalText.text = _data.FlattenRows()[0]["Cash"].ToString();
            }
        }
    }

    public void DrawList()
    {
        string probabilityFileId = _fieldid;

        var bro = Backend.Probability.GetProbabilityContents(probabilityFileId);

        if (!bro.IsSuccess())
        {
            Debug.LogError(bro.ToString());
            return;
        }

        LitJson.JsonData json = bro.FlattenRows();

        List<ProbabilityItem> itemList = new List<ProbabilityItem>();

        for (int i = 0; i < json.Count; i++)
        {
            ProbabilityItem item = new ProbabilityItem();

            item.itemid = json[i]["itemid"].ToString();
            item.itemname = json[i]["itemname"].ToString();
            item.num = int.Parse(json[i]["num"].ToString());
            item.itemtype = json[i]["itemtype"].ToString();

            itemList.Add(item);
        }

        foreach (var item in itemList)
        {
            Debug.Log(item.ToString());
        }

        Debug.Log("Ȯ�� �������� �� ���� : " + itemList.Count);
    }

    public void GetProbability()
    {
        string selectedProbabilityFileId = _fieldid;

        var bro = Backend.Probability.GetProbabilitys(selectedProbabilityFileId, 8); // 8����;

        if (!bro.IsSuccess())
        {
            Debug.LogError(bro.ToString());
            return;
        }

        LitJson.JsonData json = bro.GetFlattenJSON()["elements"];

        List<ProbabilityItem> itemList = new List<ProbabilityItem>();

        for (int i = 0; i < json.Count; i++)
        {
            ProbabilityItem item = new ProbabilityItem();

            item.itemid = json[i]["itemid"].ToString();
            item.itemname = json[i]["itemname"].ToString();
            item.num = int.Parse(json[i]["num"].ToString());
            item.itemtype = json[i]["itemtype"].ToString();

            itemList.Add(item);
        }

        foreach (var item in itemList)
        {
            Debug.Log(item.ToString());
        }
    }

    public void LogOut()
    {
        Backend.BMember.Logout((callback) => {
            _logText.text = "�α׾ƿ� �Ǿ����ϴ�.";
            // ���� ó��
        });
    }
}

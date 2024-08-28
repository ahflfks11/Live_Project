using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
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

public class Notice
{
    public string title;
    public string contents;
    public DateTime postingDate;
    public string imageKey;
    public string inDate;
    public string uuid;
    public string linkUrl;
    public bool isPublic;
    public string linkButtonName;
    public string author;

    public override string ToString()
    {
        return $"title : {title}\n" +
        $"contents : {contents}\n" +
        $"postingDate : {postingDate}\n" +
        $"imageKey : {imageKey}\n" +
        $"inDate : {inDate}\n" +
        $"uuid : {uuid}\n" +
        $"linkUrl : {linkUrl}\n" +
        $"isPublic : {isPublic}\n" +
        $"linkButtonName : {linkButtonName}\n" +
        $"author : {author}\n";
    }
}

// 뒤끝의 기본 제공 차트를 이용하면 만든 아이템입니다.  
// 업로드하신 차트의 컬럼명에 맞게 변수를 변경해주시기 바랍니다
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

public class RankItem
{
    public string gamerInDate;
    public string nickname;
    public string score;
    public string index;
    public string rank;
    public string extraData = string.Empty;
    public string extraName = "High_Stage";
    public string totalCount;

    public override string ToString()
    {
        string str = $"유저인데이트:{gamerInDate}\n닉네임:{nickname}\n점수:{score}\n정렬:{index}\n순위:{rank}\n총합:{totalCount}\n";
        if (extraName != string.Empty)
        {
            str += $"{extraName}:{extraData}\n";
        }
        return str;
    }
}

// 뒤끝의 기본 제공 차트를 이용하면 만든 아이템입니다.  
// 업로드하신 차트의 컬럼명에 맞게 변수를 변경해주시기 바랍니다.  
public class CouponItem
{
    public string uuid;
    public string chartFileName;
    public string itemID;
    public string itemName;
    public int num;
    public int itemtype;
    public int itemValues;
    public int itemCount;
    public string itemComment;
    public override string ToString()
    {
        return $"itemID : {itemID}\n" +
        $"itemName : {itemName}\n" +
        $"num : {num}\n" +
        $"itemtype : {itemtype}\n"+
        $"itemValues : {itemValues}\n"+
        $"itemCount : {itemCount}\n" +
        $"itemComment : {itemComment}\n" +
        $"chartFileName : {chartFileName}\n" +
        $"uuid : {uuid}\n";
    }
}

public class GPGSManager : MonoBehaviour
{
    public GameData gameTable = new GameData();
    public Text _logText;

    bool _loginState = false;
    //bool _login;

    private int level;

    [SerializeField] string _fieldid = "12285";

    List<RankItem> rankItemList = new List<RankItem>();

    private static GPGSManager instance = null;

    public static GPGSManager Instance { get => instance; set => instance = value; }
    public int Level { get => level; set => level = value; }

    //public bool Login { get => _login; set => _login = value; }

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
        var bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
            UpdateCheck();
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }

        if (_logText == null && GameObject.Find("LogText"))
        {
            //_logText = GameObject.Find("LogText").GetComponent<Text>();
        }

        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail() // 이메일 권한을 얻고 싶지 않다면 해당 줄(RequestEmail)을 지워주세요.
            .RequestIdToken()
            .Build();
        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 바꿔주세요.
                                                  //GPGS 시작.
        PlayGamesPlatform.Activate();

        BackendReturnObject _loginAccess = Backend.BMember.LoginWithTheBackendToken();
        if (_loginAccess.IsSuccess())
        {
            //_logText.text = "자동 로그인 성공 했습니다.";
        }
    }

    public void GpgsInit()
    {
        _loginState = false;
    }

    internal void GPGSLogin()
    {
        if (_loginState)
            return;

        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            if (BRO.IsSuccess())
            {
                _loginState = true;
                if (CheckUser())
                {
                    Transitioner.Instance.TransitionToScene(1);
                }
                else
                {
                    Transitioner.Instance.TransitionToScene(2);
                }
            }
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    string _id = Backend.BMember.GetGuestID();

                    if (_id == string.Empty)
                    {
                        // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                        if (BRO.IsSuccess())
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
                    }
                    else
                    {
                        // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                        BackendReturnObject BRO = Backend.BMember.ChangeCustomToFederation(GetTokens(), FederationType.Google);
                        if (BRO.IsSuccess())
                        {
                            _loginState = true;
                            if (CheckUser())
                            {
                                Transitioner.Instance.TransitionToScene(1);
                            }
                            else
                            {
                                Transitioner.Instance.TransitionToScene(2);
                            }
                        }
                    }
                }
                else
                {
                    // 로그인 실패
                    //_logText.text = "Login failed for some reason";
                }
            });
        }
    }

    // 구글 토큰 받아옴
    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫 번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두 번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    public void GuestLogin()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin("게스트 로그인으로 로그인함");
        if (bro.IsSuccess())
        {
            Debug.Log("게스트 로그인에 성공했습니다");
            if (CheckUser())
            {
                Transitioner.Instance.TransitionToScene(1);
            }
            else
            {
                Transitioner.Instance.TransitionToScene(2);
            }
        }
    }

    public void RemoveGuest()
    {
        Backend.BMember.DeleteGuestInfo();
    }

    void Update()
    {
        if(_logText == null && GameObject.Find("LogText"))
        {
            //_logText = GameObject.Find("LogText").GetComponent<Text>();
        }

        Backend.Notification.OnNewNoticeCreated = (string title, string content) => {
            Debug.Log(
                $"[OnNewNoticeCreated(새로운 공지사항 생성)]\n" +
                $"| title : {title}\n" +
                $"| content : {content}\n"
            );
        };
    }

    //쿠폰
    public bool UseCoupons(string _code)
    {
        var bro = Backend.Coupon.UseCoupon(_code);

        if (!bro.IsSuccess())
        {
            Debug.LogError(bro.ToString());
            return false;
        }

        List<CouponItem> couponItemList = new List<CouponItem>();

        LitJson.JsonData json = bro.GetReturnValuetoJSON();

        for (int i = 0; i < json["itemObject"].Count; i++)
        {
            CouponItem couponItem = new CouponItem();

            couponItem.uuid = json["uuid"].ToString();
            couponItem.itemCount = int.Parse(json["itemObject"][i]["itemCount"].ToString());
            couponItem.itemID = json["itemObject"][i]["item"]["itemid"].ToString();
            couponItem.itemName = json["itemObject"][i]["item"]["itemname"].ToString();
            couponItem.num = int.Parse(json["itemObject"][i]["item"]["num"].ToString());
            couponItem.itemtype = int.Parse(json["itemObject"][i]["item"]["itemtype"].ToString());
            couponItem.itemValues = int.Parse(json["itemObject"][i]["item"]["itemValues"].ToString());
            couponItem.itemComment = json["itemObject"][i]["item"]["itemComment"].ToString();
            couponItem.chartFileName = json["itemObject"][i]["item"]["chartFileName"].ToString();

            switch (couponItem.itemtype)
            {
                case 0:
                    GaveCrystal(couponItem.itemValues, LobbyManager.Instance._lobbyUIManager._CashText);
                    break;
            }

            couponItemList.Add(couponItem);
        }

        foreach (var couponItem in couponItemList)
        {
            Debug.Log(couponItem.ToString());
        }

        return true;
    }

    public void GetNoticeList()
    {
        List<Notice> noticeList = new List<Notice>();

        BackendReturnObject bro = Backend.Notice.NoticeList(10);
        if (bro.IsSuccess())
        {
            Debug.Log("리턴값 : " + bro);

            LitJson.JsonData jsonList = bro.FlattenRows();
            for (int i = 0; i < jsonList.Count; i++)
            {
                Notice notice = new Notice();

                notice.title = jsonList[i]["title"].ToString();
                notice.contents = jsonList[i]["content"].ToString();
                notice.postingDate = DateTime.Parse(jsonList[i]["postingDate"].ToString());
                notice.inDate = jsonList[i]["inDate"].ToString();
                notice.uuid = jsonList[i]["uuid"].ToString();
                notice.isPublic = jsonList[i]["isPublic"].ToString() == "y" ? true : false;
                notice.author = jsonList[i]["author"].ToString();

                if (jsonList[i].ContainsKey("imageKey"))
                {
                    notice.imageKey = "http://upload-console.thebackend.io" + jsonList[i]["imageKey"].ToString();
                }
                if (jsonList[i].ContainsKey("linkUrl"))
                {
                    notice.linkUrl = jsonList[i]["linkUrl"].ToString();
                }
                if (jsonList[i].ContainsKey("linkButtonName"))
                {
                    notice.linkButtonName = jsonList[i]["linkButtonName"].ToString();
                }

                noticeList.Add(notice);
                Debug.Log(notice.ToString());
            }
        }
    }

    public void RealtimeServer()
    {
        // 접속 시 반응하는 핸들러 설정
        Backend.Notification.OnAuthorize = (bool result, string reason) => {
            Debug.Log("실시간 알림 서버 접속 시도!");

            //접속 이후 처리
            if (result)
            {
                Debug.Log("실시간 알림 서버 접속 성공!");
            }
            else
            {
                Debug.Log("실시간 알림 서버 접속 실패 : 이유 : " + reason);
            }
        };

        // 실시간 알림 서버로 연결
        Backend.Notification.Connect();
    }


    public void SetValue(TMPro.TMP_Text _coinText, TMPro.TMP_Text _cashText)
    {
        BackendReturnObject bro = null;

        // tableName에서 최대 10개의 자기 자신이 등록한 row 불러오기
        bro = Backend.PlayerData.GetMyData("UserInfo");

        // 불러오기에 실패할 경우
        if (bro.IsSuccess() == false)
        {
            Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + bro.ToString());
            LobbyManager.Instance._lobbyUIManager.ErrorPanel();
            return;
        }

        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            _coinText.text = "0";
            _cashText.text = "0";
            return;
        }
        // 1개 이상 데이터를 불러온 경우
        if (bro.FlattenRows().Count > 0)
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            level = int.Parse(bro.FlattenRows()[0]["Level"].ToString());
            _coinText.text = bro.FlattenRows()[0]["Gold"].ToString();
            _cashText.text = bro.FlattenRows()[0]["Cash"].ToString();
        }
        // tableName에서 최대 1개의 자기 자신이 등록한 row 불러오기
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

    public void ChangeUserInfo()
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");
        // 불러오기에 실패할 경우
        if (bro.IsSuccess() == false)
        {
            Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + bro.ToString());
        }
        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Debug.Log("데이터가 존재하지 않습니다");
        }

        Debug.Log("데이터 : " + bro);
    }

    public bool CheckUser()
    {
        string nick = Backend.UserNickName;

        //닉이 존재하지 않으면 false
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
        // select[]를 이용하여 리턴 시, owner_inDate와 score만 출력되도록 설정
        string[] select = { "owner_inDate", "level", "Cash" };
        TakeData("UserInfo", select);
    }

    public void TakeData(string _tableName, string[] _tableData)
    {

        // 테이블 내 해당 rowIndate를 지닌 row를 조회
        var bro = Backend.GameData.GetMyData(_tableName, "rowIndate");

        // 테이블 내 해당 rowIndate를 지닌 row를 조회
        // select에 존재하는 컬럼만 리턴
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

        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
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

        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
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

            //데이터 수정
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


    //클리어시 서버 전송 패킷
    public void ClearStage(int _stage, int _dropCash, int _dropMoney, int _legendCount, int _rareCount)
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");

        //데이터가 존재할 경우
        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();

            Param _updateParam = new Param();
            _updateParam.AddCalculation("Gold", GameInfoOperator.addition, _dropMoney);
            _updateParam.AddCalculation("Cash", GameInfoOperator.addition, _dropCash);
            _updateParam.AddCalculation("StageClear", GameInfoOperator.addition, 1);

            //데이터 수정
            var _result = Backend.GameData.UpdateWithCalculationV2("UserInfo", inDate, Backend.UserInDate, _updateParam);
           
            //랭킹 업데이트
            Param updateRankParam = new Param();
            bool _updateRank = false;

            Param _updateStage = new Param();
            _updateStage.Add("High_Stage", _stage);

            if (_stage > int.Parse(bro.FlattenRows()[0]["High_Stage"].ToString()))
            {
                updateRankParam.Add("High_Stage", _stage);
                Backend.GameData.UpdateV2("UserInfo", inDate, Backend.UserInDate, _updateStage);
                _updateRank = true;
            }

            Param _legendParam = new Param();
            _legendParam.Add("LegendCount", _legendCount);

            if (_legendCount > int.Parse(bro.FlattenRows()[0]["LegendCount"].ToString()))
            {
                updateRankParam.Add("LegendCount", _legendCount);
                Backend.GameData.UpdateV2("UserInfo", inDate, Backend.UserInDate, _legendParam);
                _updateRank = true;
            }

            Param _rareParam = new Param();
            _rareParam.Add("RareCount", _rareCount);

            if (_legendCount > int.Parse(bro.FlattenRows()[0]["RareCount"].ToString()))
            {
                Backend.GameData.UpdateV2("UserInfo", inDate, Backend.UserInDate, _rareParam);
            }

            if (_updateRank)
                GPGSManager.instance.UpdateRanking(updateRankParam);
        }
    }

    private void UpdateCheck()
    {
        // 유니티 플레이어 세팅에 설정한 버전 정보
        Version client = new Version(Application.version);
        Debug.Log("clientVersion: " + client);

#if UNITY_EDITOR
        // 뒤끝 버전 정보 조회는 iOS / Android 환경에서만 호출 할 수 있습니다.
        Debug.Log("에디터 모드에서는 버전 정보를 조회할 수 없습니다.");
        return;
#endif

        // 뒤끝 콘솔에서 설정한 버전 정보를 조회
        Backend.Utils.GetLatestVersion(callback => {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("버전 정보를 조회하는데 실패하였습니다.\n" + callback);
                return;
            }

            var version = callback.GetReturnValuetoJSON()["version"].ToString();
            Version server = new Version(version);

            var result = server.CompareTo(client);
            if (result == 0)
            {
                // 0 이면 두 버전이 일치하는 것 입니다.
                // 아무 작업 안하고 리턴
                return;
            }
            else if (result < 0)
            {
                // 0 미만인 경우 server 버전이 client 보다 작은 경우 입니다.
                // 애플/구글 스토어에 검수를 넣었을 경우 여기에 해당 할 수 있습니다.
                // ex)
                // 검수를 신청한 클라이언트 버전은 3.0.0, 
                // 라이브에 운용중인 클라이언트 버전은 2.0.0,
                // 뒤끝 콘솔에 등록한 버전은 2.0.0 

                // 아무 작업을 안하고 리턴
                return;
            }
            // 0보다 크면 server 버전이 클라이언트 이후 버전일 수 있습니다.
            else if (client == null)
            {
                // 단 클라이언트 버전 정보가 null인 경우에도 0보다 큰 값이 리턴될 수 있습니다.
                // 이 때는 아무 작업을 안하고 리턴하도록 하겠습니다.
                Debug.LogError("클라이언트 버전 정보가 null 입니다.");
                return;
            }

            // 여기까지 리턴 없이 왔으면 server 버전(뒤끝 콘솔에 등록한 버전)이 
            // 클라이언트보다 높은 경우 입니다.

            // 유저가 스토어에서 업데이트를 하도록 업데이트 UI를 띄워줍니다.
           OpenUpdateUI();
        });
    }

    const string PlayStoreLink = "market://details?id=com.nyangdev.rndgame";
    const string AppStoreLink = "itms-apps://itunes.apple.com/app/앱ID";

    public void OpenUpdateUI()
    {
        TitleManager.Instance._versionUI.SetActive(true);
    }

    // 아래는 OpenUpdateUI 함수를 이용하여 업데이트 UI가 활성화 되었고,
    // 업데이트 UI 객체 내에 확인 버튼이 있으며
    // 해당 버튼 클릭 시 아래 함수를 호출 할 경우
    // 각 OS 환경에 따라 각각의 스토어 URL로 이동하도록 하는 함수입니다.
    public void OpenStoreLink()
    {
#if UNITY_ANDROID
        Application.OpenURL(PlayStoreLink);
#elif UNITY_IOS
      Application.OpenURL(AppStoreLink);
#else
      Debug.LogError("지원하지 않는 플랫폼 입니다.");
#endif
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

            //데이터 수정
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

            //데이터 수정
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

            //데이터 수정
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

            //데이터 수정
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

        Debug.Log("확률 아이템의 총 갯수 : " + itemList.Count);
    }

    public void GetProbability()
    {
        string selectedProbabilityFileId = _fieldid;

        var bro = Backend.Probability.GetProbabilitys(selectedProbabilityFileId, 8); // 8연차;

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

    public void GetMyRankTest()
    {
        string userUuid = "b6b7d9f0-5b9d-11ef-a529-8f74572da8f7";

        BackendReturnObject bro = Backend.URank.User.GetMyRank(userUuid);

        if (bro.IsSuccess())
        {
            Destroy(GameObject.Find("No_Rank_Text"));
            LitJson.JsonData rankListJson = bro.GetFlattenJSON();
            string extraName = string.Empty;
            RankItem rankItem = new RankItem();

            rankItem.gamerInDate = rankListJson["rows"][0]["gamerInDate"].ToString();
            rankItem.nickname = rankListJson["rows"][0]["nickname"].ToString();
            rankItem.score = rankListJson["rows"][0]["score"].ToString();
            rankItem.index = rankListJson["rows"][0]["index"].ToString();
            rankItem.rank = rankListJson["rows"][0]["rank"].ToString();
            rankItem.totalCount = rankListJson["totalCount"].ToString();

            if (rankListJson["rows"][0].ContainsKey(rankItem.extraName))
            {
                rankItem.extraData = rankListJson["rows"][0][rankItem.extraName].ToString();
            }

            Debug.Log(rankItem.ToString());

            RankingUI _rank = Instantiate(LobbyManager.Instance._lobbyUIManager.RankingUI, Vector3.zero, Quaternion.identity);
            _rank.SetRankUI(int.Parse(rankItem.rank), rankItem.nickname, int.Parse(rankItem.extraData), int.Parse(rankItem.score));
            _rank.transform.SetParent(GameObject.Find("MyRank").transform);
            _rank.transform.localScale = new Vector3(1f, 1f, 1f);
            _rank.GetComponent<RectTransform>().rect.Set(0, 0, 100, 100);
        }
    }

    public void Purchase_Game(string _type, int _cash, string _userName)
    {
        if (GPGSManager.Instance.Level < 0)
            return;

        Param _param = new Param();
        _param.Add("Type", _type);
        _param.Add("Cash", _cash);
        _param.Add("UserName", _userName);

        Backend.GameData.Insert("Purchase_history", _param);
    }

    public void RankList()
    {
        string userUuid = "b6b7d9f0-5b9d-11ef-a529-8f74572da8f7";
        int limit = 100;

        rankItemList = new List<RankItem>();

        BackendReturnObject bro = Backend.URank.User.GetRankList(userUuid, limit);

        if (bro.IsSuccess())
        {
            LitJson.JsonData rankListJson = bro.GetFlattenJSON();

            string extraName = string.Empty;

            GetMyRankTest();

            for (int i = 0; i < rankListJson["rows"].Count; i++)
            {
                RankItem rankItem = new RankItem();

                rankItem.gamerInDate = rankListJson["rows"][i]["gamerInDate"].ToString();
                try
                {
                    rankItem.nickname = rankListJson["rows"][i]["nickname"].ToString();                    
                }
                catch
                {
                    rankItem.nickname = "유저" + i;
                }
                rankItem.score = rankListJson["rows"][i]["score"].ToString();
                rankItem.index = rankListJson["rows"][i]["index"].ToString();
                rankItem.rank = rankListJson["rows"][i]["rank"].ToString();
                rankItem.totalCount = rankListJson["totalCount"].ToString();
                if (rankListJson["rows"][i].ContainsKey(rankItem.extraName))
                {
                    rankItem.extraData = rankListJson["rows"][i][rankItem.extraName].ToString();
                }

                rankItemList.Add(rankItem);

                RankingUI _rank = Instantiate(LobbyManager.Instance._lobbyUIManager.RankingUI, Vector3.zero, Quaternion.identity);
                _rank.SetRankUI(int.Parse(rankItem.rank), rankItem.nickname, int.Parse(rankItem.extraData), int.Parse(rankItem.score));
                _rank.transform.SetParent(LobbyManager.Instance._lobbyUIManager._rankTransform);
                _rank.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    public void UpdateRanking(Param _data)
    {
        string tableName = "UserInfo";
        string rowIndate = string.Empty;
        string rankingUUID = "b6b7d9f0-5b9d-11ef-a529-8f74572da8f7";

        //Param param = new Param();
        //param.Add("High_Stage", _highStage);
        //param.Add("LegendCount", _legendCount);

        var bro = Backend.GameData.Get("UserInfo", new Where());

        if (bro.IsSuccess())
        {
            if (bro.FlattenRows().Count > 0)
            {
                rowIndate = bro.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                var bro2 = Backend.GameData.Insert(tableName, _data);

                if (bro2.IsSuccess())
                {
                    rowIndate = bro2.GetInDate();
                }
                else
                {
                    return;
                }

            }
        }
        else
        {
            return;
        }

        if (rowIndate == string.Empty)
        {
            return;
        }

        var rankBro = Backend.URank.User.UpdateUserScore(rankingUUID, tableName, rowIndate, _data);
        if (rankBro.IsSuccess())
        {
            Debug.Log("랭킹 등록 성공");
        }
        else
        {
            Debug.Log("랭킹 등록 실패 : " + rankBro);
        }
    }

    public void LogOut()
    {
        Backend.BMember.Logout((callback) => {
            if (Social.localUser.authenticated == true)
            {
                ((PlayGamesPlatform)Social.Active).SignOut();
            }
        });
    }
}

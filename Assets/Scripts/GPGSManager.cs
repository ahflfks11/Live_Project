using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;
public class GameData
{
    public Param UserInfo(string _nickName, int _level, int _cash, int _gold)
    {
        Param result = new Param();
        result.Add("NickName", _nickName);
        result.Add("Level", _level);
        result.Add("Cash", _cash);
        result.Add("Gold", _gold);
        return result;
    }
}

public class GPGSManager : MonoBehaviour
{
    public GameData gameTable = new GameData();
    public Text _logText;
    bool _login;

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
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    void Update()
    {
        Backend.AsyncPoll();
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

        // tableName에서 최대 10개의 자기 자신이 등록한 row 불러오기
        bro = Backend.PlayerData.GetMyData("UserInfo");
        // 불러오기에 실패할 경우
        if (bro.IsSuccess() == false)
        {
            Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + bro.ToString());
        }
        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Debug.Log("데이터가 존재하지 않습니다");
            _coinText.text = "0";
            _cashText.text = "0";
        }
        // 1개 이상 데이터를 불러온 경우
        if (bro.FlattenRows().Count > 0)
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            int level = int.Parse(bro.FlattenRows()[0]["Cash"].ToString());
            _coinText.text = bro.FlattenRows()[0]["Gold"].ToString();
            _cashText.text = bro.FlattenRows()[0]["Cash"].ToString();
        }
        // tableName에서 최대 1개의 자기 자신이 등록한 row 불러오기
        bro = Backend.PlayerData.GetMyData("UserInfo", 1);
    }

    public void GuestLogin()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin("게스트 로그인으로 로그인함");
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
            string id = Backend.BMember.GetGuestID();
            Debug.Log("로컬 기기에 저장된 아이디 :" + id);
            Debug.Log(bro);
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
        Debug.Log(nick);

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
        if (bro.IsSuccess())
        {
            _logText.text = "데이터 수정 성공";
        }
        else
        {
            _logText.text = "데이터 수정 실패";
        }
    }

    public void CreateUserData()
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");
        // 불러오기에 실패할 경우
        if (bro.IsSuccess() == false)
        {
            _logText.text = "데이터 읽기 중에 문제가 발생했습니다 : " + bro.ToString();
        }
        // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Backend.BMember.GetUserInfo((callback) =>
            {
                var bro = Backend.GameData.Insert("UserInfo", gameTable.UserInfo(Backend.UserNickName, 0, 0, 0));
            });
        }
    }

    public bool UpdateData()
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");
        if (bro.FlattenRows().Count > 0 && bro.IsSuccess())
        {
            string inDate = bro.FlattenRows()[0]["inDate"].ToString();
            UpdateData("UserInfo", inDate, gameTable.UserInfo(Backend.UserNickName, 1, 100, 100));
            return true;
        }

        return false;
    }

    public void LogOut()
    {
        Backend.BMember.Logout((callback) => {
            _logText.text = "로그아웃 되었습니다.";
            // 이후 처리
        });
    }
}

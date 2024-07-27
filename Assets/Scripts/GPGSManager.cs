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

        // tableName���� �ִ� 10���� �ڱ� �ڽ��� ����� row �ҷ�����
        bro = Backend.PlayerData.GetMyData("UserInfo");
        // �ҷ����⿡ ������ ���
        if (bro.IsSuccess() == false)
        {
            Debug.Log("������ �б� �߿� ������ �߻��߽��ϴ� : " + bro.ToString());
        }
        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
        if (bro.IsSuccess() && bro.FlattenRows().Count <= 0)
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�");
            _coinText.text = "0";
            _cashText.text = "0";
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

    public void GuestLogin()
    {
        BackendReturnObject bro = Backend.BMember.GuestLogin("�Խ�Ʈ �α������� �α�����");
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
            Debug.Log("���� ��⿡ ����� ���̵� :" + id);
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
        Debug.Log(nick);

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
        if (bro.IsSuccess())
        {
            _logText.text = "������ ���� ����";
        }
        else
        {
            _logText.text = "������ ���� ����";
        }
    }

    public void CreateUserData()
    {
        var bro = Backend.PlayerData.GetMyData("UserInfo");
        // �ҷ����⿡ ������ ���
        if (bro.IsSuccess() == false)
        {
            _logText.text = "������ �б� �߿� ������ �߻��߽��ϴ� : " + bro.ToString();
        }
        // �ҷ����⿡�� ���������� �����Ͱ� �������� �ʴ� ���
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
            _logText.text = "�α׾ƿ� �Ǿ����ϴ�.";
            // ���� ó��
        });
    }
}

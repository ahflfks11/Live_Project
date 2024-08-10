using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class JsonParseManager : MonoBehaviour
{
    private static JsonParseManager instance;

    string jsonFileName = "data.json";

    public int _txtNumber = 0;

    private int _yesNumber = -1;
    private int _noNumber = -1;

    private bool _tutorial = false;

    private string _yes_Setence;
    private string _no_Setence;

    public List<int> _actionTypeList = new List<int>();
    public List<int> _actionType = new List<int>();

    [System.Serializable]
    public class DataEntry
    {
        public int no;
        public string Name;
        public int Types;
        public string setence;
        public int? Yes;
        public int? No;
        public string EndTalk;
        public string Yes_setence;
        public string No_setence;
    }

    private List<DataEntry> dataList = new List<DataEntry>();

    public static JsonParseManager Instance { get => instance; set => instance = value; }
    public int YesNumber { get => _yesNumber; set => _yesNumber = value; }
    public int NoNumber { get => _noNumber; set => _noNumber = value; }
    public string Yes_Setence { get => _yes_Setence; set => _yes_Setence = value; }
    public string No_Setence { get => _no_Setence; set => _no_Setence = value; }
    public bool Tutorial { get => _tutorial; set => _tutorial = value; }

    public void LoadJson()
    {
        StartCoroutine(LoadJsonCoroutine());
    }

    private IEnumerator LoadJsonCoroutine()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        string jsonStr = "";

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            // Android, iOS에서 StreamingAssets에 있는 파일을 읽기 위한 처리
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading JSON file: " + request.error);
                yield break;
            }

            jsonStr = request.downloadHandler.text;
        }
        else
        {
            // Editor 및 Standalone에서의 처리
            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogError("JSON file not found at path: " + filePath);
                yield break;
            }
        }

        // JSON 문자열을 List<DataEntry>로 자동 변환
        dataList = JsonMapper.ToObject<List<DataEntry>>(jsonStr);
    }

    public string PrintDataForName(int no)
    {
        string _name = null;

        DataEntry entry = dataList.Find(e => e.no == no);
        if (entry != null)
        {
            //Debug.Log("No: " + entry.no);
            //Debug.Log("Name: " + entry.Name);
            //Debug.Log("Setence: " + entry.setence);
            //Debug.Log("Yes: " + (entry.Yes.HasValue ? entry.Yes.Value.ToString() : "null"));
            //Debug.Log("No: " + (entry.No.HasValue ? entry.No.Value.ToString() : "null"));
            _name = entry.Name;
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        return _name;
    }

    public string[] AddTalk(int no, int type)
    {
        string[] _result = null;
        List<string> _tempList = new List<string>();
        _actionTypeList = new List<int>();
        _actionType = new List<int>();

        GeneratorTalk(no, type, _tempList);

        _result = _tempList.ToArray();

        return _result;
    }

    public void GeneratorTalk(int no, int type, List<string> _talks)
    {
        string _setence = null;

        string _yes = null;
        string _no = null;
        _txtNumber = no;
        DataEntry entry = dataList.Find(e => e.no == no);

        if (entry != null)
        {
            _setence = entry.setence;
            if (entry.Types != 0)
            {
                _actionTypeList.Add(_talks.Count);
                _actionType.Add(entry.Types);
            }
            _yes = (entry.Yes.HasValue ? entry.Yes.Value.ToString() : null);
            _no = (entry.No.HasValue ? entry.No.Value.ToString() : null);
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        _talks.Add(_setence);

        if (entry.EndTalk == "" && (_yes == null && _no == null))
        {
            GeneratorTalk(no + 1, type, _talks);
        }
        else if (_yes != null && _no != null)
        {
            YesNumber = int.Parse(_yes);
            NoNumber = int.Parse(_no);
            Yes_Setence = entry.Yes_setence;
            No_Setence = entry.No_setence;
        }
        else
        {
            YesNumber = -1;
            NoNumber = -1;
            Yes_Setence = null;
            No_Setence = null;
        }
    }

    public string PrintDataForEndTalk(int no)
    {
        string _endTalk = null;

        DataEntry entry = dataList.Find(e => e.no == no);
        if (entry != null)
        {
            _endTalk = entry.EndTalk;
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        return _endTalk;
    }

    public string PrintDataForSentence(int no)
    {
        string _setence = null;

        DataEntry entry = dataList.Find(e => e.no == no);
        if (entry != null)
        {
            _setence = entry.setence;
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        return _setence;
    }

    public int PrintDataForTypes(int no)
    {
        int _types = -1;

        DataEntry entry = dataList.Find(e => e.no == no);
        if (entry != null)
        {
            _types = entry.Types;
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        return _types;
    }

    public int PrintDataForYes(int no)
    {
        int _yes = -1;

        DataEntry entry = dataList.Find(e => e.no == no);
        if (entry != null)
        {
            _yes = entry.Yes.HasValue ? entry.Yes.Value : -1;
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        return _yes;
    }

    public int PrintDataForNo(int no)
    {
        int _no = -1;

        DataEntry entry = dataList.Find(e => e.no == no);
        if (entry != null)
        {
            _no = entry.No.HasValue ? entry.No.Value : -1;
        }
        else
        {
            Debug.LogWarning("No entry found for no: " + no);
        }

        return _no;
    }

    public void ActionType(int _number)
    {
        Debug.Log("test");

        switch (_number)
        {
            case 0:
                break;
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                DialogueManager.Instance.SignPanelUI();
                break;
            case 3:
                GameObject spawnEffect = Instantiate(LobbyManager.Instance.SpawnEffect, new Vector3(LobbyManager.Instance.LobbyMonster.transform.position.x, LobbyManager.Instance.LobbyMonster.transform.position.y - 2.5f, LobbyManager.Instance.LobbyMonster.transform.position.z), Quaternion.identity);
                LobbyManager.Instance.LobbyMonster.gameObject.SetActive(true);
                Camera.main.GetComponent<CameraShake>().ShakeCamera(0.3f, 0.8f);
                break;
            case 4:
                LobbyManager.Instance.DungeonArrowObject.GetComponent<UnityEngine.UI.Image>().rectTransform.sizeDelta = new Vector2(9000, 9000);
                LobbyManager.Instance.DungeonArrowObject.enabled = true;
                LobbyManager.Instance.SetArrow();
                break;
            case 5:
                GameManager.Instance.UiManager.ShowArrow();
                break;
            case 6:
                GameManager.Instance.EnemyGenerator.SpawnEnemy(1);
                break;
            case 7:
                GameManager.Instance.UiManager.ShowArrow();
                break;
            case 8:
                GameManager.Instance.GameStop = false;
                break;
            case 9:
                GameManager.Instance.EnemyGenerator.enermy();
                break;
            case 10:
                Destroy(GameObject.FindGameObjectWithTag("Enermy"));
                GameManager.Instance.GameStop = false;
                GameManager.Instance.EnemyGenerator.SpawnEnemy(10);
                GameManager.Instance.CoinDrop(GameManager.Instance.UiManager._coin_Text.position, 12);
                break;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        LoadJson();
    }
}
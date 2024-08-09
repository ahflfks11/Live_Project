using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class JsonParseManager : MonoBehaviour
{
    private static JsonParseManager instance;

    private int _textNumber = 1;

    string jsonFileName = "data.json";

    [System.Serializable]
    public class DataEntry
    {
        public int no;
        public string Name;
        public int value;
        public string setence;
        public int? Yes;
        public int? No;
    }

    private List<DataEntry> dataList = new List<DataEntry>();

    public static JsonParseManager Instance { get => instance; set => instance = value; }

    public void LoadJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string jsonStr = File.ReadAllText(filePath);

            // JSON 문자열을 List<DataEntry>로 자동 변환
            dataList = JsonMapper.ToObject<List<DataEntry>>(jsonStr);
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
        }
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
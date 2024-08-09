using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class JsonParseManager : MonoBehaviour
{
    private static JsonParseManager instance;

    // JSON 파일 경로
    public string jsonFileName = "data.json";

    // JSON 데이터를 담을 클래스
    [System.Serializable]
    public class DataEntry
    {
        public string Name;
        public int value;
        public string setence;
    }

    // JSON 데이터를 저장할 Dictionary
    private Dictionary<string, DataEntry> dataDict = new Dictionary<string, DataEntry>();

    public static JsonParseManager Instance { get => instance; set => instance = value; }

    // JSON 파일을 읽고 데이터를 처리하는 함수
    public void LoadJson()
    {
        // JSON 파일 경로 설정
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // JSON 파일 내용을 문자열로 읽기
            string jsonStr = File.ReadAllText(filePath);

            // JSON 문자열을 Dictionary로 변환
            JsonData jsonData = JsonMapper.ToObject(jsonStr);

            foreach (string key in jsonData.Keys)
            {
                JsonData entryData = jsonData[key];
                DataEntry entry = new DataEntry
                {
                    Name = entryData["Name"].ToString(),
                    value = int.Parse(entryData["value"].ToString()),
                    setence = entryData["setence"].ToString()
                };
                dataDict.Add(key, entry);
            }
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
        }
    }

    // 특정 키에 해당하는 데이터를 출력하는 함수
    public void PrintDataForKey(string key)
    {
        if (dataDict.ContainsKey(key))
        {
            DataEntry entry = dataDict[key];
            Debug.Log("Key: " + key);
            Debug.Log("Name: " + entry.Name);
            Debug.Log("Setence: " + entry.setence);
        }
        else
        {
            Debug.LogWarning("Key not found: " + key);
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

    // Start 함수에서 JSON 파일 읽기 및 특정 키 값 출력
    void Start()
    {
        LoadJson();

        // 예시로 특정 키 "1"에 해당하는 데이터를 출력
        PrintDataForKey("1");
    }
}

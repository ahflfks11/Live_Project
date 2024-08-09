using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class JsonParseManager : MonoBehaviour
{
    private static JsonParseManager instance;

    // JSON ���� ���
    public string jsonFileName = "data.json";

    // JSON �����͸� ���� Ŭ����
    [System.Serializable]
    public class DataEntry
    {
        public string Name;
        public int value;
        public string setence;
    }

    // JSON �����͸� ������ Dictionary
    private Dictionary<string, DataEntry> dataDict = new Dictionary<string, DataEntry>();

    public static JsonParseManager Instance { get => instance; set => instance = value; }

    // JSON ������ �а� �����͸� ó���ϴ� �Լ�
    public void LoadJson()
    {
        // JSON ���� ��� ����
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePath))
        {
            // JSON ���� ������ ���ڿ��� �б�
            string jsonStr = File.ReadAllText(filePath);

            // JSON ���ڿ��� Dictionary�� ��ȯ
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

    // Ư�� Ű�� �ش��ϴ� �����͸� ����ϴ� �Լ�
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

    // Start �Լ����� JSON ���� �б� �� Ư�� Ű �� ���
    void Start()
    {
        LoadJson();

        // ���÷� Ư�� Ű "1"�� �ش��ϴ� �����͸� ���
        PrintDataForKey("1");
    }
}

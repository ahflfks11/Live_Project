using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private UnitManager unitManager;
    private UIManager uiManager;
    public Transform myArea;
    private int _gold;
    private int _requireGold;
    int clickCount;
    private float _limitTimer = 40f;
    float _setTime;
    float _sectime;
    int _mintime;

    private int _wave = 1;
    private int _enermyCount = 0;

    public UnitData[] _unitObject;
    public GameObject Exclamation;

    public GameObject _gameOverText;

    public Color[] _rareColor;
    public Color _hiddenColor;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public UnitManager UnitManager { get => unitManager; set => unitManager = value; }
    public int Gold { get => _gold; set => _gold = value; }
    public int EnermyCount { get => _enermyCount; set => _enermyCount = value; }
    public int Wave { get => _wave; set => _wave = value; }
    public float Sectime { get => _sectime; set => _sectime = value; }
    public int Mintime { get => _mintime; set => _mintime = value; }
    public float LimitTimer { get => _limitTimer; set => _limitTimer = value; }
    public float SetTime { get => _setTime; set => _setTime = value; }
    public int RequireGold { get => _requireGold; set => _requireGold = value; }
    public UIManager UiManager { get => uiManager; set => uiManager = value; }
    public int ClickCount { get => clickCount; set => clickCount = value; }

    public void SpwnUnit()
    {
        UnitManager.SpawnUnit(myArea.position);
    }

    public void SpecialSpawnUnit()
    {
        UnitManager.SpecialSpawnUnit(myArea.position);
    }

    public void LevelUp()
    {
        UnitManager.maxCount++;
    }

    private void Start()
    {
        UnitManager = GameObject.FindObjectOfType<UnitManager>();
        UiManager = GameObject.FindObjectOfType<UIManager>();
        _gold = 25;
        RequireGold = 3;
        SetTime = _limitTimer;
        ClickCount = 0;
    }

    private void Update()
    {
        SetTime -= Time.deltaTime;

        if (SetTime >= 60f)
        {
            Mintime = (int)SetTime / 60;
            Sectime = SetTime % 60;
        }

        if(SetTime < 60f)
        {
            Mintime = 0;
            Sectime = (int)SetTime;
        }

        if (SetTime <= 0f)
        {
            SetTime = LimitTimer;
            _wave++;
            UiManager.Wave(Wave);
        }

        _unitObject = FindObjectsOfType<UnitData>();
    }
}

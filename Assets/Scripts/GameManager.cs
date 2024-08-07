using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float _bossTimer = 90f;
    float _setTime;
    float _sectime;
    int _mintime;

    private int _wave = 1;
    private int _enermyCount = 0;
    private int _bossCount = 0;

    public UnitData[] _unitObject;
    public GameObject Exclamation;

    public GameObject _gameOverText;

    public Color[] _rareColor;
    public Color _hiddenColor;

    [SerializeField] bool _isBoss = false;

    [SerializeField] private float gameSpeed = 0;

    [SerializeField] private EnermyCoinText _coinText;

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
    public float GameSpeed { get => gameSpeed; set => gameSpeed = value; }
    public bool IsBoss { get => _isBoss; set => _isBoss = value; }
    public int BossCount { get => _bossCount; set => _bossCount = value; }
    public EnermyCoinText CoinText { get => _coinText; set => _coinText = value; }

    public void GameInit()
    {
        UnitManager = GameObject.FindObjectOfType<UnitManager>();
        UiManager = GameObject.FindObjectOfType<UIManager>();
        _gold = 25;
        RequireGold = 3;
        SetTime = _limitTimer;
        ClickCount = 0;
        myArea = GameObject.Find("SpawnPoint").transform;
    }

    private void Start()
    {
        GameInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 1f)
                Time.timeScale = 2f;
            else
                Time.timeScale = 1f;
        }

        if (unitManager == null && GameObject.FindObjectOfType<UnitManager>())
        {
            GameInit();
        }
        else if (unitManager != null)
        {
            SetTime -= Time.deltaTime;

            if (SetTime >= 60f)
            {
                Mintime = (int)SetTime / 60;
                Sectime = (int)SetTime % 60;
            }

            if (SetTime < 60f)
            {
                Mintime = 0;
                Sectime = (int)SetTime;
            }

            if (SetTime <= 0f)
            {
                if ((_wave + 1) % 10 == 0 && !IsBoss)
                {
                    IsBoss = true;
                    SetTime = _bossTimer;
                    UiManager.BossWave();
                }
                else
                {
                    if (IsBoss)
                    {
                        if (uiManager.SkipUIPanel.enabled)
                            uiManager.SkipUI();

                        if (GameObject.Find("Boss"))
                        {
                            
                        }

                        IsBoss = false;
                        BossCount++;
                    }

                    SetTime = LimitTimer;
                    _wave++;

                    UiManager.Wave(Wave);
                }
            }

            _unitObject = FindObjectsOfType<UnitData>();
        }
    }
}

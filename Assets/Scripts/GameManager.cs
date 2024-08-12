using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private UnitManager unitManager;
    private UIManager uiManager;
    private EnermyGenerator _enemyGenerator;
    public Transform myArea;
    private int _gold;
    private int _requireGold;
    int clickCount;
    [SerializeField] private float _limitTimer = 40f;
    private float _bossTimer = 90f;
    float _setTime;
    float _sectime;
    int _mintime;

    [SerializeField] private int _wave = 1;
    private int _enermyCount = 0;
    private int _bossCount = 0;

    public UnitData[] _unitObject;
    public GameObject Exclamation;

    public GameObject _gameOverText;

    public Color[] _rareColor;
    public Color _hiddenColor;
    public bool _SpawnComplete;
    bool _gameStart;
    bool _gameStop;

    [SerializeField] bool _isBoss = false;

    [SerializeField] private float gameSpeed = 0;

    [SerializeField] private EnermyCoinText _coinText;

    [SerializeField] private TMPro.TMP_Text _logText;

    [SerializeField] private GameObject _warningBoss;

    [SerializeField] private GameObject _coinDropObject;

    [SerializeField] private GameObject[] _spawnEffect;

    private void Awake()
    {
        /*
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        */

        instance = this;
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
    public bool GameStart { get => _gameStart; set => _gameStart = value; }
    public bool GameStop { get => _gameStop; set => _gameStop = value; }
    public EnermyGenerator EnemyGenerator { get => _enemyGenerator; set => _enemyGenerator = value; }

    public void GameInit()
    {
        UnitManager = GameObject.FindObjectOfType<UnitManager>();
        UiManager = GameObject.FindObjectOfType<UIManager>();
        _enemyGenerator = FindObjectOfType<EnermyGenerator>();
        ClickCount = 0;
        myArea = GameObject.Find("SpawnPoint").transform;

        if (JsonParseManager.Instance.Tutorial)
        {
            _gold = 3;
            RequireGold = 3;
            GameStart = false;
            GameStop = true;
            DialogueManager.Instance.TalkLauncher(8);
        }
        else
        {
            _gold = 20;
            RequireGold = 3;
            SetTime = _limitTimer;
            GameStart = true;
            UiManager.Wave(GameManager.Instance.Wave);
        }
    }

    public void Lobby()
    {
        Time.timeScale = 1f;

        if (GameObject.Find("GoogleManager"))
            Transitioner.Instance.TransitionToScene(1);
        else
            SceneManager.LoadScene(1);
    }

    public void CreateSpawnEffect(int _effectNumber, Transform parents)
    {
        GameObject _spawnEffectPrefab = Instantiate(_spawnEffect[_effectNumber], parents.position, Quaternion.identity);
        _spawnEffectPrefab.transform.SetParent(parents);
        _spawnEffectPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
        _spawnEffectPrefab.transform.position = Vector3.zero;
    }

    public void Log(string _text)
    {
        _logText.text = _text;
    }

    public void Warning()
    {
        GameObject _bossWarningObject = Instantiate(_warningBoss, _warningBoss.transform.position, Quaternion.identity);
    }

    public void CoinDrop(Vector3 _pos, int _coin)
    {
        Vector3 _tempPos = new Vector3(_pos.x + 0.5f, _pos.y, _pos.z);
        GameObject coin = Instantiate(_coinDropObject, _tempPos, Quaternion.identity);
        coin.GetComponentInChildren<TMPro.TMP_Text>().text = "+" + _coin + "G";
        coin.transform.DOMove(new Vector3(_tempPos.x, _tempPos.y + 0.5f, _tempPos.z), 1f);
        _gold += _coin;
    }

    private void Start()
    {
        GameInit();
        AudioManager.instance.PlayBgm(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Time.timeScale = 0f;
        _unitObject = FindObjectsOfType<UnitData>();

        if (!GameStart)
            return;

        if (_enermyCount > 50)
        {
            Time.timeScale = 0;
            uiManager.EndGameUI();
            return;
        }

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
                Warning();
            }
            else
            {
                if (IsBoss)
                {
                    if (uiManager.SkipUIPanel.enabled)
                        uiManager.SkipUI();

                    if (GameObject.Find("Boss"))
                    {
                        Time.timeScale = 0f;
                        uiManager.EndGameUI();
                        return;
                    }

                    IsBoss = false;
                    BossCount++;
                }

                SetTime = LimitTimer;
                _wave++;

                UiManager.Wave(Wave);
            }
        }
    }
}

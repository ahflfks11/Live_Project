using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum EliteSpawnType
{
    소환불가,
    소환가능,
    소환중,
    소환해제
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private UnitManager unitManager;
    private UIManager uiManager;
    private EnermyGenerator _enemyGenerator;
    public Transform myArea;
    [SerializeField] private int _gold;
    private int _requireGold;
    int clickCount;
    [SerializeField] private float _limitTimer = 40f;
    private int _legendCount;
    //소환 스크롤
    [SerializeField] private int[] _RarilitySpawnCount = { 0, 0, 0, 0 };
    private float _bossTimer = 90f;
    float _setTime;
    float _sectime;
    int _mintime;

    [SerializeField] private int _wave = 1;
    [SerializeField] private int _enermyCount = 0;
    [SerializeField] private int _bossCount = 0;

    public UnitData[] _unitObject;
    public GameObject[] _specialUIEffect; //히든 출현 or 전설 출현 이펙트

    public GameObject _gameOverText;

    public Color[] _rareColor;
    public Color _hiddenColor;
    public bool _SpawnComplete;
    bool _gameStart;
    bool _gameStop;

    bool _isAutoRevolution;

    bool _restTimeState;
    bool _tempRestTimeState;

    float _restTime = 60;

    float _timeScaleValue;
    AudioSource _sfxAudio;

    [SerializeField] bool _isBoss = false;

    [SerializeField] private float gameSpeed = 0;

    [SerializeField] private EnermyCoinText _coinText;

    [SerializeField] private TMPro.TMP_Text _logText;

    [SerializeField] private GameObject _warningBoss;

    [SerializeField] private GameObject _coinDropObject;

    [SerializeField] private GameObject[] _spawnEffect;

    private EliteSpawnType[] _eliteSpawnState;

    private void Awake()
    {
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
    public int[] RarilitySpawnCount { get => _RarilitySpawnCount; set => _RarilitySpawnCount = value; }
    public float TimeScaleValue { get => _timeScaleValue; set => _timeScaleValue = value; }
    public EliteSpawnType[] EliteSpawnState { get => _eliteSpawnState; set => _eliteSpawnState = value; }
    public int LegendCount { get => _legendCount; set => _legendCount = value; }
    public bool RestTimeState { get => _restTimeState; set => _restTimeState = value; }

    public void GameInit()
    {
        UnitManager = GameObject.FindObjectOfType<UnitManager>();
        UiManager = GameObject.FindObjectOfType<UIManager>();
        _enemyGenerator = FindObjectOfType<EnermyGenerator>();
        ClickCount = 0;
        myArea = GameObject.Find("SpawnPoint").transform;
        Time.timeScale = 1f;
        TimeScaleValue = Time.timeScale;
        _sfxAudio = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("AutoRevolution"))
        {
            if (PlayerPrefs.GetInt("AutoRevolution") == 0)
            {
                uiManager.SetAutoRevolutionToggle(false);
            }
            else
            {
                uiManager.SetAutoRevolutionToggle(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("AutoRevolution", 0);
            PlayerPrefs.Save();
        }

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

    public int CalculateCrystals(int stage)
    {
        int baseCrystals = 0;
        int incrementPer10Stages = 60;

        int numberOfIncrements = (stage - 1) / 10;
        int totalCrystals = baseCrystals + (incrementPer10Stages * numberOfIncrements);

        return totalCrystals;
    }

    public void Lobby()
    {
        SceneManager.LoadScene(1);
    }

    public void CreateSpecialUIEffect(bool _specialUnit, Vector3 _pos, int _level)
    {
        GameObject _specialUIEffectPrefab;

        if (_specialUnit)
        {
            _specialUIEffectPrefab = Instantiate(_specialUIEffect[1], _pos, Quaternion.identity);
        }
        else
        {
            if (_level > 2)
                _specialUIEffectPrefab = Instantiate(_specialUIEffect[0], _pos, Quaternion.identity);
        }
    }

    public void CreateSpawnEffect(int _effectNumber, Transform parents, bool _specialUnit)
    {
        if (!_specialUnit)
        {
            GameObject _spawnEffectPrefab = Instantiate(_spawnEffect[_effectNumber], parents.position, Quaternion.identity);
            _spawnEffectPrefab.transform.SetParent(parents);
            if (_spawnEffectPrefab.gameObject.tag != "HiddenEffect")
            {
                _spawnEffectPrefab.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                _spawnEffectPrefab.transform.localPosition = new Vector3(0f, 0.4f, 0f);
                _spawnEffectPrefab.transform.parent = null;
            }
            else
            {
                _spawnEffectPrefab.transform.localScale = new Vector3(4f, 4f, 4f);
                _spawnEffectPrefab.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
                _spawnEffectPrefab.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }
        else
        {
            GameObject _spawnEffectPrefab = Instantiate(_spawnEffect[4], parents.position, Quaternion.identity);
            _spawnEffectPrefab.transform.SetParent(parents);
            _spawnEffectPrefab.transform.localScale = new Vector3(6f, 6f, 6f);
            _spawnEffectPrefab.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            _spawnEffectPrefab.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    public void Log(string _text)
    {
        _logText.text = _text;
    }

    public void Warning()
    {
        GameObject _bossWarningObject = Instantiate(_warningBoss, _warningBoss.transform.position, Quaternion.identity);
    }

    public void SetSfx(int _sfxNumber)
    {
        _sfxAudio.clip = AudioManager.instance.SetClip(_sfxNumber);
        _sfxAudio.Play();
    }

    public void CoinDrop(Vector3 _pos, int _coin)
    {
        _coinDropObject.GetComponent<UnityEngine.UI.Text>().text = "+" + _coin;
        _coinDropObject.GetComponent<DOTweenVisualManager>().enabled = true;
        //Vector3 _tempPos = new Vector3(_pos.x + 0.5f, _pos.y+5f, _pos.z);
        //GameObject coin = Instantiate(_coinDropObject, _tempPos, Quaternion.identity);
        //coin.GetComponentInChildren<TMPro.TMP_Text>().text = "+" + _coin + "G";
        //coin.transform.DOMove(new Vector3(_tempPos.x, _tempPos.y + 0.5f, _tempPos.z), 1f);
        _gold += _coin;
    }

    private void Start()
    {
        GameInit();
        AudioManager.instance.PlayInGameBgm(true);
    }

    public void AutoRevolution()
    {
        for (int i = 0; i < _unitObject.Length; i++)
        {
            _unitObject[i].Revolution();
        }
    }

    public void SetRevolutionBtn()
    {
        if (!_isAutoRevolution)
        {
            PlayerPrefs.SetInt("AutoRevolution", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("AutoRevolution", 0);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        _unitObject = FindObjectsOfType<UnitData>();

        if (!GameStart)
            return;

        if (_enermyCount > 50)
        {
            uiManager.EndGameUI(false);
            return;
        }

        if (_tempRestTimeState != RestTimeState)
        {
            SetTime = _restTime;
            RestTimeState = _tempRestTimeState;
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
            if (_restTimeState)
            {
                _restTimeState = false;
            }
            else
            {
                if (Wave + 1 == 65)
                {
                    _tempRestTimeState = true;
                    UiManager.Wave(-1);
                    return;
                }
            }

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
                        uiManager.EndGameUI(false);
                        return;
                    }

                    IsBoss = false;
                    //BossCount++;
                }

                SetTime = LimitTimer;
                _wave++;

                UiManager.Wave(Wave);
            }
        }

        _isAutoRevolution = uiManager.AutoRevolutionToggle.isOn;

        if (_isAutoRevolution)
            AutoRevolution();

        if (uiManager.PauseToggle.isOn)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = TimeScaleValue;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!_tempRestTimeState)
                _tempRestTimeState = true;
            else
                _tempRestTimeState = false;
        }
    }
}

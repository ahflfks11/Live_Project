using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text waveText;
    public Text _goldText;
    public Text _monsterText;
    public Text _requireText;
    public CartoonFX.CFXR_ParticleText _waveText;
    public Text _timerText;
    public Transform _content;
    public Transform _coin_Text;
    public Image _SpeedImage;
    public Text _spawnRateText;
    public Text _hiddenPossibleText;
    public Text _legendCountText;

    public GameObject _RevolutionImageObject;
    [SerializeField] private DOTweenVisualManager _EnforceShopPanel;
    [SerializeField] private EnforceUI _enforceUI;
    [SerializeField] private DOTweenVisualManager _skipUIPanel;
    [SerializeField] private DOTweenVisualManager _settingUIPanel;
    [SerializeField] private DOTweenVisualManager _arrow;
    [SerializeField] private DOTweenVisualManager _limitUIObject;
    [SerializeField] private DOTweenVisualManager _tutorialArrow;
    [SerializeField] private DOTweenVisualManager _restBattleWarningObject; //전투 준비 패널
    [SerializeField] private EndGameUI _endGamePanel;
    [SerializeField] private MissionUI _missionPanel;
    [SerializeField] private RemoveUI _removePanel;
    [SerializeField] private GameObject _developMode_Btn;
    [SerializeField] private TMPro.TMP_InputField _develop_WaveText;

    [SerializeField] private Toggle _autoRevolutionToggle;
    [SerializeField] private Toggle _pauseToggle;

    public EnforceUI EnforceUI { get => _enforceUI; set => _enforceUI = value; }
    public DOTweenVisualManager SkipUIPanel { get => _skipUIPanel; set => _skipUIPanel = value; }
    public Toggle AutoRevolutionToggle { get => _autoRevolutionToggle; set => _autoRevolutionToggle = value; }
    public Toggle PauseToggle { get => _pauseToggle; set => _pauseToggle = value; }

    public void OpenPanel(UnitData _data)
    {
        GameObject[] _image = GameObject.FindGameObjectsWithTag("Rev");

        for (int i = 0; i < _image.Length; i++)
        {
            Destroy(_image[i]);
        }

        if (_data._data.EvolutionAvailability)
        {
            bool _isOpen = false;

            for (int i = 0; i < GameManager.Instance.UnitManager._soldiers.Count; i++)
            {
                if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._unit == _data._data._unit && _data._data.rarelityLevel + 1 == GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data.rarelityLevel)
                {
                    GameObject _revolutionObject = Instantiate(_RevolutionImageObject, _content.position, Quaternion.identity);
                    _revolutionObject.transform.SetParent(_content);
                    _revolutionObject.transform.localScale = _RevolutionImageObject.transform.localScale;
                    _revolutionObject.GetComponent<Revolution_Sprite>()._spr.sprite = GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._spr;
                    _revolutionObject.transform.localPosition = Vector3.zero;
                    _isOpen = true;
                }
                else
                {
                    if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._multiUnit.Length != 0)
                    {
                        for (int j = 0; j < GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._multiUnit.Length; j++)
                        {
                            if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._multiUnit[j] == GameManager.Instance.UnitManager._soldiers[i])
                            {
                                GameObject _revolutionObject = Instantiate(_RevolutionImageObject, _content.position, Quaternion.identity);
                                _revolutionObject.transform.SetParent(_content);
                                _revolutionObject.transform.localScale = _RevolutionImageObject.transform.localScale;
                                _revolutionObject.GetComponent<Revolution_Sprite>()._spr.sprite = GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._spr;
                                _revolutionObject.transform.localPosition = Vector3.zero;
                                _isOpen = true;
                            }
                        }
                    }
                }
            }

            if (_isOpen)
            {
                //_RevolutionPanel.SetActive(true);
            }
        }
    }

    public void SetWave()
    {
        if (GameObject.Find("Boss"))
        {
            Destroy(GameObject.Find("Boss"));
        }

        GameManager.Instance.IsBoss = false;
        GameManager.Instance.TempRestTimeState = false;
        GameManager.Instance.RestTimeState = false;
        GameManager.Instance.Wave = int.Parse(_develop_WaveText.text) - 1;
        GameManager.Instance.SetTime = 0f;
    }

    public void SetRestImage(bool _state)
    {
        _restBattleWarningObject.enabled = _state;
    }

    public void SetAutoRevolutionToggle(bool _state)
    {
        AutoRevolutionToggle.isOn = _state;
    }

    public void EndGameUI(bool _clear)
    {
        if (_endGamePanel.gameObject.activeSelf)
            return;

        Time.timeScale = 0;
        _endGamePanel.gameObject.SetActive(true);
        int _rareCount = 0;
        int _legendCount = 0;
        for (int i = 0; i < GameManager.Instance._unitObject.Length; i++)
        {
            if (GameManager.Instance._unitObject[i]._data.rarelityLevel == 2)
            {
                _rareCount++;
            } else if (GameManager.Instance._unitObject[i]._data.rarelityLevel == 3)
            {
                _legendCount++;
            }
        }

        if (!_clear)
        {
            if (GPGSManager.Instance.Level >= 0)
                GPGSManager.Instance.ClearStage(GameManager.Instance.Wave, GameManager.Instance.CalculateCrystals(GameManager.Instance.Wave), 100 * GameManager.Instance.Wave, _legendCount, _rareCount);
            _endGamePanel.SetUI(GameManager.Instance.Wave, _legendCount, _rareCount);
        }
        else
        {
            if (GPGSManager.Instance.Level >= 0)
                GPGSManager.Instance.ClearStage(GameManager.Instance.Wave + 1, GameManager.Instance.CalculateCrystals(GameManager.Instance.Wave + 1), 100 * GameManager.Instance.Wave + 1, _legendCount, _rareCount);
            _endGamePanel.SetUI(GameManager.Instance.Wave + 1, _legendCount, _rareCount);
        }
    }

    public void LimitUI()
    {
        if (_limitUIObject.enabled)
            _limitUIObject.enabled = false;
        else
            _limitUIObject.enabled = true;
    }

    public void LimitUI(bool _status)
    {
        _limitUIObject.enabled = _status;
    }

    public void MissionPanel()
    {
        if (JsonParseManager.Instance.Tutorial)
            return;

        _missionPanel.SetUI();
    }

    public void RemovePanel()
    {
        if (JsonParseManager.Instance.Tutorial)
            return;

        _removePanel.SetUI();
    }

    public void SetArrow(Transform _parents, Vector3 vec, string _objectName)
    {
        GameObject _tutorialArrowPrefabs = Instantiate(_tutorialArrow.gameObject, Vector3.zero , Quaternion.identity);
        _tutorialArrowPrefabs.transform.SetParent(_parents);
        _tutorialArrowPrefabs.transform.localPosition = new Vector3(-0.05f, 0.8f, 0f);
        _tutorialArrowPrefabs.transform.localScale = Vector3.zero;
        _tutorialArrowPrefabs.GetComponentInChildren<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
        _tutorialArrowPrefabs.GetComponent<DOTweenVisualManager>().enabled = true;
        _tutorialArrowPrefabs.gameObject.name = _objectName;
    }

    public void ShowArrow(Transform _parents, float _posY)
    {
        if (_parents != null)
        {
            _arrow.transform.SetParent(_parents);
            _arrow.transform.position = new Vector3(0f, _posY, 0f);
        }


        if (_arrow.enabled)
            _arrow.enabled = false;
        else
            _arrow.enabled = true;
    }

    public void SkipUI()
    {
        if (SkipUIPanel.enabled)
            SkipUIPanel.enabled = false;
        else
            SkipUIPanel.enabled = true;
    }

    public void Skip()
    {
        if(GameManager.Instance.SetTime > 1)
        {
            GameManager.Instance.SetTime = 0f;
            SkipUI();
        }
    }

    public void SettingUI()
    {
        if (_settingUIPanel.enabled)
        {
            _settingUIPanel.enabled = false;
        }
        else
        {
            _settingUIPanel.enabled = true;
        }
    }

    public void EnforceShopPanel()
    {
        if (_EnforceShopPanel.enabled)
        {
            if (JsonParseManager.Instance.Tutorial)
            {
                if (JsonParseManager.Instance._txtNumber < 28)
                    return;
            }

            _EnforceShopPanel.enabled = false;
            AudioManager.instance.ONSFX();
        }
        else
        {
            if (JsonParseManager.Instance.Tutorial)
            {
                if (JsonParseManager.Instance._txtNumber != 27)
                    return;
            }

            _EnforceShopPanel.enabled = true;
            AudioManager.instance.OFFSFX();
        }
    }

    public void Wave(float _level)
    {
        if (_level == -1)
        {
            GameObject text_wave_Object = Instantiate(_waveText.gameObject, Vector2.zero, Quaternion.identity);
            text_wave_Object.GetComponent<CartoonFX.CFXR_ParticleText>().UpdateText("Rest Time");
            waveText.text = "Rest Time";
        }
        else
        {
            GameObject text_wave_Object = Instantiate(_waveText.gameObject, Vector2.zero, Quaternion.identity);
            text_wave_Object.GetComponent<CartoonFX.CFXR_ParticleText>().UpdateText("WAVE" + _level);
            waveText.text = "WAVE" + _level;
        }
    }

    public void BossWave()
    {
        GameObject text_wave_Object = Instantiate(_waveText.gameObject, Vector2.zero, Quaternion.identity);
        text_wave_Object.GetComponent<CartoonFX.CFXR_ParticleText>().UpdateText("Boss Wave");
        waveText.text = "Boss Wave";
    }

    private void Start()
    {
        if (GPGSManager.Instance.Level != -1)
        {
            _developMode_Btn.SetActive(false);
        }
        else
        {
            _developMode_Btn.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _goldText.text = string.Format("{0:#,0}", GameManager.Instance.Gold);
        _monsterText.text = GameManager.Instance.EnermyCount.ToString();
        _requireText.text = GameManager.Instance.RequireGold + " G";

        _legendCountText.text = GameManager.Instance.LegendCount.ToString();

        if (GameManager.Instance.Sectime >= 10f)
        {
            _timerText.text = GameManager.Instance.Mintime + ":" + GameManager.Instance.Sectime;
        }
        else
        {
            _timerText.text = GameManager.Instance.Mintime + ":0" + GameManager.Instance.Sectime;
        }
    }
}

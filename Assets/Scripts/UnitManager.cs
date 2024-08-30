using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    public List<GameObject> _soldiers;
    public List<GameObject> _spawnList;

    public List<GameObject> _tutorialSoldierList;
    public List<GameObject> _tutorialSpecialSoldierList;

    int number = 0;
    public int tutorialSoldierNumber = 0;
    public int tutorialRevolutionSoldierNumber = 0;
    float rndRangeX = 3f; // Default 3f
    float rndRangeY = 3f;
    private float normalEnforceDmg = 0;
    private float rareEnforceDmg = 0;
    private float legendEnforceDmg = 0;
    private float hiddenEnforceDmg = 0;

    private int _tutorialspecialSpawnCount = 0;
    private int _hiddenCharacterCount = 0;

    public int maxCount = 10;

    public int maxSpawnlevel = 0;

    public bool _spawnRateState; //Ȯ��ǥ ����

    public UnitSelector unitSelector;

    public UnityEngine.UI.Text[] _rateText;


    public List<UnitData> ck_List = new List<UnitData>();
    List<UnitData> temp_ck_List = new List<UnitData>();

    [SerializeField] private DamageFonts _dmgFont;

    public DamageFonts DmgFont { get => _dmgFont; set => _dmgFont = value; }
    public float NormalEnforceDmg { get => normalEnforceDmg; set => normalEnforceDmg = value; }
    public float RareEnforceDmg { get => rareEnforceDmg; set => rareEnforceDmg = value; }
    public float LegendEnforceDmg { get => legendEnforceDmg; set => legendEnforceDmg = value; }
    public float HiddenEnforceDmg { get => hiddenEnforceDmg; set => hiddenEnforceDmg = value; }
    public int HiddenCharacterCount { get => _hiddenCharacterCount; set => _hiddenCharacterCount = value; }

    public bool CheckSpawn(GameObject[] _data)
    {
        if (_data.Length > 0)
            return false;

        return true;
    }

    private void Start()
    {
        if (DataManager.Instance.MyHeroList.Count > 0 && GPGSManager.Instance.Level > -1)
        {
            _soldiers = new List<GameObject>();

            for (int i = 0; i < DataManager.Instance.MyHeroList.Count; i++)
            {
                if (DataManager.Instance.MyHeroList[i]._data.specialUnit)
                {
                    _soldiers.Add(DataManager.Instance.MyHeroList[i].gameObject);
                    HiddenCharacterCount++;
                }
                else
                {
                    for (int j = 0; j < DataManager.Instance._data.Length; j++)
                    {
                        if (DataManager.Instance.MyHeroList[i]._data._unit == DataManager.Instance._data[j]._unit._data._unit)
                        {
                            _soldiers.Add(DataManager.Instance._data[j]._unit.gameObject);
                        }
                    }
                }
            }

            _soldiers = _soldiers.Distinct().ToList();
        }
        else
        {
            _soldiers = new List<GameObject>();

            for (int i = 0; i < DataManager.Instance._data.Length; i++)
            {
                _soldiers.Add(DataManager.Instance._data[i]._unit.gameObject);
            }

            _soldiers = _soldiers.Distinct().ToList();
        }
    }

    public void Retry()
    {
        if (!GameManager.Instance.GameStart)
            return;

        GameManager.Instance.GameStart = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void GetLoadSpawnRate()
    {
        if (_spawnRateState)
        {
            GameManager.Instance.UiManager._spawnRateText.text = "�Ϲ� ��ȯǥ";
            _spawnRateState = false;
        }
        else
        {
            GameManager.Instance.UiManager._spawnRateText.text = "Ư�� ��ȯǥ";
            _spawnRateState = true;
        }
    }

    public void Enforce_Normal()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            return;
        }

        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpNormalCoin())
        {
            NormalEnforceDmg += 3;
        }
    }

    public void Enforce_Rare()
    {
        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpRareCoin())
        {
            rareEnforceDmg += 3;
        }

        if (JsonParseManager.Instance.Tutorial)
        {
            DialogueManager.Instance.TalkLauncher(28);
        }
    }

    public void Enforce_Legend()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            return;
        }

        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpLegendCoin())
        {
            legendEnforceDmg += 5;
        }
    }

    public void Enforce_Hidden()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            return;
        }

        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpHiddenCoin())
        {
            hiddenEnforceDmg += 10;
        }
    }

    public void LevelUp()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            return;
        }

        if (GameManager.Instance.UiManager.EnforceUI.SetLevelText())
        {
            if (maxSpawnlevel + 1 < 4)
                maxSpawnlevel++;
        }
    }

    public void RemoveUnit(UnitData.Unit[] _data)
    {
        for (int i = _spawnList.Count; i > 0; i--)
        {
            for (int j = 0; j < _data.Length; j++)
            {
                if (_spawnList[i].GetComponent<UnitData>()._data._unit == _data[j])
                {
                    _spawnList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    //���� ��ȯ
    public void SelectSpawn_Free(GameObject _Solider, Vector3 _pos)
    {
        GameObject _unit;

        _unit = Instantiate(_Solider, _pos, Quaternion.identity);

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        number++;
        _spawnList.Add(_unit);
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform, _unit.GetComponent<UnitData>()._data.specialUnit);
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);

        GameManager.Instance.ClickCount++;
    }

    public void SelectSpawn(GameObject _Solider, Vector3 _pos)
    {
        if (GameManager.Instance.Gold < GameManager.Instance.RequireGold)
            return;

        GameObject _unit;

        _unit = Instantiate(_Solider, _pos, Quaternion.identity);
        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform, _unit.GetComponent<UnitData>()._data.specialUnit);
        number++;
        _spawnList.Add(_unit);

        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);

        GameManager.Instance.ClickCount++;

        GameManager.Instance.Gold -= GameManager.Instance.RequireGold;

        if (GameManager.Instance.ClickCount % 10 == 0)
        {
            GameManager.Instance.RequireGold++;
        }
    }

    public void SelectSpecialSpawn(GameObject _Solider, Vector3 _pos)
    {
        if (GameManager.Instance.Gold < 20)
            return;

        GameObject _unit;

        _unit = Instantiate(_Solider, _pos, Quaternion.identity);

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform, _unit.GetComponent<UnitData>()._data.specialUnit);
        number++;
        _spawnList.Add(_unit);
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);

        GameManager.Instance.ClickCount++;

        GameManager.Instance.Gold -= 20;
    }

    int HiddenSpawnListCount()
    {
        int result = 0;
        for (int i = 0; i < _soldiers.Count; i++)
        {
            if (_soldiers[i].GetComponent<UnitData>()._data.specialUnit)
                result++;
        }

        return result;
    }

    public void TutorialGaveSoldier(UnitData.Unit _unit, int unitLevel, int _limitCount)
    {
        for (int i = 0; i < _soldiers.Count; i++)
        {
            UnitData data = _soldiers[i].GetComponent<UnitData>().GetComponent<UnitData>();
            if (data._data._unit == _unit && data._data.rarelityLevel == unitLevel)
            {
                for (int j = 0; j < _limitCount; j++)
                {
                    Vector3 rndPos = new Vector3(GameManager.Instance.myArea.position.x + Random.Range(-rndRangeX, rndRangeX), GameManager.Instance.myArea.position.y + Random.Range(-rndRangeX, rndRangeY), GameManager.Instance.myArea.position.z);
                    SelectSpawn_Free(_soldiers[i].gameObject, rndPos);
                }
                break;
            }
        }
    }

    //��ư Ŭ���� ��ȯ �Լ�
    public void Spawn(int _spawnNum, Vector3 _pos, bool _randomPosition)
    {
        if (GameManager.Instance.Gold < GameManager.Instance.RequireGold)
            return;

        GameObject _unit;

        if (_randomPosition)
        {
            Vector3 rndPos = new Vector3(_pos.x + Random.Range(-rndRangeX, rndRangeX), _pos.y + Random.Range(-rndRangeX, rndRangeY), _pos.z);
            _unit = Instantiate(_soldiers[_spawnNum], rndPos, Quaternion.identity);
        }
        else
        {
            _unit = Instantiate(_soldiers[_spawnNum], _pos, Quaternion.identity);
        }

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        number++;
        _spawnList.Add(_unit);
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform, _unit.GetComponent<UnitData>()._data.specialUnit);
        GameManager.Instance.CreateSpecialUIEffect(_unit.GetComponent<UnitData>()._data.specialUnit, new Vector3(1, -6f, 0), _unit.GetComponent<UnitData>()._data.rarelityLevel);
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);

        GameManager.Instance.ClickCount++;

        GameManager.Instance.Gold -= GameManager.Instance.RequireGold;

        if (GameManager.Instance.ClickCount % 10 == 0)
        {
            GameManager.Instance.RequireGold++;

        }
    }

    public void SpecialRalitySpawn(int _spawnlevel, Vector3 _pos)
    {
        GameObject _unit;

        List<GameObject> _units = new List<GameObject>();

        for (int i = 0; i < GameManager.Instance.UnitManager._soldiers.Count; i++)
        {
            if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data.rarelityLevel == _spawnlevel)
            {
                _units.Add(GameManager.Instance.UnitManager._soldiers[i]);
            }
        }

        int rndNumber = Random.Range(0, _units.Count);

        _unit = Instantiate(_units[rndNumber], _pos, Quaternion.identity);

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform,_unit.GetComponent<UnitData>()._data.specialUnit);
        GameManager.Instance.CreateSpecialUIEffect(_unit.GetComponent<UnitData>()._data.specialUnit, new Vector3(1, -6f,0), _unit.GetComponent<UnitData>()._data.rarelityLevel);
        number++;
        _spawnList.Add(_unit);
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);

        GameManager.Instance.ClickCount++;
    }


    //��ȭ�� ��ȯ �Լ�
    public void RevolutionSpawn(int _spawnNum, Vector3 _pos, bool _randomPosition)
    {
        GameObject _unit;

        if (_randomPosition)
        {
            Vector3 rndPos = new Vector3(_pos.x + Random.Range(-rndRangeX, rndRangeX), _pos.y + Random.Range(-rndRangeX, rndRangeY), _pos.z);

            _unit = Instantiate(_soldiers[_spawnNum], rndPos, Quaternion.identity);
        }
        else
        {
            _unit = Instantiate(_soldiers[_spawnNum], _pos, Quaternion.identity);
        }

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);
        number++;
        _spawnList.Add(_unit);
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform, _unit.GetComponent<UnitData>()._data.specialUnit);
        GameManager.Instance.CreateSpecialUIEffect(_unit.GetComponent<UnitData>()._data.specialUnit, new Vector3(1, -6f, 0), _unit.GetComponent<UnitData>()._data.rarelityLevel);
        if (JsonParseManager.Instance.Tutorial && JsonParseManager.Instance._txtNumber == 25)
        {
            tutorialRevolutionSoldierNumber++;
            if (tutorialRevolutionSoldierNumber > 1)
            {
                DialogueManager.Instance.TalkLauncher(26);
            }
        }
    }

    //Ư�� ��ȯ
    public void SpecialSpawn(int _spawnNum, Vector3 _pos, bool _randomPosition)
    {
        if (JsonParseManager.Instance.Tutorial && JsonParseManager.Instance._txtNumber == 21 && GameManager.Instance.EnermyCount == 0 && GameManager.Instance._SpawnComplete)
        {
            if (_tutorialspecialSpawnCount < _tutorialSpecialSoldierList.Count)
            {
                Vector3 rndPos = new Vector3(_pos.x + Random.Range(-rndRangeX, rndRangeX), _pos.y + Random.Range(-rndRangeX, rndRangeY), _pos.z);

                SelectSpecialSpawn(_tutorialSpecialSoldierList[_tutorialspecialSpawnCount], rndPos);
                _tutorialspecialSpawnCount++;
                if (_tutorialspecialSpawnCount > 1)
                {
                    DialogueManager.Instance.TalkLauncher(22);
                }

                return;
            }
        }

        if (GameManager.Instance.Gold < 20)
            return;

        GameObject _unit;

        if (_randomPosition)
        {
            Vector3 rndPos = new Vector3(_pos.x + Random.Range(-rndRangeX, rndRangeX), _pos.y + Random.Range(-rndRangeX, rndRangeY), _pos.z);

            _unit = Instantiate(_soldiers[_spawnNum], rndPos, Quaternion.identity);
        }
        else
        {
            _unit = Instantiate(_soldiers[_spawnNum], _pos, Quaternion.identity);
        }

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
        GameManager.Instance.CreateSpawnEffect(_unit.GetComponent<UnitData>()._data.rarelityLevel, _unit.transform, _unit.GetComponent<UnitData>()._data.specialUnit);
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);
        number++;
        _spawnList.Add(_unit);
        GameManager.Instance.Gold -= 20;
    }

    public void SpawnUnit()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            if (JsonParseManager.Instance._txtNumber != 11 && JsonParseManager.Instance._txtNumber != 18)
                return;
        }

        Vector3 pos = GameManager.Instance.myArea.position;

        if (JsonParseManager.Instance.Tutorial)
        {
            if(tutorialSoldierNumber < _tutorialSoldierList.Count)
            {
                if (tutorialSoldierNumber == 0)
                {
                    GameManager.Instance.GameStop = false;
                }

                SelectSpawn(_tutorialSoldierList[tutorialSoldierNumber], pos);
                tutorialSoldierNumber++;
                return;
            }
        }
        int rnd_level = unitSelector.GetUnitGrade(-1) - 1;

        /*
        List<int> _numberList = new List<int>();

        for(int i=0; i<_soldiers.Count; i++)
        {
            if (_soldiers[i].GetComponent<UnitData>()._data.rarelityLevel == rnd_level && !_soldiers[i].GetComponent<UnitData>()._data.specialUnit)
            {
                _numberList.Add(i);
            }
        }
        */

        List<int> _numberList = new List<int>();

        double sum = 0;
        int rnd_UnitNumber = 0;

        for (int i = 0; i < _soldiers.Count; i++)
        {
            if (_soldiers[i].GetComponent<UnitData>()._data.rarelityLevel == rnd_level)
            {
                _numberList.Add(i);
                sum += _soldiers[i].GetComponent<UnitData>()._data.ingame_Weight;
            }
        }

        sum *= Random.value;

        for (int i = 0; i < _numberList.Count; i++)
        {
            sum -= _soldiers[_numberList[i]].GetComponent<UnitData>()._data.ingame_Weight;
            if (sum <= 0)
            {
                rnd_UnitNumber = i;
                break;
            }
        }

        //int rnd_UnitNumber = Random.Range(0, _numberList.Count);

        /*
        int rnd_UnitNumber = Random.Range(0, _soldiers.Count);

        if (!CheckSpawn(_soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data._multiUnit) || _soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data.rarelityLevel > maxSpawnlevel || _soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data.specialUnit)
        {
            SpawnUnit();
            return;
        }
        */

        Spawn(_numberList[rnd_UnitNumber], pos, true);
    }

    public void SpecialSpawnUnit()
    {
        Vector3 pos = GameManager.Instance.myArea.position;


        int rnd_Number = unitSelector.GetUnitGrade(maxSpawnlevel + 1);
        List<GameObject> _unit = new List<GameObject>();

        int rnd_UnitNumber = 0;
        double sum = 0;

        for (int i = 0; i < _soldiers.Count; i++)
        {
            if (_soldiers[i].GetComponent<UnitData>()._data.rarelityLevel == rnd_Number - 1)
            {
                _unit.Add(_soldiers[i]);
                sum += _soldiers[i].GetComponent<UnitData>()._data.ingame_Weight;
            }
        }

        int rnd_key = 0;

        sum *= Random.value;

        for (int i = 0; i < _unit.Count; i++)
        {
            sum -= _unit[i].GetComponent<UnitData>()._data.ingame_Weight;
            if (sum <= 0)
            {
                rnd_key = i;
                break;
            }
        }

        //int rnd_key = Random.Range(0, _unit.Count);

        for (int i = 0; i < _soldiers.Count; i++)
        {
            if (_soldiers[i] == _unit[rnd_key])
            {
                rnd_UnitNumber = i;
                break;
            }
        }

        /*
        int rnd_UnitNumber = Random.Range(0, _soldiers.Count);


        if (!CheckSpawn(_soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data._multiUnit) || _soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data.rarelityLevel > maxSpawnlevel + 1)
        {
            SpecialSpawnUnit();
            return;
        }
        */


        SpecialSpawn(rnd_UnitNumber, pos, true);
    }

    bool SearchSacrifice(GameObject _unitdata)
    {
        bool _chk = false;

        foreach (UnitData unitdata in GameManager.Instance._unitObject)
        {
            if (unitdata.gameObject.name == _unitdata.name)
            {
                temp_ck_List.Add(unitdata);
                _chk = true;
            }
        }

        return _chk;
    }

    public void GameSpeed()
    {
        if (Time.timeScale == 1f)
        {
            GameManager.Instance.UiManager._SpeedImage.color = Color.yellow;
            Time.timeScale = 2f;
        }
        else
        {
            GameManager.Instance.UiManager._SpeedImage.color = Color.white;
            Time.timeScale = 1f;
        }

        GameManager.Instance.TimeScaleValue = Time.timeScale;
    }

    private void Update()
    {
        if (!_spawnRateState)
        {
            string[] probabilityArray = unitSelector.GetGradeProbabilitiesAsArray(-1);
            GameManager.Instance.UiManager._spawnRateText.color = Color.white;
            for (int i = 0; i < probabilityArray.Length; i++)
            {
                if (float.Parse(probabilityArray[i]) == 0)
                {
                    _rateText[i].color = Color.white;
                }
                else
                {
                    _rateText[i].color = Color.yellow;
                }

                _rateText[i].text = probabilityArray[i] + "%";

            }
        }
        else
        {
            string[] probabilityArray = unitSelector.GetGradeProbabilitiesAsArray(maxSpawnlevel + 1);
            GameManager.Instance.UiManager._spawnRateText.color = Color.yellow;
            for (int i = 0; i < probabilityArray.Length; i++)
            {
                if (float.Parse(probabilityArray[i]) == 0)
                {
                    _rateText[i].color = Color.white;
                }
                else
                {
                    _rateText[i].color = Color.yellow;
                }
                _rateText[i].text = probabilityArray[i] + "%";

            }
        }

        foreach (GameObject searchSoldier in _soldiers)
        {
            if (searchSoldier.GetComponent<UnitData>()._data._multiUnit.Length != 0)
            {
                int ck_Result = 0;
                
                temp_ck_List = new List<UnitData>();
                ck_List = new List<UnitData>();
                for (int i = 0; i < searchSoldier.GetComponent<UnitData>()._data._multiUnit.Length; i++)
                {
                    if (SearchSacrifice(searchSoldier.GetComponent<UnitData>()._data._multiUnit[i]))
                    {
                        ck_Result++;
                    }
                }

                if (ck_Result == searchSoldier.GetComponent<UnitData>()._data._multiUnit.Length)
                {
                    ck_List = temp_ck_List;
                    foreach (UnitData unitdata in ck_List)
                    {
                        unitdata.PossibleRev = true;
                    }
                }
            }
        }
    }
}

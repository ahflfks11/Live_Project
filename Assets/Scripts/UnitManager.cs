using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    public List<GameObject> _soldiers;
    public List<GameObject> _spawnList;

    public List<GameObject> _tutorialSoldierList;

    int number = 0;
    public int tutorialSoldierNumber = 0;
    float rndRangeX = 3f; // Default 3f
    float rndRangeY = 3f;
    private float normalEnforceDmg = 0;
    private float rareEnforceDmg = 0;
    private float legendEnforceDmg = 0;
    private float hiddenEnforceDmg = 0;

    public int maxCount = 10;

    public int maxSpawnlevel = 0;
    
    public List<UnitData> ck_List = new List<UnitData>();
    List<UnitData> temp_ck_List = new List<UnitData>();

    [SerializeField] private DamageFonts _dmgFont;

    public DamageFonts DmgFont { get => _dmgFont; set => _dmgFont = value; }
    public float NormalEnforceDmg { get => normalEnforceDmg; set => normalEnforceDmg = value; }
    public float RareEnforceDmg { get => rareEnforceDmg; set => rareEnforceDmg = value; }
    public float LegendEnforceDmg { get => legendEnforceDmg; set => legendEnforceDmg = value; }
    public float HiddenEnforceDmg { get => hiddenEnforceDmg; set => hiddenEnforceDmg = value; }

    public bool CheckSpawn(GameObject[] _data)
    {
        if (_data.Length > 0)
            return false;

        return true;
    }

    private void Start()
    {
        if (DataManager.Instance.MyHeroList.Count > 0)
        {
            _soldiers = new List<GameObject>();

            for (int i = 0; i < DataManager.Instance.MyHeroList.Count; i++)
            {
                if (DataManager.Instance.MyHeroList[i]._data.specialUnit)
                {
                    _soldiers.Add(DataManager.Instance.MyHeroList[i].gameObject);
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
    }

    public void Enforce_Normal()
    {
        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpNormalCoin())
        {
            NormalEnforceDmg += 1;
        }
    }

    public void Enforce_Rare()
    {
        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpRareCoin())
        {
            rareEnforceDmg += 1;
        }
    }

    public void Enforce_Legend()
    {
        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpLegendCoin())
        {
            legendEnforceDmg += 1;
        }
    }

    public void Enforce_Hidden()
    {
        if (GameManager.Instance.UiManager.EnforceUI.SetDmgUpHiddenCoin())
        {
            hiddenEnforceDmg += 1;
        }
    }

    public void LevelUp()
    {
        if (GameManager.Instance.UiManager.EnforceUI.SetLevelText())
        {
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

    public void SelectSpawn(GameObject _Solider, Vector3 _pos)
    {
        if (GameManager.Instance.Gold < GameManager.Instance.RequireGold)
            return;

        GameObject _unit;

        _unit = Instantiate(_Solider, _pos, Quaternion.identity);

        _unit.GetComponent<UnitData>().number = number;
        _unit.GetComponent<UnitData>().UnitManager = this;
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

    //버튼 클릭시 소환 함수
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


    //진화시 소환 함수
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
    }

    //특수 소환
    public void SpecialSpawn(int _spawnNum, Vector3 _pos, bool _randomPosition)
    {
        if (GameManager.Instance.Gold < (maxSpawnlevel + 1) * 10)
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
        int index = _unit.name.IndexOf("(Clone)");
        if (index > 0)
            _unit.name = _unit.name.Substring(0, index);
        number++;
        _spawnList.Add(_unit);
        GameManager.Instance.Gold -= (maxSpawnlevel + 1) * 10;
    }

    public void SpawnUnit()
    {
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

        int rnd_UnitNumber = Random.Range(0, _soldiers.Count);

        if (!CheckSpawn(_soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data._multiUnit) || _soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data.rarelityLevel > maxSpawnlevel || _soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data.specialUnit)
        {
            SpawnUnit();
            return;
        }

        Spawn(rnd_UnitNumber, pos, true);
    }

    public void SpecialSpawnUnit()
    {
        Vector3 pos = GameManager.Instance.myArea.position;

        int rnd_UnitNumber = Random.Range(0, _soldiers.Count);

        if (!CheckSpawn(_soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data._multiUnit) || _soldiers[rnd_UnitNumber].GetComponent<UnitData>()._data.rarelityLevel > maxSpawnlevel + 1)
        {
            SpecialSpawnUnit();
            return;
        }

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
            Time.timeScale = 2f;
        else
            Time.timeScale = 1f;
    }

    private void Update()
    {
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

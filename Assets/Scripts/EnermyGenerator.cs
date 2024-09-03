using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyGenerator : MonoBehaviour
{
    public EnermyControl[] _enermyList;
    public EnermyControl[] _bossList;
    public Transform[] wayPoint;
    int stageLimitNumber = 50;
    int tempStageLimitNumber = 0;
    float createTime = 2f;
    int wave = 0;
    Coroutine _launcher;

    [SerializeField] private EnermyControl _tutorialBossPrefab;
    [SerializeField] private EnermyControl[] _missionBossPrefab;

    bool _bossSpawn;


    public IEnumerator GeneratorLauncher()
    {
        while (true)
        {
            if (!GameManager.Instance.IsBoss)
            {
                if (stageLimitNumber <= tempStageLimitNumber)
                {
                    yield return null;
                }
                else
                {
                    GameObject _enermy = Instantiate(_enermyList[wave - 1].gameObject, wayPoint[0].position, Quaternion.identity);
                    _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
                    tempStageLimitNumber++;
                    GameManager.Instance.EnermyCount++;
                    yield return new WaitForSeconds(createTime - GameManager.Instance.GameSpeed);
                }
            }
            else
            {
                if (!_bossSpawn)
                {
                    GameObject _enermy = Instantiate(_bossList[GameManager.Instance.BossCount].gameObject, wayPoint[0].position, Quaternion.identity);
                    _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
                    GameManager.Instance.EnermyCount++;
                    _enermy.gameObject.name = "Boss";
                    _bossSpawn = true;
                }

                yield return null;
            }
        }
    }

    IEnumerator tutorialGenerator(int _number)
    {
        int i = 0;
        while (i < _number)
        {
            if (i > 0 && _number > 1 && GameManager.Instance.GameStop)
                yield return null;
            else
            {
                GameObject _enermy = Instantiate(_enermyList[wave - 1].gameObject, wayPoint[0].position, Quaternion.identity);
                _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
                GameManager.Instance.EnermyCount++;
                i++;

                if (GameManager.Instance.GameStop)
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(createTime - GameManager.Instance.GameSpeed);
                }
            }
        }

        if (_number > 1)
            GameManager.Instance._SpawnComplete = true;
    }

    public void Tutorial_BossSpawn()
    {
        GameObject _enermy = Instantiate(_tutorialBossPrefab.gameObject, wayPoint[0].position, Quaternion.identity);
        _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
        _enermy.gameObject.name = "Boss";
        GameManager.Instance.EnermyCount++;
    }

    public void MissionBossSpawn(int _missionNumber)
    {
        GameObject _enermy = Instantiate(_missionBossPrefab[_missionNumber].gameObject, wayPoint[0].position, Quaternion.identity);
        _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
        _enermy.gameObject.name = "MissionBoss" + _missionNumber;
        GameManager.Instance.EnermyCount++;
    }

    public void SpawnEnemy(int _enremyCount)
    {
        StartCoroutine(tutorialGenerator(_enremyCount));
    }

    public void enermy()
    {
        GameObject _enermy = Instantiate(_enermyList[wave - 1].gameObject, wayPoint[0].position, Quaternion.identity);
        _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
        GameManager.Instance.EnermyCount++;
        GameManager.Instance._SpawnComplete = false;
    }

    private void Start()
    {
        wave = GameManager.Instance.Wave;
    }

    public void StartWave()
    {
        _launcher = StartCoroutine(GeneratorLauncher());
    }

    private void Update()
    {
        if (_launcher == null && GameManager.Instance.GameStart)
            StartWave();

        if (GameManager.Instance.IsBoss)
            tempStageLimitNumber = stageLimitNumber;

        if (wave != GameManager.Instance.Wave)
        {
            _bossSpawn = false;
            if (_enermyList.Length >= GameManager.Instance.Wave)
            {
                wave = GameManager.Instance.Wave;
                tempStageLimitNumber = 0;
            }
        }

        if(JsonParseManager.Instance.Tutorial && GameManager.Instance.EnermyCount <= 0 && JsonParseManager.Instance._txtNumber == 18 && GameManager.Instance._SpawnComplete)
        {
            DialogueManager.Instance.TalkLauncher(19);
        }
    }
}

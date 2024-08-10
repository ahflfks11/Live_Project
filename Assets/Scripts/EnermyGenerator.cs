using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyGenerator : MonoBehaviour
{
    public EnermyControl[] _enermyList;
    public EnermyControl[] _bossList;
    public Transform[] wayPoint;
    int stageLimitNumber = 30;
    int tempStageLimitNumber = 0;
    float createTime = 2f;
    int wave = 0;
    Coroutine _launcher;

    bool _bossSpawn;

    public IEnumerator GeneratorLauncher()
    {
        while (true)
        {
            if (!GameManager.Instance.IsBoss)
            {
                if (stageLimitNumber == tempStageLimitNumber)
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
    }

    public void SpawnEnemy(int _enremyCount)
    {
        StartCoroutine(tutorialGenerator(_enremyCount));
    }

    public void enermy()
    {
        GameObject _enermy = Instantiate(_enermyList[wave - 1].gameObject, wayPoint[0].position, Quaternion.identity);
        _enermy.GetComponent<EnermyControl>().WayPoint = wayPoint;
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

        if (wave != GameManager.Instance.Wave)
        {
            _bossSpawn = false;
            if (_enermyList.Length >= GameManager.Instance.Wave)
            {
                wave = GameManager.Instance.Wave;
            }

            tempStageLimitNumber = 0;
        }
    }
}

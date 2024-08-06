using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyGenerator : MonoBehaviour
{
    public EnermyControl[] _enermyList;
    public Transform[] wayPoint;
    int stageLimitNumber = 30;
    int tempStageLimitNumber = 0;
    float createTime = 2f;
    int wave = 0;
    Coroutine _launcher;

    public IEnumerator GeneratorLauncher()
    {
        while (true)
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
    }

    private void Start()
    {
        wave = GameManager.Instance.Wave;
        StartWave();
    }

    public void StartWave()
    {
        _launcher = StartCoroutine(GeneratorLauncher());
    }

    private void Update()
    {
        if (wave != GameManager.Instance.Wave)
        {
            if (_enermyList.Length >= GameManager.Instance.Wave)
            {
                wave = GameManager.Instance.Wave;
            }

            tempStageLimitNumber = 0;
        }
    }
}

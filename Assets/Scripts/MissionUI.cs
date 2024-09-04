using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class MissionUI : MonoBehaviour
{
    private DOTweenVisualManager _visualManager;
    private EnermyGenerator _enermyGenerator;
    [SerializeField] private GameObject[] _Lock;

    [SerializeField] private Image[] _coolTimeImages;

    [SerializeField] private int[] _LimitWave;

    [SerializeField] private int[] _spawnCool;

    // Start is called before the first frame update
    void Start()
    {
        _visualManager = transform.GetComponent<DOTweenVisualManager>();
        _enermyGenerator = FindAnyObjectByType<EnermyGenerator>();
        GameManager.Instance.EliteSpawnState = new EliteSpawnType[_Lock.Length];

        for (int i = 0; i < GameManager.Instance.EliteSpawnState.Length; i++)
        {
            GameManager.Instance.EliteSpawnState[i] = EliteSpawnType.소환가능;
        }
    }

    public void SpawnMissionMob(int _spawnNumber)
    {
        if (GameManager.Instance.EliteSpawnState[_spawnNumber] != EliteSpawnType.소환가능)
            return;

        _enermyGenerator.MissionBossSpawn(_spawnNumber);

        GameManager.Instance.EliteSpawnState[_spawnNumber] = EliteSpawnType.소환중;
    }

    public void SetUI()
    {
        if (_visualManager.enabled)
            _visualManager.enabled = false;
        else
            _visualManager.enabled = true;
    }

    private void Update()
    {
        for (int i = 0; i < _LimitWave.Length; i++)
        {
            if (_LimitWave[i] >= 0)
            {
                if (GameManager.Instance.Wave >= _LimitWave[i])
                {
                    if (_Lock[i].activeSelf)
                    {
                        _Lock[i].SetActive(false);
                    }
                }
            }
            else
            {
                if (GameManager.Instance.BossCount >= Mathf.Abs(_LimitWave[i]))
                {
                    if (_Lock[i].activeSelf)
                    {
                        _Lock[i].SetActive(false);
                    }
                }
            }
        }

        for (int i = 0; i < GameManager.Instance.EliteSpawnState.Length; i++)
        {
            if (GameManager.Instance.EliteSpawnState[i] == EliteSpawnType.소환중)
            {
                _coolTimeImages[i].fillAmount = 1;
            }
            else if (GameManager.Instance.EliteSpawnState[i] == EliteSpawnType.소환해제)
            {
                _coolTimeImages[i].fillAmount -= 1 * Time.smoothDeltaTime / _spawnCool[i];

                if (_coolTimeImages[i].fillAmount == 0)
                {
                    GameManager.Instance.EliteSpawnState[i] = EliteSpawnType.소환가능;
                }
            }
        }
    }
}

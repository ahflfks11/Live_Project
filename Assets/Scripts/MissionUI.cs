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

    [SerializeField] private Text[] _remainCoolTimeText;

    float[] _remainCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        _visualManager = transform.GetComponent<DOTweenVisualManager>();
        _enermyGenerator = FindAnyObjectByType<EnermyGenerator>();
        GameManager.Instance.EliteSpawnState = new EliteSpawnType[_Lock.Length];

        for (int i = 0; i < GameManager.Instance.EliteSpawnState.Length; i++)
        {
            GameManager.Instance.EliteSpawnState[i] = EliteSpawnType.��ȯ�Ұ�;
        }

        _remainCoolTime = new float[_spawnCool.Length];

        for (int i = 0; i < _remainCoolTime.Length; i++)
        {
            _remainCoolTime[i] = _spawnCool[i];
            _remainCoolTimeText[i].text = "";
        }
    }

    public void SpawnMissionMob(int _spawnNumber)
    {
        if (GameManager.Instance.EliteSpawnState[_spawnNumber] != EliteSpawnType.��ȯ����)
            return;

        _enermyGenerator.MissionBossSpawn(_spawnNumber);

        GameManager.Instance.EliteSpawnState[_spawnNumber] = EliteSpawnType.��ȯ��;
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
                        GameManager.Instance.EliteSpawnState[i] = EliteSpawnType.��ȯ����;
                    }
                }
            }
            else
            {
                if ((GameManager.Instance.BossCount >= Mathf.Abs(_LimitWave[i]) - 1 && GameManager.Instance.KillBoss == Mathf.Abs(_LimitWave[i])) || GameManager.Instance.BossCount > Mathf.Abs(_LimitWave[i]) - 1)
                {
                    if (_Lock[i].activeSelf)
                    {
                        _Lock[i].SetActive(false);
                        GameManager.Instance.EliteSpawnState[i] = EliteSpawnType.��ȯ����;
                    }
                }
            }
        }

        for (int i = 0; i < GameManager.Instance.EliteSpawnState.Length; i++)
        {
            if (GameManager.Instance.EliteSpawnState[i] == EliteSpawnType.��ȯ��)
            {
                _coolTimeImages[i].fillAmount = 1;
                _remainCoolTime[i] = _spawnCool[i];
                _remainCoolTimeText[i].text = "��ȯ��";
            }
            else if (GameManager.Instance.EliteSpawnState[i] == EliteSpawnType.��ȯ����)
            {
                _remainCoolTime[i] -= Time.unscaledDeltaTime;
                _coolTimeImages[i].fillAmount = _remainCoolTime[i] / _spawnCool[i];
                _remainCoolTimeText[i].text = Mathf.Ceil(_remainCoolTime[i]).ToString() + "s";

                if (_coolTimeImages[i].fillAmount == 0)
                {
                    _remainCoolTimeText[i].text = "";
                    GameManager.Instance.EliteSpawnState[i] = EliteSpawnType.��ȯ����;
                }
            }
        }
    }
}

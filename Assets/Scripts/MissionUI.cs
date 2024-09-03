using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MissionUI : MonoBehaviour
{
    private DOTweenVisualManager _visualManager;
    private EnermyGenerator _enermyGenerator;
    [SerializeField] private GameObject[] _Lock;

    [SerializeField] private int[] _LimitWave;
    // Start is called before the first frame update
    void Start()
    {
        _visualManager = transform.GetComponent<DOTweenVisualManager>();
        _enermyGenerator = FindAnyObjectByType<EnermyGenerator>();
    }

    public void SpawnMissionMob(int _spawnNumber)
    {
        if (_Lock[_spawnNumber].activeSelf)
            return;

        _enermyGenerator.MissionBossSpawn(_spawnNumber);
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
            if(GameManager.Instance.Wave >= _LimitWave[i])
            {
                if (_Lock[i].activeSelf)
                {
                    _Lock[i].SetActive(false);
                }
            }
        }
    }
}

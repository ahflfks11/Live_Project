using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] private Image _profile;
    [SerializeField] private Image _rarelity_background;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _comment_Text;
    [SerializeField] private TMP_Text _dmg_Text;
    [SerializeField] private TMP_Text _distance_Text;
    [SerializeField] private TMP_Text _up_dmg_Text;
    [SerializeField] private TMP_Text _up_distance_Text;
    GPGSManager _gpgsManager;

    private UnitData _unit;
    private int _number;

    private void Start()
    {
        _gpgsManager = FindObjectOfType<GPGSManager>();
    }

    public void Save()
    {
        if (DataManager.Instance.NowLevel.Count < 1)
            return;

        if (DataManager.Instance.MyHeroLevel[Number] > DataManager.Instance.NowLevel[Number])
        {
            DataManager.Instance.NowLevel[Number] += 1;
            _gpgsManager.SaveLevel(DataManager.Instance.NowLevel);
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public UnitData Unit { get => _unit; set => _unit = value; }
    public int Number { get => _number; set => _number = value; }
}

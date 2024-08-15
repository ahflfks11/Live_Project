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
    [SerializeField] private Image _levelUpImage;
    [SerializeField] private TMP_Text _herolevelText;
    [SerializeField] private TMP_Text _levelupText;

    GPGSManager _gpgsManager;

    private DataManager.Data _data;
    private int _number;

    private void Start()
    {
        _gpgsManager = FindObjectOfType<GPGSManager>();
    }

    private void Update()
    {
        if (DataManager.Instance.NowLevel.Count > 0)
        {
            if (DataManager.Instance.NowLevel[_number] > 0)
            {
                _up_dmg_Text.text = "+" + DataManager.Instance.NowLevel[_number].ToString();
                _up_distance_Text.text = "+" + (0.1f * DataManager.Instance.NowLevel[_number]).ToString();
            }
            else
            {
                _up_dmg_Text.text = "";
                _up_distance_Text.text = "";
            }

            if (DataManager.Instance.MyHeroLevel[Number] > DataManager.Instance.NowLevel[Number])
            {
                _levelUpImage.enabled = true;
            }
            else
            {
                _levelUpImage.enabled = false;
            }

            _herolevelText.text = DataManager.Instance.NowLevel[Number] + "/" + DataManager.Instance.MyHeroLevel[Number];

            if (DataManager.Instance.NowLevel[_number]!=2)
            {
                _levelupText.text = string.Format("{0:n0}", 5000 * (DataManager.Instance.NowLevel[_number] + 1)) + "G";
            }
            else
            {
                _levelupText.text = "MAX";
            }
        }
    }

    public void Save()
    {
        if (DataManager.Instance.NowLevel.Count < 1)
            return;

        if (int.Parse(LobbyManager.Instance._lobbyUIManager._CoinText.text.ToString()) < 5000 * (DataManager.Instance.NowLevel[_number] + 1))
            return;

        if (DataManager.Instance.MyHeroLevel[Number] > DataManager.Instance.NowLevel[Number])
        {
            DataManager.Instance.NowLevel[Number] += 1;
            _gpgsManager.SaveLevel(DataManager.Instance.NowLevel);

            _gpgsManager.LeastGold(5000 * (DataManager.Instance.NowLevel[_number]), LobbyManager.Instance._lobbyUIManager._CoinText);
        }
    }

    public void Close()
    {
        _up_dmg_Text.text = "";
        _up_distance_Text.text = "";
        this.gameObject.SetActive(false);
    }

    public int Number { get => _number; set => _number = value; }
    public DataManager.Data Data { get => _data; set => _data = value; }
    public Image Rarelity_background { get => _rarelity_background; set => _rarelity_background = value; }
    public Image Profile { get => _profile; set => _profile = value; }
    public TMP_Text NameText { get => _nameText; set => _nameText = value; }
    public TMP_Text Comment_Text { get => _comment_Text; set => _comment_Text = value; }
    public TMP_Text Dmg_Text { get => _dmg_Text; set => _dmg_Text = value; }
    public TMP_Text Distance_Text { get => _distance_Text; set => _distance_Text = value; }
}

using UnityEngine;
using UnityEngine.UI;

public class Character_Icon : MonoBehaviour
{
    [SerializeField] private Image _spr;
    [SerializeField] private Image _back_Image;
    [SerializeField] private Image _star_Image;
    [SerializeField] private Image _up_Image;
    private DataManager.Data _myData;
    private int _number;
    public Image Spr { get => _spr; set => _spr = value; }
    public Image Back_Image { get => _back_Image; set => _back_Image = value; }
    public Image Star_Image { get => _star_Image; set => _star_Image = value; }
    public DataManager.Data MyData { get => _myData; set => _myData = value; }
    public int Number { get => _number; set => _number = value; }

    public void SetImage(DataManager.Data _data)
    {
        _spr.sprite = _data._unit._spr;
        _back_Image.sprite = DataManager.Instance.BackgroundSprite[_data._rarelity];
    }

    public void OpenCharacterStatus()
    {
        LobbyUIManager _lobbyUIManager = FindObjectOfType<LobbyUIManager>();
        _lobbyUIManager.Status.gameObject.SetActive(true);
        _lobbyUIManager.Status.Data = _myData;
        _lobbyUIManager.Status.Number = Number;
        _lobbyUIManager.Status.Profile.sprite = _spr.sprite;
        _lobbyUIManager.Status.Rarelity_background.sprite = _back_Image.sprite;
        _lobbyUIManager.Status.NameText.text = _myData._unit._data._unit_name;
        _lobbyUIManager.Status.Comment_Text.text = _myData._unit._data._comment;
        _lobbyUIManager.Status.Dmg_Text.text = _myData._unit._data.dmg.ToString();
        _lobbyUIManager.Status.Distance_Text.text = _myData._unit.FindRange.ToString();
        //Status.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (DataManager.Instance.NowLevel.Count > 0)
        {
            if (DataManager.Instance.MyHeroLevel[_number] > DataManager.Instance.NowLevel[_number])
            {
                _up_Image.enabled = true;
            }
            else
            {
                _up_Image.enabled = false;
            }

            if (DataManager.Instance.NowLevel[_number] < 3)
            {
                _star_Image.sprite = DataManager.Instance.StarImgs[DataManager.Instance.NowLevel[_number]];
            }
            else
            {
                _star_Image.sprite = DataManager.Instance.StarImgs[2];
            }
        }
    }

}

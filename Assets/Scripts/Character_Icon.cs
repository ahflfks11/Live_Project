using UnityEngine;
using UnityEngine.UI;

public class Character_Icon : MonoBehaviour
{
    [SerializeField] private Image _spr;
    [SerializeField] private Image _back_Image;
    [SerializeField] private Image _star_Image;

    private DataManager.Data _myData;

    public Image Spr { get => _spr; set => _spr = value; }
    public Image Back_Image { get => _back_Image; set => _back_Image = value; }
    public Image Star_Image { get => _star_Image; set => _star_Image = value; }
    public DataManager.Data MyData { get => _myData; set => _myData = value; }

    public void SetImage(DataManager.Data _data)
    {
        _spr.sprite = _data._unit._spr;
        _back_Image.sprite = DataManager.Instance.BackgroundSprite[_data._rarelity];
    }
}

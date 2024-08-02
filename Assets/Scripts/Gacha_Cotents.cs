using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gacha_Cotents : MonoBehaviour
{
    [SerializeField] Image[] _Gacha_Box_Image;
    [SerializeField] Image[] _Gacha_Background_Image;

    public void SetupGachaImage(UnitData _unit, int _dataNumber, int _rarelity)
    {
        if (_unit._spr != null)
        {
            _Gacha_Box_Image[_dataNumber].sprite = _unit._spr;
            _Gacha_Background_Image[_dataNumber].sprite = DataManager.Instance.BackgroundSprite[_rarelity];
        }
    }

    public void CleanGachaImage()
    {
        for (int i = 0; i < _Gacha_Box_Image.Length; i++)
        {
            _Gacha_Box_Image[i].sprite = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}

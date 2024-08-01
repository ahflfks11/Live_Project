using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gacha_Cotents : MonoBehaviour
{
    [SerializeField]
    Image[] _Gacha_Box_Image;
    
    public void SetupGachaImage(UnitData _unit, int _dataNumber)
    {
        if (_unit._spr != null)
        {
            _Gacha_Box_Image[_dataNumber].sprite = _unit._spr;
            _Gacha_Box_Image[_dataNumber].color = _unit._myRareColor.color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}

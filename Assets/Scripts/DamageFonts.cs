using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DamageFonts : MonoBehaviour
{
    [SerializeField] private Text _dmgText;
    [SerializeField] private Font[] _textList;
    
    public Text DmgText { get => _dmgText; set => _dmgText = value; }

    public void SetText(float _dmg, Transform _pos, UnitData.UnitType _type)
    {
        if (_type == UnitData.UnitType.Melee_Attack)
            _dmgText.font = _textList[0];
        else
            _dmgText.font = _textList[1];

        _dmgText.text = _dmg.ToString();
        transform.DOMove(new Vector3(_pos.position.x + 0.5f, _pos.position.y + 0.5f, 0f), 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

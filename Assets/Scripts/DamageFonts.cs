using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DamageFonts : MonoBehaviour
{
    [SerializeField] private Text _dmgText;
    [SerializeField] private Font[] _textList;
    [SerializeField] private DOTweenAnimation _tweenAni;
    public Text DmgText { get => _dmgText; set => _dmgText = value; }

    public void SetText(float _dmg, Transform _pos, UnitData.UnitType _type)
    {
        transform.DOKill();
        transform.localPosition = _pos.localPosition;
        _tweenAni.DORestart();

        if (_type == UnitData.UnitType.Melee_Attack)
            _dmgText.font = _textList[0];
        else
            _dmgText.font = _textList[1];

        _dmgText.text = _dmg.ToString();
        transform.DOMove(new Vector3(_pos.position.x + 0.5f, _pos.position.y + 0.5f, 0f), 1f);
    }
}

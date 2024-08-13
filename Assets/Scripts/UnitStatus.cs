using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    [SerializeField] private Text _DmgText;
    [SerializeField] private Text _AttackCountText;

    public void SetUI(float _dmg, float _attackCount)
    {
        _DmgText.text = _dmg.ToString();
        if (_attackCount != 1)
            _AttackCountText.text = "(" + _attackCount.ToString() + ")";
        else
            _AttackCountText.text = "(¥‹¿œ)";
    }
}

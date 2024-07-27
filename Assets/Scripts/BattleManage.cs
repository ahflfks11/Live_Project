using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManage : MonoBehaviour
{
    UnitData _unitdata;
    // Start is called before the first frame update
    void Start()
    {
        _unitdata = transform.GetComponentInParent<UnitData>();
    }

    public void NormalAttack()
    {
        _unitdata.NormalAttack();
    }

    public void SpecialDamage(float _dmg)
    {
        _unitdata.SpecialAttack(_dmg);
    }
}

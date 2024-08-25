using System.Collections;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject, ISkill
{
    public abstract void ApplySkill(EnermyControl target);

}
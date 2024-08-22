using System.Collections;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject, ISkill
{
    public abstract void ApplySkill(EnermyControl target);

}

[CreateAssetMenu(menuName = "Skills/SlowSkill")]
public class SlowSkill : UnitSkill
{
    public float slowAmount = 0.5f;  // 슬로우 비율 (50%)
    public float duration = 5f;      // 지속 시간 (5초)

    public override void ApplySkill(EnermyControl target)
    {
        target.Slow(slowAmount, duration);
        Debug.Log("test");
    }
}
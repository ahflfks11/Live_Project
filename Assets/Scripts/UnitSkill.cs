using System.Collections;
using UnityEngine;

public abstract class UnitSkill : ScriptableObject, ISkill
{
    public abstract void ApplySkill(GameObject target);

    public abstract void RemoveSkill(GameObject target);
}

[CreateAssetMenu(menuName = "Skills/SlowSkill")]
public class SlowSkill : UnitSkill
{
    public float slowAmount = 0.5f;  // 슬로우 비율 (50%)
    public float duration = 5f;      // 지속 시간 (5초)

    private float originalSpeed;

    public override void ApplySkill(GameObject target)
    {
        // 대상의 속도를 감소시키는 로직
        EnermyControl movement = target.GetComponent<EnermyControl>();
        if (movement != null)
        {
            originalSpeed = movement._data.speed;
            movement._data.speed *= slowAmount;
            target.GetComponent<MonoBehaviour>().StartCoroutine(RemoveSkillAfterDelay(target));
        }
    }

    public override void RemoveSkill(GameObject target)
    {
        EnermyControl movement = target.GetComponent<EnermyControl>();
        //이 부분 수정해야댐
        if (movement != null)
        {
            movement._data.speed = originalSpeed;  // 원래 속도로 복귀
        }
    }

    private IEnumerator RemoveSkillAfterDelay(GameObject target)
    {
        yield return new WaitForSeconds(duration);
        RemoveSkill(target);
    }
}
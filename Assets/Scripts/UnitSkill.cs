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
    public float slowAmount = 0.5f;  // ���ο� ���� (50%)
    public float duration = 5f;      // ���� �ð� (5��)

    private float originalSpeed;

    public override void ApplySkill(GameObject target)
    {
        // ����� �ӵ��� ���ҽ�Ű�� ����
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
        //�� �κ� �����ؾߴ�
        if (movement != null)
        {
            movement._data.speed = originalSpeed;  // ���� �ӵ��� ����
        }
    }

    private IEnumerator RemoveSkillAfterDelay(GameObject target)
    {
        yield return new WaitForSeconds(duration);
        RemoveSkill(target);
    }
}
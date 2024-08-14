using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    // �� ��޺� Ȯ���� ������ �Լ�
    public int GetUnitGrade(int playerLevel)
    {
        List<float> gradeProbabilities = GetGradeProbabilities(playerLevel);
        return GetRandomGrade(gradeProbabilities);
    }

    // Ȯ���� ���� ����� ��ȯ�ϴ� �Լ�
    private int GetRandomGrade(List<float> probabilities)
    {
        float total = 0;
        foreach (float prob in probabilities)
        {
            total += prob;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probabilities.Count; i++)
        {
            if (randomPoint < probabilities[i])
            {
                return i + 1;  // ����� 1���� �����ϹǷ� 1�� ������
            }
            else
            {
                randomPoint -= probabilities[i];
            }
        }

        return probabilities.Count;  // ����ó��: ���� ��� Ȯ���� �����ƴٸ� ���� ���� ��� ��ȯ
    }

    // �÷��̾� ������ ���� Ȯ���� ��ȯ�ϴ� �Լ�
    private List<float> GetGradeProbabilities(int playerLevel)
    {
        switch (playerLevel)
        {
            case 1:
                return new List<float> { 80f, 20f, 0f, 0f };
            case 2:
                return new List<float> { 50f, 42f, 8f, 0f };
            case 3:
                return new List<float> { 30f, 44f, 25f, 1f };
            case 4:
                return new List<float> { 2f, 45f, 45f, 8f };
            default:
                return new List<float> { 100f, 0f, 0f, 0f };
        }
    }

    // ���� �÷��̾� ������ ���� Ȯ���� ���ڿ� �迭�� ��ȯ�ϴ� �Լ�
    public string[] GetGradeProbabilitiesAsArray(int playerLevel)
    {
        List<float> probabilities = GetGradeProbabilities(playerLevel);
        string[] result = new string[probabilities.Count];

        for (int i = 0; i < probabilities.Count; i++)
        {
            result[i] = probabilities[i].ToString();
        }

        return result;
    }
}

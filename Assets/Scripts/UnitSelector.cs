using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    // 각 등급별 확률을 설정한 함수
    public int GetUnitGrade(int playerLevel)
    {
        List<float> gradeProbabilities = GetGradeProbabilities(playerLevel);
        return GetRandomGrade(gradeProbabilities);
    }

    // 확률에 따라 등급을 반환하는 함수
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
                return i + 1;  // 등급은 1부터 시작하므로 1을 더해줌
            }
            else
            {
                randomPoint -= probabilities[i];
            }
        }

        return probabilities.Count;  // 예외처리: 만약 모든 확률을 지나쳤다면 가장 높은 등급 반환
    }

    // 플레이어 레벨에 따른 확률을 반환하는 함수
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

    // 현재 플레이어 레벨에 따른 확률을 문자열 배열로 반환하는 함수
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

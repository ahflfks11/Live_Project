using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewSizeController : MonoBehaviour
{
    public float targetAspect = 9.0f / 16.0f; // 목표 화면 비율 (9:16 비율)

    void Start()
    {
        SetCameraView();
    }

    void SetCameraView()
    {
        // 현재 화면의 비율
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 화면 비율 차이 계산
        float scaleHeight = windowAspect / targetAspect;

        // 카메라 컴포넌트 가져오기
        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // 현재 화면 비율이 목표 비율보다 크면, 카메라의 viewport를 조정해줘야 함
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // 현재 화면 비율이 목표 비율보다 작으면, 카메라의 viewport를 조정해줘야 함
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}

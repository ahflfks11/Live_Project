using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    // 목표 화면 비율 (세로 모드 9:16)
    public float targetAspectRatio = 9.0f / 16.0f;

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // 현재 화면의 종횡비
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleWidth = windowAspectRatio / targetAspectRatio;

        Camera camera = GetComponent<Camera>();

        if (scaleWidth < 1.0f)
        {
            // 현재 화면의 너비가 기준 종횡비보다 더 작으면
            // 카메라의 orthographic size를 증가시켜야 합니다.
            camera.orthographicSize = camera.orthographicSize / scaleWidth;
        }
        else
        {
            // 현재 화면의 높이가 기준 종횡비보다 더 작으면
            // 카메라의 viewport rect를 수정해야 합니다.
            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}

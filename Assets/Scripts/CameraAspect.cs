using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    // ��ǥ ȭ�� ���� (���� ��� 9:16)
    public float targetAspectRatio = 9.0f / 16.0f;

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // ���� ȭ���� ��Ⱦ��
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleWidth = windowAspectRatio / targetAspectRatio;

        Camera camera = GetComponent<Camera>();

        if (scaleWidth < 1.0f)
        {
            // ���� ȭ���� �ʺ� ���� ��Ⱦ�񺸴� �� ������
            // ī�޶��� orthographic size�� �������Ѿ� �մϴ�.
            camera.orthographicSize = camera.orthographicSize / scaleWidth;
        }
        else
        {
            // ���� ȭ���� ���̰� ���� ��Ⱦ�񺸴� �� ������
            // ī�޶��� viewport rect�� �����ؾ� �մϴ�.
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

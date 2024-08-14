using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewSizeController : MonoBehaviour
{
    public float targetAspect = 9.0f / 16.0f; // ��ǥ ȭ�� ���� (9:16 ����)

    void Start()
    {
        SetCameraView();
    }

    void SetCameraView()
    {
        // ���� ȭ���� ����
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // ȭ�� ���� ���� ���
        float scaleHeight = windowAspect / targetAspect;

        // ī�޶� ������Ʈ ��������
        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // ���� ȭ�� ������ ��ǥ �������� ũ��, ī�޶��� viewport�� ��������� ��
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            // ���� ȭ�� ������ ��ǥ �������� ������, ī�޶��� viewport�� ��������� ��
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

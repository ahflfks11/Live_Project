using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadUI : MonoBehaviour
{
    DOTweenVisualManager _visualManager;

    public void Close()
    {
        _visualManager.enabled = false;
    }

    public void EscapeGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    private void Awake()
    {
        _visualManager = GetComponent<DOTweenVisualManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_visualManager.enabled)
            {
                _visualManager.enabled = true;
            }
            else
            {
                EscapeGame();
            }
        }
    }
}

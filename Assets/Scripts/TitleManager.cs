using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    GPGSManager _gpgsManager;
    public GameObject menuPanel;
    public GameObject loadingPanel;
    public Sprite[] _backgrounds;
    public UnityEngine.UI.Image _spr;

    private void Start()
    {
        _spr.sprite = _backgrounds[Random.Range(0, _backgrounds.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (_gpgsManager == null)
        {
            _gpgsManager = FindObjectOfType<GPGSManager>();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (BackEnd.Backend.IsInitialized == false)
            {
                menuPanel.SetActive(false);
                loadingPanel.SetActive(true);
            }
            else
            {
                menuPanel.SetActive(true);
                loadingPanel.SetActive(false);
            }
        }
        else
        {
            menuPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }
    }

    public void CheckLobbyScene()
    {
        if (_gpgsManager.CheckUser())
        {
            Transitioner.Instance.TransitionToScene(1);
        }
        else
        {
            Transitioner.Instance.TransitionToScene(2);
        }
    }

    public void GuestLogin()
    {
        if (_gpgsManager == null)
            return;

        _gpgsManager.GuestLogin();
    }
}

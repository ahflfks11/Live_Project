using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    GPGSManager _gpgsManager;
    public GameObject menuPanel;
    public GameObject loadingPanel;

    private void Awake()
    {
        if (GameObject.Find("DataManager"))
        {
            Destroy(GameObject.Find("DataManager"));
        }

        if (GameObject.Find("TalkCanvas"))
        {
            Destroy(GameObject.Find("TalkCanvas"));
        }
    }

    private void Start()
    {
        AudioManager.instance.TitleBgm(true);
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
        _gpgsManager.GuestLogin();
    }
}

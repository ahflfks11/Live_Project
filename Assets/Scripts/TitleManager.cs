using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    GPGSManager _gpgsManager;
    static TitleManager instance;
    public GameObject menuPanel;
    public GameObject loadingPanel;
    public GameObject _versionUI;

    public static TitleManager Instance { get => instance; set => instance = value; }

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

        instance = this;
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
            _gpgsManager.GpgsInit();
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GPGSManager.Instance.GuestLogin();
            Debug.Log("계정 생성 완료");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GPGSManager.Instance.RemoveGuest();
            Debug.Log("계정 삭제 완료");
        }
    }

    public void VersionUpdate()
    {
        GPGSManager.Instance.OpenStoreLink();
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

    public void GoogleLogin()
    {
        _gpgsManager.GPGSLogin();
    }
}

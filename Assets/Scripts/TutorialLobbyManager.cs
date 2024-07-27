using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLobbyManager : MonoBehaviour
{
    public GPGSManager _gpgsManager;

    // Update is called once per frame
    void Update()
    {
        if (_gpgsManager == null)
        {
            _gpgsManager = FindObjectOfType<GPGSManager>();
        }
    }
}

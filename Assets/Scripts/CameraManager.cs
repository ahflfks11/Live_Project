using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public CameraSizeHandler[] _sizeList;

    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.deviceName.Contains("Fold"))
        {
            _sizeList[1].enabled = true;
            _sizeList[0].enabled = false;
        }
        else if (!SystemInfo.deviceName.Contains("DESKTOP"))
        {
            _sizeList[1].enabled = false;
            _sizeList[0].enabled = true;
        }
    }
}

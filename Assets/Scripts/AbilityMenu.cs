using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityMenu : MonoBehaviour
{
    EnforceUI _mainUI;
    Image _mySpr;
    // Start is called before the first frame update
    void Start()
    {
        _mainUI = FindObjectOfType<EnforceUI>();
        _mySpr = transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainUI.transform.localScale.x > 0)
        {
            _mySpr.raycastTarget = true;
        }
        else
        {
            _mySpr.raycastTarget = false;
        }
    }
}

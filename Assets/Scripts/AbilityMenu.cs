using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityMenu : MonoBehaviour
{
    EnforceUI _mainUI;
    Image _mySpr;
    [SerializeField] TMPro.TMP_Text _rarelityText;
    [SerializeField] int _number;
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

        if (_rarelityText != null)
        {
            _rarelityText.text = GameManager.Instance.RarilitySpawnCount[_number].ToString();
        }
    }
}

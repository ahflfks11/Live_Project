using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text waveText;
    public Text _goldText;
    public Text _monsterText;
    public Text _requireText;
    public CartoonFX.CFXR_ParticleText _waveText;
    public Text _timerText;
    public GameObject _RevolutionPanel;
    public Transform _content;
    public GameObject _RevolutionImageObject;
    [SerializeField] private DOTweenVisualManager _EnforceShopPanel;

    public void OpenPanel(UnitData _data)
    {
        GameObject[] _image = GameObject.FindGameObjectsWithTag("Rev");

        for (int i = 0; i < _image.Length; i++)
        {
            Destroy(_image[i]);
        }

        _RevolutionPanel.SetActive(false);

        if (_data._data.EvolutionAvailability)
        {
            bool _isOpen = false;

            for (int i = 0; i < GameManager.Instance.UnitManager._soldiers.Count; i++)
            {
                if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._unit == _data._data._unit && _data._data.rarelityLevel + 1 == GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data.rarelityLevel)
                {
                    GameObject _revolutionObject = Instantiate(_RevolutionImageObject, _content.position, Quaternion.identity);
                    _revolutionObject.transform.SetParent(_content);
                    _revolutionObject.transform.localScale = _RevolutionImageObject.transform.localScale;
                    _revolutionObject.GetComponent<Revolution_Sprite>()._spr.sprite = GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._spr;
                    _revolutionObject.transform.localPosition = Vector3.zero;
                    _isOpen = true;
                }
                else
                {
                    if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._multiUnit.Length != 0)
                    {
                        for (int j = 0; j < GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._multiUnit.Length; j++)
                        {
                            if (GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._data._multiUnit[j] == GameManager.Instance.UnitManager._soldiers[i])
                            {
                                GameObject _revolutionObject = Instantiate(_RevolutionImageObject, _content.position, Quaternion.identity);
                                _revolutionObject.transform.SetParent(_content);
                                _revolutionObject.transform.localScale = _RevolutionImageObject.transform.localScale;
                                _revolutionObject.GetComponent<Revolution_Sprite>()._spr.sprite = GameManager.Instance.UnitManager._soldiers[i].GetComponent<UnitData>()._spr;
                                _revolutionObject.transform.localPosition = Vector3.zero;
                                _isOpen = true;
                            }
                        }
                    }
                }
            }

            if (_isOpen)
            {
                _RevolutionPanel.SetActive(true);
            }
        }
    }

    public void EnforceShopPanel()
    {
        if (_EnforceShopPanel.enabled)
        {
            _EnforceShopPanel.enabled = false;
        }
        else
        {
            _EnforceShopPanel.enabled = true;
        }
    }

    public void Wave(float _level)
    {
        GameObject text_wave_Object = Instantiate(_waveText.gameObject, Vector2.zero, Quaternion.identity);
        text_wave_Object.GetComponent<CartoonFX.CFXR_ParticleText>().UpdateText("WAVE" + _level);
        waveText.text = "WAVE" + _level;
    }



    // Start is called before the first frame update
    void Start()
    {
        Wave(GameManager.Instance.Wave);
        _RevolutionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _goldText.text = string.Format("{0:#,0}", GameManager.Instance.Gold);
        _monsterText.text = GameManager.Instance.EnermyCount.ToString();
        _requireText.text = GameManager.Instance.RequireGold + " G";

        if (GameManager.Instance.Sectime >= 10f)
        {
            _timerText.text = GameManager.Instance.Mintime + ":" + GameManager.Instance.Sectime;
        }
        else
        {
            _timerText.text = GameManager.Instance.Mintime + ":0" + GameManager.Instance.Sectime;
        }
    }
}

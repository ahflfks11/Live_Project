using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RemoveUI : MonoBehaviour
{
    DOTweenVisualManager _visualManager;

    public Toggle _commonToggle;
    public Toggle _unCommonToggle;
    int _commonGold = 3;
    int _unCommonGold = 9;
    AudioSource _myAudio;
    [SerializeField] Text _goldText;
    void Start()
    {
        _visualManager = GetComponent<DOTweenVisualManager>();
        _myAudio = GetComponent<AudioSource>();
    }

    public void SetUI()
    {
        if (_visualManager.enabled)
            _visualManager.enabled = false;
        else
            _visualManager.enabled = true;
    }

    public void RemoveUnit()
    {
        int Gold = 0;

        if (!_commonToggle.isOn && !_unCommonToggle.isOn)
            return;

        for (int i = GameManager.Instance._unitObject.Length - 1; i >= 0; i--)
        {
            if (_commonToggle.isOn)
            {
                if (GameManager.Instance._unitObject[i]._data.rarelityLevel == 0 && !GameManager.Instance._unitObject[i]._data.specialUnit)
                {
                    Gold += _commonGold;
                    GameManager.Instance._unitObject[i].DestroyThisObject();
                }
            }

            if (_unCommonToggle.isOn)
            {
                if (GameManager.Instance._unitObject[i]._data.rarelityLevel == 1 && !GameManager.Instance._unitObject[i]._data.specialUnit)
                {
                    Gold += _unCommonGold;
                    GameManager.Instance._unitObject[i].DestroyThisObject();
                }
            }
        }

        GameManager.Instance.Gold += Gold;
        _myAudio.clip = AudioManager.instance.SetClip(0);
        _myAudio.Play();
    }

    private void Update()
    {
        if (!_visualManager.enabled)
            return;
        
        int Gold = 0;

        for (int i = GameManager.Instance._unitObject.Length - 1; i >= 0; i--)
        {
            if (_commonToggle.isOn)
            {
                if (GameManager.Instance._unitObject[i]._data.rarelityLevel == 0 && !GameManager.Instance._unitObject[i]._data.specialUnit)
                {
                    Gold += _commonGold;
                }
            }

            if (_unCommonToggle.isOn)
            {
                if (GameManager.Instance._unitObject[i]._data.rarelityLevel == 1 && !GameManager.Instance._unitObject[i]._data.specialUnit)
                {
                    Gold += _unCommonGold;
                }
            }
        }

        _goldText.text = "예상 판매 가격 : " + Gold + "G";
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    //상태 이상 정보
    [SerializeField] private Text _statusText;
    [SerializeField] private Image _defenceIcon;
    [SerializeField] private Text _defenceText;

    public void SetStatus(string _status, float _statusSpeed)
    {
        _statusText.text = _status;
        _statusText.color = Color.red;
    }

    public void DefenceStatus(float _defence)
    {
        if (!_defenceIcon.enabled)
            _defenceIcon.enabled = true;

        _defenceText.text = _defence.ToString();
    }

    public void ResetStatus()
    {
        _statusText.text = "";
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    //상태 이상 정보
    [SerializeField] private Text _statusText;

    public void SetStatus(string _status, float _statusSpeed)
    {
        _statusText.text = _status;
        _statusText.color = Color.red;
    }

    public void ResetStatus()
    {
        _statusText.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RankingUI : MonoBehaviour
{
    [SerializeField] private Image _rankingImage;
    [SerializeField] private Text _rankingText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _highStageText;
    [SerializeField] private Text _legendText;

    public void SetRankUI(int _number, string _name, int _highStage, int _legend)
    {
        if (_number <= 3)
        {
            _rankingImage.enabled = true;
            _rankingImage.sprite = LobbyManager.Instance._lobbyUIManager._rankImg[_number - 1];
        }
        else
        {
            _rankingImage.enabled = false;
            _rankingText.text = _number.ToString();
        }

        _nameText.text = _name;
        _highStageText.text = _highStage.ToString();
        _legendText.text = _legend.ToString();
    }
}

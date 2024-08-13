using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EndGameUI : MonoBehaviour
{
    public TMP_Text  _stageClearText;
    public TMP_Text _legendText;
    public TMP_Text _rareText;
    public TMP_Text _crystalText;
    public TMP_Text _goldText;
    public DOTweenVisualManager _panel;

    public void SetUI(int _stage, int _legendCount, int _rareCount)
    {
        _panel.enabled = true;
        _stageClearText.text = _stage.ToString();
        _legendText.text = _legendCount.ToString();
        _rareText.text = _rareCount.ToString();
        _goldText.text = (100 * _stage).ToString();
        _crystalText.text = GameManager.Instance.CalculateCrystals(_stage).ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Image[] resultImages;

    public void SetUI(int _stage, int _legendCount, int _rareCount)
    {
        if(_stage < 40)
        {
            resultImages[0].gameObject.SetActive(true);
        }
        else
        {
            resultImages[1].gameObject.SetActive(true);
            resultImages[2].gameObject.SetActive(true);
        }
        _panel.enabled = true;
        _stageClearText.text = _stage.ToString();
        _legendText.text = _legendCount.ToString();
        _rareText.text = _rareCount.ToString();
        _goldText.text = (100 * _stage).ToString();
        _crystalText.text = GameManager.Instance.CalculateCrystals(_stage).ToString();
    }
}

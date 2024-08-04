using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DamageFonts : MonoBehaviour
{
    [SerializeField] private Text _dmgText;
    public Text DmgText { get => _dmgText; set => _dmgText = value; }

    public void SetText(float _dmg, Transform _pos)
    {
        _dmgText.text = _dmg.ToString();
        transform.DOMove(new Vector3(_pos.position.x + 0.5f, _pos.position.y + 0.5f, 0f), 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

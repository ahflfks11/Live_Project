using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnermyControl : MonoBehaviour
{

    private Transform[] wayPoint;
    public Slider _hp_Img;
    public Text _hpText;
    int wayNumber = 1;
    public Transform[] WayPoint { get => wayPoint; set => wayPoint = value; }

    [System.Serializable]
    public struct Data
    {
        public float HP;
        public float defence;
        public int gainGold;
        public float speed;
    }

    public Data _data;

    float maxHP;

    public void MobHit(float _dmg)
    {
        _data.HP -= _dmg;
    }

    private void Start()
    {
        maxHP = _data.HP;
    }

    private void Update()
    {
        _hp_Img.value = Mathf.Lerp(_hp_Img.value, _data.HP / maxHP, 3f * Time.deltaTime);
        _hp_Img.value = Mathf.Clamp01(_hp_Img.value);
        _hpText.text = _data.HP.ToString();


        if (_data.HP <= 0)
        {
            GameManager.Instance.Gold += _data.gainGold;
            GameManager.Instance.EnermyCount--;
            Destroy(this.gameObject);
            return;
        }

        if (Vector3.Distance(transform.position, wayPoint[wayNumber].position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoint[wayNumber].position, _data.speed * Time.deltaTime);
        }
        else
        {
            if (wayNumber + 1 <= 3)
            {
                wayNumber++;
            }
            else
            {
                wayNumber = 0;
            }
        }
    }
}

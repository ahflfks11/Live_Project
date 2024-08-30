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
    int enermyTutorialNumber = 0;
    [SerializeField] bool _isLastBoss;
    [SerializeField] ItemManager.SetItem _itemData;
    public bool _isBoss;
    MonsterStatus _status;

    bool _isStatus;

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

    float _originSpeed;

    float _nowSpeed;

    public void MobHit(float _dmg)
    {
        _data.HP -= _dmg;
    }

    private void Start()
    {
        _status = GetComponentInChildren<MonsterStatus>();
        maxHP = _data.HP;
        _originSpeed = _data.speed;
        _nowSpeed = _data.speed;
    }

    public void Slow(float _amount, float _duration, bool _duplication)
    {
        StopAllCoroutines();

        if (!_duplication)
        {
            if (_nowSpeed != _data.speed)
                return;

            _nowSpeed = _data.speed * _amount;
        }
        else
        {
            _nowSpeed *= _amount;
        }

        _status.SetStatus("이동 속도 감소", _amount);
        StartCoroutine(RemoveSkillAfterDelay(_duration, "Slow"));
        _isStatus = true;
    }

    private IEnumerator RemoveSkillAfterDelay(float _duration, string _type)
    {
        yield return new WaitForSeconds(_duration);
        switch (_type)
        {
            case "Slow":
                _nowSpeed = _originSpeed;
                break;
        }

        _isStatus = false;
        _status.ResetStatus();
    }

    private void Update()
    {
        _hp_Img.value = Mathf.Lerp(_hp_Img.value, _data.HP / maxHP, 3f * Time.deltaTime);
        _hp_Img.value = Mathf.Clamp01(_hp_Img.value);
        _hpText.text = _data.HP.ToString();


        if (_data.HP <= 0)
        {
            if (JsonParseManager.Instance._txtNumber < 14 && JsonParseManager.Instance.Tutorial && enermyTutorialNumber == 0)
            {
                DialogueManager.Instance.TalkLauncher(14);
            }

            GameManager.Instance.Gold += _data.gainGold;
            EnermyCoinText _coin = Instantiate(GameManager.Instance.CoinText, transform.position, Quaternion.identity);

            _coin.SetCoin(_data.gainGold);
            GameManager.Instance.EnermyCount--;

            if (_isBoss)
            {
                if (JsonParseManager.Instance.Tutorial)
                {
                    DialogueManager.Instance.TalkLauncher(31);
                }
                else
                {
                    if (_isLastBoss)
                    {
                        GameManager.Instance.UiManager.EndGameUI(true);
                    }
                    else
                    {
                        GameManager.Instance.UiManager.SkipUI();
                    }
                }

            }

            if (_itemData._type != ItemManager.ItemType.None)
            {
                ItemManager.Instance.SpawnItem(_itemData.itemID, _itemData._type, _itemData._itemCount, _itemData._name, _itemData._itemColor);
            }

            Destroy(this.gameObject);
            return;
        }

        if (GameManager.Instance.GameStop)
            return;

        if (Vector3.Distance(transform.position, wayPoint[wayNumber].position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wayPoint[wayNumber].position, _nowSpeed * Time.deltaTime);
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

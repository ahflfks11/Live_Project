using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{
    [SerializeField] UnitManager _unitManager;
    Animator _animator;

    public enum Unit
    {
        None,
        Warrior,
        Archor,
        Magition,
        Ninja,
        Hidden,
        Babari
    }

    public enum UnitType
    {
        None,
        Melee_Attack,
        Ranged_Attack
    }

    [System.Serializable]
    public struct Data
    {
        public Unit _unit;
        public UnitType _type;
        public GameObject[] _multiUnit;
        public string _unit_name;
        public string _comment;
        public int attackCount;
        public double weight;
        public float attackDelay;
        public float attackSpeed;
        public AudioManager.skillSfx soundNumber;
        public float speed;
        public float dmg;
        public int rarelityLevel;
        public bool EvolutionAvailability;
        public bool specialUnit;
    };

    //effect
    public GameObject _Weapon;

    public Data _data;
    public int number;
    Vector3 _tempPosition;
    public bool onselect;
    bool _isAttack;
    bool _possibleRev;

    public Collider2D targetEnermy;
    public Collider2D[] enermys;
    public LayerMask EnemyLayer; //레이어 선택
    public float FindRange = 4f; //범위
    float _increaseDmg = 0f;
    float _increaseRange = 0f;
    public Sprite _spr;

    public Material _sprMat;
    public SpriteRenderer _myRareColor;
    int tutorialNumber = 0;
    CircleRangeVisualizer _visualizer;
    private Vector3 targetEnemy;   // 현재 타겟 적

    UnitStatus _status;

    public UnitManager UnitManager { get => _unitManager; set => _unitManager = value; }
    public Vector3 TempPosition { get => _tempPosition; set => _tempPosition = value; }
    public bool PossibleRev { get => _possibleRev; set => _possibleRev = value; }

    private void Start()
    {
        //StartCoroutine(AutoBattle());
        _visualizer = transform.GetComponent<CircleRangeVisualizer>();
        if (transform.GetComponentInChildren<Animator>())
        {
            _animator = transform.GetComponentInChildren<Animator>();
        }

        _sprMat = transform.GetComponentInChildren<SpriteRenderer>().material;

        if (!_data.specialUnit)
            _myRareColor.color = GameManager.Instance._rareColor[_data.rarelityLevel];
        else
            _myRareColor.color = GameManager.Instance._hiddenColor;

        _unitManager = FindObjectOfType<UnitManager>();

        if (GameObject.Find("DataManager"))
        {
            for (int index = 0; index < DataManager.Instance.MyHeroList.Count; index++)
            {
                if (DataManager.Instance.MyHeroList[index]._data._unit == _data._unit && DataManager.Instance.MyHeroList[index]._data.rarelityLevel == _data.rarelityLevel)
                {
                    _increaseDmg = 1 * DataManager.Instance.NowLevel[index];
                    _increaseRange = 0.1f * DataManager.Instance.NowLevel[index];
                    FindRange += _increaseRange;
                    break;
                }
            }
        }
        _status = gameObject.GetComponentInChildren<UnitStatus>();
        _status.gameObject.SetActive(false);
        FindClosestEnemy();
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enermy");
        float closestDistance = Mathf.Infinity;
        targetEnemy = Vector3.zero;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                targetEnemy = enemy.transform.position;
            }
        }
    }

    void MoveTowardsTarget()
    {
        if (!onselect)
        {
            if (Vector2.Distance(targetEnemy, transform.position) > FindRange && !_isAttack)
            {
                if (!_animator.GetBool("Walk"))
                    _animator.SetBool("Walk", true);
                Vector2 direction = (targetEnemy - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, targetEnemy, _data.speed * Time.deltaTime);
            }
            else
            {
                _animator.SetBool("Walk", false);
                targetEnemy = Vector3.zero;
            }
        }
    }

    public void NormalAttack()
    {

        float _dmg = _data.dmg;

        if (_data.specialUnit)
        {
            _dmg = _data.dmg + _unitManager.HiddenEnforceDmg;
        }
        else
        {
            if (_data.rarelityLevel <= 1)
            {
                _dmg = _data.dmg + _unitManager.NormalEnforceDmg;
            }
            else if (_data.rarelityLevel == 2)
            {
                _dmg = _data.dmg + _unitManager.RareEnforceDmg;
            }
            else
            {
                _dmg = _data.dmg + _unitManager.LegendEnforceDmg;
            }
        }

        _dmg = _dmg + _increaseDmg;

        if (_data.attackCount == 1)
        {
            if (targetEnermy != null && !onselect && _Weapon != null)
            {
                targetEnermy.GetComponent<EnermyControl>().MobHit(_dmg);
                GameObject _effect = Instantiate(_Weapon, targetEnermy.transform.position, Quaternion.identity);
                DamageFonts _dmgFont = Instantiate(_unitManager.DmgFont, targetEnermy.transform.position, Quaternion.identity);

                _dmgFont.SetText(_dmg, targetEnermy.transform, _data._type);
                Destroy(_effect, 1f);
            }
        }
        else if (_data.attackCount > 1)
        {
            if (enermys != null && !onselect && _Weapon != null)
            {
                if (enermys.Length <= _data.attackCount)
                {
                    for (int i = 0; i < enermys.Length; i++)
                    {
                        try
                        {
                            enermys[i].GetComponent<EnermyControl>().MobHit(_dmg);
                            GameObject _effect = Instantiate(_Weapon, enermys[i].transform.position, Quaternion.identity);
                            DamageFonts _dmgFont = Instantiate(_unitManager.DmgFont, enermys[i].transform.position, Quaternion.identity);
                            _dmgFont.SetText(_dmg, enermys[i].transform, _data._type);
                            Destroy(_effect, 1f);
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _data.attackCount; i++)
                    {
                        try
                        {
                            enermys[i].GetComponent<EnermyControl>().MobHit(_dmg);
                            GameObject _effect = Instantiate(_Weapon, enermys[i].transform.position, Quaternion.identity);
                            DamageFonts _dmgFont = Instantiate(_unitManager.DmgFont, enermys[i].transform.position, Quaternion.identity);
                            _dmgFont.SetText(_dmg, enermys[i].transform, _data._type);
                            Destroy(_effect, 1f);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        AudioManager.instance.PlaySkillSfx(_data.soundNumber);
    }

    public void SpecialAttack(float _takeDmg)
    {
        if (targetEnermy != null && !onselect)
        {
            targetEnermy.GetComponent<EnermyControl>().MobHit(_takeDmg);
        }
    }

    public void IsSelect()
    {
        if (JsonParseManager.Instance.Tutorial)
        {
            if (GameObject.Find("Arrow" + number) && JsonParseManager.Instance._txtNumber <= 13)
            {
                Destroy(GameObject.Find("Arrow" + number));
            }

            if (tutorialNumber == 1)
            {
                DialogueManager.Instance.TalkLauncher(13);
                tutorialNumber = 2;
            }
        }

        onselect = true;
        _animator.SetBool("Walk", false);
        _visualizer.DrawCircle();
        _status.gameObject.SetActive(true);

        float _dmg = _data.dmg;

        if (_data.specialUnit)
        {
            _dmg = _data.dmg + _unitManager.HiddenEnforceDmg;
        }
        else
        {
            if (_data.rarelityLevel <= 1)
            {
                _dmg = _data.dmg + _unitManager.NormalEnforceDmg;
            }
            else if (_data.rarelityLevel == 2)
            {
                _dmg = _data.dmg + _unitManager.RareEnforceDmg;
            }
            else
            {
                _dmg = _data.dmg + _unitManager.LegendEnforceDmg;
            }
        }

        _dmg = _dmg + _increaseDmg;

        _status.SetUI(_dmg, _data.attackCount);
    }

    public void Deselect()
    {
        if (JsonParseManager.Instance.Tutorial && tutorialNumber == 2)
        {
            DialogueManager.Instance.CompleteCurrentSentence();
            DialogueManager.Instance._dialogueBox.enabled = false;
            GameManager.Instance.GameStop = false;
            tutorialNumber = 3;
        }

        _visualizer.ClearCircle();
        onselect = false;
        _status.gameObject.SetActive(false);
        if (Vector3.Distance(transform.position, TempPosition) < 0.1f)
        {
            Revolution();
            //GameManager.Instance.UiManager.OpenPanel(this);
        }
    }

    public void Revolution()
    {
        if (!_data.EvolutionAvailability)
            return;

        int count = 0;
        List<int> _numberList = new List<int>();

        for (int i = 0; i < UnitManager._spawnList.Count; i++)
        {
            if (UnitManager._spawnList[i] != this.gameObject && _data._unit == UnitManager._spawnList[i].GetComponent<UnitData>()._data._unit && UnitManager._spawnList[i].GetComponent<UnitData>()._data.rarelityLevel == _data.rarelityLevel)
            {
                _numberList.Add(UnitManager._spawnList[i].GetComponent<UnitData>().number);
                count++;

                if (count == 2)
                    break;
            }
        }

        if (count >= 2)
        {
            for (int i = 0; i < _numberList.Count; i++)
            {
                for (int j = UnitManager._spawnList.Count - 1; j >= 0; j--)
                {
                    if (UnitManager._spawnList[j].GetComponent<UnitData>().number == _numberList[i])
                    {
                        GameObject _tempObject = UnitManager._spawnList[j];
                        UnitManager._spawnList.RemoveAt(j);
                        Destroy(_tempObject);
                        break;
                    }
                }
            }

            for (int i = _unitManager._spawnList.Count - 1; i >= 0; i--)
            {
                if (_unitManager._spawnList[i].GetComponent<UnitData>().number == number)
                {
                    _unitManager._spawnList.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < UnitManager._soldiers.Count; i++)
            {
                if (UnitManager._soldiers[i].GetComponent<UnitData>()._data._unit == _data._unit && _data.rarelityLevel + 1 == UnitManager._soldiers[i].GetComponent<UnitData>()._data.rarelityLevel)
                {
                    UnitManager.RevolutionSpawn(i, transform.position, false);
                }
            }
            Destroy(this.gameObject);
        }
    }

    public void Attack()
    {
        if (_data.attackCount == 1)
        {
            var enermyObj = Physics2D.OverlapCircle(transform.position, FindRange, EnemyLayer);
            targetEnermy = enermyObj;
        }
        else if (_data.attackCount > 1)
        {
            enermys = Physics2D.OverlapCircleAll(transform.position, FindRange, EnemyLayer);
        }
    }

    private void Update()
    {
        if (targetEnemy != Vector3.zero)
            MoveTowardsTarget();

        FindRevolution(GameManager.Instance._unitObject);

        if (_animator != null)
        {
            if (!onselect)
            {
                if (_data.attackCount <= 1)
                {
                    if (targetEnermy != null && (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")))
                    {
                        _isAttack = true;
                        if (JsonParseManager.Instance.Tutorial && JsonParseManager.Instance._txtNumber < 12)
                        {
                            if (tutorialNumber == 0)
                                tutorialNumber = 1;
                        }
                    }
                    else if (targetEnermy == null)
                    {
                        _isAttack = false;
                        if (JsonParseManager.Instance.Tutorial && tutorialNumber == 1 && JsonParseManager.Instance._txtNumber < 12)
                        {
                            DialogueManager.Instance.TalkLauncher(12);
                        }
                    }
                }
                else
                {
                    if (enermys.Length != 0 && (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")))
                    {
                        _isAttack = true;
                    }
                    else if (enermys.Length == 0)
                    {
                        _isAttack = false;
                    }
                }
            }
            else
            {
                _isAttack = false;
            }

            if (_isAttack)
            {
                _animator.SetBool("Attack", true);

            }
            else
            {
                _animator.SetBool("Attack", false);
            }
        }

        if (!onselect)
        {
            TempPosition = transform.position;
        }

        Attack();

        if (_possibleRev)
        {
            bool possible = false;
            foreach (UnitData unitdata in GameManager.Instance.UnitManager.ck_List)
            {
                if (unitdata == this)
                {
                    possible = true;
                }
            }

            if (!_data.specialUnit)
            {
                if (!possible)
                {
                    _sprMat.SetColor("_OuterOutlineColor", Color.yellow);
                    _possibleRev = false;
                }
                else
                {
                    _sprMat.SetColor("_OuterOutlineColor", Color.green);
                    _sprMat.SetFloat("_OuterOutlineFade", 1f);
                    return;
                }
            }
        }
    }

    void FindRevolution(UnitData[] _dataObject)
    {
        int count = 0;

        foreach (UnitData unitdata in _dataObject)
        {
            if (unitdata != this && unitdata._data._unit == _data._unit && unitdata._data.rarelityLevel == _data.rarelityLevel)
            {
                count++;
            }
        }

        if (count >= 2 && !_data.specialUnit && _data.EvolutionAvailability)
        {
            if (JsonParseManager.Instance.Tutorial)
            {
                if (!GameObject.Find("Arrow" + number))
                {
                    GameManager.Instance.UiManager.SetArrow(transform, Vector3.zero, "Arrow" + number);
                }
            }

            _sprMat.SetFloat("_OuterOutlineFade", 1f);
        }
        else
        {
            _sprMat.SetFloat("_OuterOutlineFade", 0f);
        }
    }

    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FindRange);
    }
}

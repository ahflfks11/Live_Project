using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    [SerializeField] private Sprite[] _backgroundSprite;
    [SerializeField] private Sprite[] _starImgs;

    [System.Serializable]
    public struct Data
    {
        public int _rarelity;
        public double _weight;
        public UnitData _unit;
    }

    public Data[] _data;

    //보유한 영웅 리스트
    [SerializeField] private List<UnitData> myHeroList = new List<UnitData>();
    //보유한 영웅의 중복 리스트
    [SerializeField] private List<int> myHeroLevel = new List<int>();

    //보유한 영웅의 현재 레벨
    [SerializeField] private List<int> nowLevel = new List<int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static DataManager Instance { get => instance; set => instance = value; }
    public List<UnitData> MyHeroList { get => myHeroList; set => myHeroList = value; }
    public List<int> MyHeroLevel { get => myHeroLevel; set => myHeroLevel = value; }
    public Sprite[] BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }
    public List<int> NowLevel { get => nowLevel; set => nowLevel = value; }
    public Sprite[] StarImgs { get => _starImgs; set => _starImgs = value; }
}

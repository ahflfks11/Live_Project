using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    [SerializeField] private Sprite[] _backgroundSprite;

    [System.Serializable]
    public struct Data
    {
        public int _rarelity;
        public double _weight;
        public UnitData _unit;
    }

    public Data[] _data;

    [SerializeField] private List<UnitData> myHeroList = new List<UnitData>();
    [SerializeField] private List<int> myHeroLevel = new List<int>();

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public struct Data
    {
        public int _rarelity;
        public double _weight;
        public int _number;
        public UnitData _unit;
    }

    public Data _data;

    public static DataManager Instance { get => instance; set => instance = value; }
}

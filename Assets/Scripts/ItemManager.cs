using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;

    public static ItemManager Instance { get => _instance; set => _instance = value; }

    [System.Serializable]
    public enum ItemType
    {
        None,
        Scroll
    }

    [System.Serializable]
    public struct SetItem
    {
        public string _name;
        public ItemType _type;
        public int itemID;
        public int _itemCount;
        public Color _itemColor;
    }

    [SerializeField] Sprite[] _itemImage;

    [SerializeField] ItemObject _itemObjects;

    private void Awake()
    {
        _instance = this;
    }

    public Sprite ItemGenerator(ItemType _type, int _itemId, int _itemCount)
    {
        Sprite item_Img = null;
        switch (_type)
        {
            case ItemType.Scroll:
                GameManager.Instance.RarilitySpawnCount[_itemId] += _itemCount;
                item_Img = _itemImage[0];
                break;
        }

        return item_Img;
    }

    public void SpawnItem(int _itemNumber, ItemType _type, int _count, string _itemName, Color _itemTextColor)
    {
        Vector3 _pos = new Vector3(-1, -6, 0);
        ItemObject _item = Instantiate(_itemObjects, _pos, Quaternion.identity);
        _item.gameObject.transform.DOMove(new Vector3(-1, -4, 0), 1f);
        _item.Spawn(_itemName, _count, _type, _itemNumber, _itemTextColor);
    }
}

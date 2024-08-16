using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemObject : MonoBehaviour
{
    [SerializeField] Text _ItemNameText;
    [SerializeField] Image _itemSprite;

    private void Start()
    {

    }

    public void Spawn(string _itemName, int _itemCount, ItemManager.ItemType _type, int _itemID, Color _itemColor)
    {
        _ItemNameText.text = _itemName + "x" + _itemCount.ToString();
        _itemSprite.sprite = ItemManager.Instance.ItemGenerator(_type, _itemID);
        _ItemNameText.color = _itemColor;
    }
}

using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum ItemType
{
    Weapon = 0,
    Armor = 1,
    Portion = 2,
    Resource = 4,
    PlaceObject = 8,
    Tile = 16,
    Tool = 32,
    xxxx = 64,
    xxxxx = 128,
    xxxxxx = 256
}
[System.Serializable]
public enum ItemTier
{
    Cooper,
    Steel,
    Silver,
    Gold
}
[System.Serializable]
public enum ItemGrade
{
    common,
    rare,
    unique,
    epic
}

[System.Serializable]
[CreateAssetMenu(fileName = "xxx_ItemName", menuName = "ItemObject/Item")]
public class Item : ScriptableObject
{
    public int ID{ get { return id; } }
    public Sprite sprite { get { return _sprite; } } //임시
    public ItemType itemType { get { return _itemType; } }
    public ItemGrade ItemGrade { get { return itemGrade; } }

    public string ItemName { get { return itemName; } }
    public int itemLimit { get { return _itemLimit; } }
    public string Description { get { return description; } }
    public ItemTier ItemTier { get { return itemTier; } }

    [SerializeField] private int id;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ItemType _itemType = ItemType.Resource;
    [SerializeField] private ItemGrade itemGrade;
    [SerializeField] private string itemName = "";
    [SerializeField] private int _itemLimit = 99;
    [SerializeField] private string description;
    [SerializeField] private ItemTier itemTier;
}

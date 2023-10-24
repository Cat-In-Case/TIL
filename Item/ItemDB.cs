using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "DataBase/ItemDB")]
public class ItemDB : ScriptableObject
{
    [SerializeField] private string path;

    [SerializeReference]
    private List<Item> items = new List<Item>();

    public Item GetItem(int id)
    {
        return items[id];
    }
}

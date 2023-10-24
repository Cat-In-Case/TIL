using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "xxx_SeedItemName", menuName = "ItemObject/SeedItem")]
public class ResourceItem_Seed : ResourceItem
{
    public Plant plantData;
    public GameObject Prefab { get { return prefab; } }
    [SerializeField] private GameObject prefab;
}

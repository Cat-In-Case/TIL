using System.Collections;
using UnityEngine;

public enum ResourceType
{
    Ore,
    Seed
}

[CreateAssetMenu(fileName = "xxx_ResourceItemName", menuName = "ItemObject/ResourceItem")]
public class ResourceItem : Item
{
    public virtual ResourceType ResourceType { get { return resourceType; } }
    [SerializeField] protected ResourceType resourceType = ResourceType.Ore;
}
